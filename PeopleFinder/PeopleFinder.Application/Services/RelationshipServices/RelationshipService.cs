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
            SentAt = DateTime.Now
        };
            
        await _unitOfWork.RelationshipRepository.AddAsync(friendRequest);
        
        await _unitOfWork.SaveAsync();
        await transaction.CommitAsync();
            
        return friendRequest;
    }
    

    public async Task<Result> ApproveFriendRequest(int profileId, int friendSenderId)
    {
        var relationship = await _unitOfWork.RelationshipRepository.GetRequest(friendSenderId, profileId);

        if (relationship is null)
            return Result.Fail(FriendRequestErrors.FriendRequestNotFound);

        relationship.Status = RelationshipStatus.Approved;
        relationship.AcknowledgeAt = DateTime.Now;

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

    public async Task<Result<PagedList<Profile>>> GetFriends(int profileId, PagedPaginationParams pagedPaginationParams)
    {
        var profile = await _unitOfWork.ProfileRepository.GetOne(profileId);
        if (profile is null)
            return Result.Fail(ProfileErrors.ProfileNotFound);
        
        var friends = await _unitOfWork.ProfileRepository.GetFriends(profile.Id, pagedPaginationParams);
        
        return friends;
    }

    public async Task<Result<CursorList<FriendProfile>>> GetFriends(int profileId, CursorPaginationParams<DateTime> cursorPaginationParams)
    {
        var profile = await _unitOfWork.ProfileRepository.GetOne(profileId);
        if (profile is null)
            return Result.Fail(ProfileErrors.ProfileNotFound);

        var friends = await _unitOfWork.ProfileRepository
            .GetFriends(profile.Id, cursorPaginationParams.PageSize, cursorPaginationParams.After);
        
        
        
        return friends;
    }
    
  

    /*public async Task<Result> RateProfile(int userId, RateProfileRequest rateProfileRequest)
    {
        var raterProfile = await _unitOfWork.ProfileRepository.GetProfileByUserIdAsync(userId);
            
        if (raterProfile is null)
            return Result.Fail(ProfileErrors.ProfileNotFound);
            
        var ratedProfile = await _unitOfWork.ProfileRepository.GetProfileByUserIdAsync(rateProfileRequest.RatedUserId);

        if (ratedProfile is null)
            return Result.Fail(ProfileErrors.RatedProfileNotFound);

        //begin transaction to remove inconsistency between threads
        await using (var transaction = await _unitOfWork.BeginTransactionAsync(IsolationLevel.Serializable))
        {
            var ret = await _unitOfWork.FriendRequestRepository.GetFirstOrDefaultRequestByUserIds(raterProfile.Id, ratedProfile.Id);
            
            if (ret != null)
            {
                //if rating already exists, approve it
                if (ret.ProfileId == raterProfile.Id || ret.Approved != null)
                {
                    return  Result.Fail(ProfileErrors.ProfileAlreadyRated);
                }

                ret.Approved = rateProfileRequest.IsLiked;
                _unitOfWork.FriendRequestRepository.Update(ret);
                
            }
            else
            {
                //if rating doesn't exist, create it and delete recommendation
                
                //check if the user has rights to rate this person
                var rec = await _unitOfWork.RecommendationRepository.
                    GetRecommendationByRaterAndRatedProfileId(raterProfile.Id, ratedProfile.Id);
                if (rec == null)
                    return Result.Fail(ProfileErrors.AccessToRateDenied);
                
                var rating = new FriendRequest
                {
                    Profile = raterProfile,
                    ReceiverProfile = ratedProfile,
                    SentAt = DateTime.Today,
                };
                await _unitOfWork.FriendRequestRepository.AddAsync(rating);
                _unitOfWork.RecommendationRepository.Delete(rec);
                
                raterProfile.LastActivity = DateTime.Now;
                
                Chat chat = new()
                {
                    Members = new List<Profile>() {raterProfile, ratedProfile},
                    CreatedAt = DateTime.Now
                };
                
               await _unitOfWork.ChatRepository.AddAsync(chat);
                
            }
            await _unitOfWork.SaveAsync();
            await transaction.CommitAsync();

        }
        
            
        return Result.Ok();

    }*/

    /*public async Task<Result<PagedList<Relationship>>> GetFriendshipRequestUpdates(int profileId, PagedPaginationParams pagedPaginationParams)
    {
        var profile = await _unitOfWork.ProfileRepository.GetByIdAsync(profileId);
        if (profile is null)
            return Result.Fail(ProfileErrors.ProfileNotFound);

        var friendshipRequests = await _unitOfWork.RelationshipRepository.GetRequestsWithoutAnswer(profile.Id, pagedPaginationParams);

        return friendshipRequests.ToResult();
    }*/

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

    public async Task<Result<CursorList<FriendProfileResult>>> GetMutualFriends(int requesterProfileId, int otherProfileId, CursorPaginationParams<DateTime> cursorPaginationParams)
    {
        if (await _unitOfWork.ProfileRepository.GetOne(otherProfileId) is null)
            return Result.Fail(ProfileErrors.ProfileNotFound);

        var mutualFriends = await _unitOfWork.ProfileRepository.GetMutualFriends(requesterProfileId,
            otherProfileId, cursorPaginationParams.PageSize, cursorPaginationParams.After);
        
        return  _mapper.Map<CursorList<FriendProfileResult>>(mutualFriends);
        
    }
}