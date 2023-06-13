using FluentResults;
using FluentValidation;
using PeopleFinder.Application.Common.Errors;
using PeopleFinder.Application.Common.Extensions;
using PeopleFinder.Application.Common.Interfaces.FileStorage;
using PeopleFinder.Application.Models.Chat;
using PeopleFinder.Application.Models.File;
using PeopleFinder.Application.Models.Message;
using PeopleFinder.Application.Services.FileStorage;
using PeopleFinder.Domain.Common.Models;
using PeopleFinder.Domain.Common.Pagination.Cursor;
using PeopleFinder.Domain.Entities.MessagingEntities;
using PeopleFinder.Domain.Repositories.Common;


namespace PeopleFinder.Application.Services.ChatServices;

public class ChatService : IChatService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileStorageManager _fileStorageManager;
    private readonly IValidator<FileDto> _fileValidator;


    public ChatService(IUnitOfWork unitOfWork, IFileStorageManager fileStorageManager,
        IValidator<FileDto> fileValidator)
    {
        _unitOfWork = unitOfWork;
        _fileStorageManager = fileStorageManager;
        _fileValidator = fileValidator;
    }
    
    public async Task<Result<CursorList<UserChat>>> GetChats(int profileId, CursorPaginationParams<DateTime> paginationParams)
    {
        var profile = await _unitOfWork.ProfileRepository.GetOne(profileId);
        if (profile is null)
            return Result.Fail(ProfileErrors.ProfileNotFound);

        var chats = await _unitOfWork.ChatRepository.GetChatsAsync(profileId, paginationParams.PageSize, paginationParams.After);

        return chats;

    }
    
    public async Task<Result<IList<Chat>>> GetAllChats(int profileId)
    {

        var chats = await _unitOfWork.ChatRepository.GetAllChatsAsync(profileId);

        return chats.ToResult();

    }
    
    

    public async Task<Result> DeleteChat(int profileId, Guid chatId)
    {
        var chat = await _unitOfWork.ChatRepository.GetOne(chatId);
        
        if (chat is null)
        {
            return Result.Fail(ChatErrors.ChatNotFound);
        }
        
        if (!await _unitOfWork.ChatRepository.IsProfileInChatAsync(profileId, chatId))
        {
            return Result.Fail(ChatErrors.ProfileNotInChat);
        }
        
        _unitOfWork.ChatRepository.Delete(chat);
        await _unitOfWork.SaveAsync();
        
        return Result.Ok();
    }


    public async Task<Result<ChatCreationResult>> CreateDirectChat(CreateDirectChatRequest request)
    {

        if (request.CreatorId == request.FriendId)
        {
            return Result.Fail(ChatErrors.CannotCreateChatWithSelf);
        }
        
        var creator = await _unitOfWork.ProfileRepository.GetOne(request.CreatorId);
        if (creator is null)
        {
            return Result.Fail(ProfileErrors.ProfileNotFound);
        }
        
        var friend = await _unitOfWork.ProfileRepository.GetOne(request.FriendId);
        if (friend is null)
        {
            return Result.Fail(ChatErrors.NotFriends);
        }
        
        if (!await _unitOfWork.RelationshipRepository.IsProfilesFriends(request.CreatorId, request.FriendId))
        {
            return Result.Fail(ChatErrors.NotFriends);
        }

        var existingChat = await _unitOfWork.ChatRepository.GetDirectChatAsync(request.CreatorId, friend.Id);
        if (existingChat is not null)
        {
            return new ChatCreationResult(existingChat, false);
        }
        
        
        var now = DateTime.UtcNow;
        
        var chat = new Chat()
        {
            ChatType = ChatType.Direct,
            CreatedAt = now,
            MembersCount = 2,
            ChatMembers = new List<ChatMember>()
            {
                new ChatMember()
                {
                    ProfileId = request.CreatorId,
                    Role = MemberRole.Admin,
                    JoinedAt = DateTime.UtcNow
                },
                new ChatMember()
                {
                    ProfileId =  request.FriendId,
                    Role = MemberRole.Admin,
                    JoinedAt = DateTime.UtcNow
                }
            },
        };

        await _unitOfWork.ChatRepository.AddAsync(chat);
        await _unitOfWork.SaveAsync();
        

        return new ChatCreationResult(chat, true);

    }


    public async Task<Result<UserChat>> GetDirectChat(int profileId, int friendId)
    {
        if (profileId == friendId)
            return Result.Fail(ChatErrors.CannotGetChatWithSelf);
        
        var friend = await _unitOfWork.ProfileRepository.GetOne(friendId);
        if(friend is null)
            return Result.Fail(RelationshipErrors.FriendNotFound);
        
        var chat = await _unitOfWork.ChatRepository.GetDirectChatAsync(profileId, friend);
        
        if(chat is null)
            return Result.Fail(ChatErrors.ChatNotFound);

        return chat;

    }

    public async Task<Result<UserChat>> GetChat(int profileId,Guid chatId)
    {
        var chat = await _unitOfWork.ChatRepository.GetChatAsync(profileId, chatId);
        if (chat is null)
            return Result.Fail(ChatErrors.ChatNotFound);
        
        if(!await _unitOfWork.ChatRepository.IsProfileInChatAsync(profileId, chatId))
            return Result.Fail(ChatErrors.ProfileNotInChat);

        return chat;
    }


    
    
}