using FluentResults;
using MapsterMapper;
using PeopleFinder.Application.Common.Errors;
using PeopleFinder.Application.Models.Friend;
using PeopleFinder.Application.Services.ProfileServices;
using PeopleFinder.Domain.Common.Models;
using PeopleFinder.Domain.Common.Pagination.Cursor;
using PeopleFinder.Domain.Common.Pagination.Page;
using PeopleFinder.Domain.Entities;
using PeopleFinder.Domain.Repositories.Common;

namespace PeopleFinder.Application.Services.RelationshipServices;

public class RelationshipService : IRelationshipService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RelationshipService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<Relationship>> SendFriendRequest(int profileId, FriendshipRequest friendshipRequest)
    {
        if (profileId == friendshipRequest.FriendProfileId)
            return Result.Fail(FriendRequestErrors.SelfFriendshipError);

        var senderProfile = await _unitOfWork.ProfileRepository.GetOne(profileId);
            
        if (senderProfile is null)
            return Result.Fail(ProfileErrors.ProfileNotFound);
            
        var receiverProfile = await _unitOfWork.ProfileRepository.GetOne(friendshipRequest.FriendProfileId);

        if (receiverProfile is null)
            return Result.Fail(FriendRequestErrors.FriendRequestReceiverNotFound);

        await using var transaction = await _unitOfWork.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);
        
        var relationship = await _unitOfWork.RelationshipRepository.GetRelationshipByProfileIdsAsync(senderProfile.Id, receiverProfile.Id);
        if (relationship != null)
        {
            return Result.Fail(FriendRequestErrors.FriendRequestAlreadySent);
        }

        var friendRequest = new Relationship()
        {
            InitiatorProfile = senderProfile,
            ReceiverProfile = receiverProfile,
            Status = RelationshipStatus.Pending,
            SentAt = DateTime.UtcNow
        };
            
        await _unitOfWork.RelationshipRepository.AddAsync(friendRequest);
        
        await _unitOfWork.SaveAsync();
        await transaction.CommitAsync();
            
        return friendRequest;
    }
    

    public async Task<Result> ApproveFriendRequest(int profileId, int friendSenderId)
    {
        var relationship = await _unitOfWork.RelationshipRepository.GetRequest(friendSenderId, profileId);
        
        if (relationship is null || relationship.Status != RelationshipStatus.Pending)
            return Result.Fail(FriendRequestErrors.FriendRequestNotFound);

        relationship.Status = RelationshipStatus.Approved;
        relationship.AcknowledgeAt = DateTime.UtcNow;

        await _unitOfWork.SaveAsync();
        
        return Result.Ok();
    }

    public async Task<Result> CancelFriendRequest(int profileId, int receiverId)
    {
        var friendRequest = await _unitOfWork.RelationshipRepository.GetSentRequest(profileId, receiverId);

        if (friendRequest is null ||friendRequest.Status != RelationshipStatus.Pending)
            return Result.Fail(FriendRequestErrors.FriendRequestNotFound);
        
        _unitOfWork.RelationshipRepository.Delete(friendRequest);
        
        await _unitOfWork.SaveAsync();
        
        return Result.Ok();
    }
    public async Task<Result> RejectFriendRequest(int profileId, int friendSenderId)
    {

        var friendRequest = await _unitOfWork.RelationshipRepository.GetRequest(profileId, friendSenderId);

        if (friendRequest is null)
            return Result.Fail(FriendRequestErrors.FriendRequestNotFound);
        
        friendRequest.Status = RelationshipStatus.Rejected;
        
        await _unitOfWork.SaveAsync();
        
        return Result.Ok();
    }
    

    public async Task<Result> RemoveFriend(int profileId, int friendId)
    {
         var friendship = await _unitOfWork.RelationshipRepository.GetRelationshipByProfileIdsAsync(profileId, friendId);
         if(friendship is null)
             return Result.Fail(FriendRequestErrors.FriendshipNotFound);
         
         _unitOfWork.RelationshipRepository.Delete(friendship);
         await _unitOfWork.SaveAsync();
         return Result.Ok();
            
    }
    

    public async Task<Result<CursorList<RelationshipProfile>>> GetFriends(int profileId, 
        CursorPaginationParams<DateTime> cursorPaginationParams, string? searchQuery = null)
    {
        var profile = await _unitOfWork.ProfileRepository.GetOne(profileId);
        if (profile is null)
            return Result.Fail(ProfileErrors.ProfileNotFound);

        var friends = await _unitOfWork.ProfileRepository
            .GetFriends(profile.Id, cursorPaginationParams.PageSize, cursorPaginationParams.After, searchQuery);
        
        
        
        return friends;
    }
  
    public async Task<Result<CursorList<Relationship>>> GetFriendshipRequestUpdates(int profileId, CursorPaginationParams<DateTime> pagedPaginationParams)
    {
        var profile = await _unitOfWork.ProfileRepository.GetByIdAsync(profileId);
        if (profile is null)
            return Result.Fail(ProfileErrors.ProfileNotFound);
        
        int totalFriendshipRequests = await _unitOfWork.RelationshipRepository.GetRequestsWithoutAnswerCount(profile.Id);
        
        var requests = await _unitOfWork.RelationshipRepository
            .GetRequestsWithoutAnswer(profile.Id, pagedPaginationParams.PageSize, pagedPaginationParams.After);
        
       
        
        
        return requests;
    }

    public async Task<Result<CursorList<RelationshipProfileResult>>> GetMutualFriends(int requesterProfileId, int otherProfileId, CursorPaginationParams<DateTime> cursorPaginationParams)
    {
        if (await _unitOfWork.ProfileRepository.GetOne(otherProfileId) is null)
            return Result.Fail(ProfileErrors.ProfileNotFound);

        var mutualFriends = await _unitOfWork.ProfileRepository.GetMutualFriends(requesterProfileId,
            otherProfileId, cursorPaginationParams.PageSize, cursorPaginationParams.After);
        
        return  _mapper.Map<CursorList<RelationshipProfileResult>>(mutualFriends);
        
    }
}