using FluentResults;
using PeopleFinder.Application.Common.Errors;
using PeopleFinder.Application.Common.Interfaces.FileStorage;
using PeopleFinder.Application.Models.Chat;
using PeopleFinder.Application.Models.File;
using PeopleFinder.Domain.Common.Models;
using PeopleFinder.Domain.Common.Pagination.Cursor;
using PeopleFinder.Domain.Entities.MessagingEntities;
using PeopleFinder.Domain.Repositories.Common;


namespace PeopleFinder.Application.Services.ChatServices;

public class ChatService : IChatService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileStorageManager _fileStorageManager;

    public ChatService(IUnitOfWork unitOfWork, IFileStorageManager fileStorageManager)
    {
        _unitOfWork = unitOfWork;
        _fileStorageManager = fileStorageManager;
    }
    
    public async Task<Result<CursorList<UserChat>>> GetChats(int profileId, CursorPaginationParams<DateTime> paginationParams)
    {
        var profile = await _unitOfWork.ProfileRepository.GetOne(profileId);
        if (profile is null)
            return Result.Fail(ProfileErrors.ProfileNotFound);

        var chats = await _unitOfWork.ChatRepository.GetChatsAsync(profileId, paginationParams.PageSize, paginationParams.After);

        return chats;

    }

    
    public async Task<Result<Chat>> CreateDirectChat(CreateDirectChatRequest request)
    {
        
        if (request.CreatorId == request.FriendId)
        {
            return Result.Fail(ChatErrors.CannotCreateChatWithSelf);
        }
        if (!await _unitOfWork.RelationshipRepository.IsProfilesFriends(request.CreatorId, request.FriendId))
        {
            return Result.Fail(ChatErrors.NotFriends);
        }
        
        var now = DateTime.Now;
        
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
                    JoinedAt = DateTime.Now
                },
                new ChatMember()
                {
                    ProfileId =  request.FriendId,
                    Role = MemberRole.Admin,
                    JoinedAt = DateTime.Now
                }
            },
            LastMessage = request.Text,
            LastMessageAt = now,
        };
        
        MediaFile? mediaFile = await UploadFileAsync(request.Attachment, DateTime.Now);

        var firstMessage = new Message(){
            SenderId = request.CreatorId,
            Text = request.Text,
            SentAt = now,
        };
            
        chat.Messages.Add(firstMessage);

        await _unitOfWork.ChatRepository.AddAsync(chat);
        await _unitOfWork.SaveAsync();

        return chat;
            
    }
    public async Task<Result<Message>> SendMessage(SendMessageRequest request)
    {
        var chat = await _unitOfWork.ChatRepository.GetOne(request.ChatId);
        if (chat is null)
            return Result.Fail(ChatErrors.ChatNotFound);
        
        var profile = await _unitOfWork.ProfileRepository.GetOne(request.SenderId);
        if (profile is null)
            return Result.Fail(ProfileErrors.ProfileNotFound);

        if (!await _unitOfWork.ChatRepository.IsProfileInChatAsync(request.SenderId, request.ChatId))
        {
            return Result.Fail(ChatErrors.ProfileNotInChat);
        }

        var timeNow = DateTime.Now;
        
        MediaFile? mediaFile = await UploadFileAsync(request.Attachment, timeNow);

        var message = new Message()
        {
            SenderId = request.SenderId,
            Text = request.Text,
            SentAt = timeNow,
            AttachmentFile = mediaFile,
        };
        
        await _unitOfWork.MessageRepository.AddAsync(message);
        await _unitOfWork.SaveAsync();
        return message;
    }
    
    private async Task<MediaFile?> UploadFileAsync(FileDto? fileDto, DateTime now)
    {
        if (fileDto is null)
            return null;
        
        var file =await _fileStorageManager.UploadFileAsync(fileDto, now);
        var mediaFile = new MediaFile()
        {
            Id = file.Token,
            OriginalName = fileDto.FileName,
            Type = fileDto.Type,
            Extension = file.Extension,
            UploadTime = now,
            IsInPrivateChat = true
        };
        await _unitOfWork.MediaFileRepository.AddAsync(mediaFile);
        return mediaFile;
        
    }
    
    
}