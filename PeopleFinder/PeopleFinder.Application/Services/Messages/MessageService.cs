using FluentResults;
using FluentValidation;
using PeopleFinder.Application.Common.Errors;
using PeopleFinder.Application.Common.Extensions;
using PeopleFinder.Application.Common.Interfaces.FileStorage;
using PeopleFinder.Application.Models.File;
using PeopleFinder.Application.Models.Message;
using PeopleFinder.Application.Services.FileStorage;
using PeopleFinder.Domain.Common.Models;
using PeopleFinder.Domain.Common.Pagination.Cursor;
using PeopleFinder.Domain.Entities.MessagingEntities;
using PeopleFinder.Domain.Repositories.Common;

namespace PeopleFinder.Application.Services.Messages;

public class MessageService : IMessageService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileStorageManager _fileStorageManager;
    private readonly IValidator<FileDto> _fileValidator;


    public MessageService(IUnitOfWork unitOfWork, IFileStorageManager fileStorageManager, IValidator<FileDto> fileValidator)
    {
        _unitOfWork = unitOfWork;
        _fileStorageManager = fileStorageManager;
        _fileValidator = fileValidator;
    }

    public async Task<Result<CursorList<UserMessage>>> GetMessages(int profileId, Guid chatId,
        CursorPaginationParams<DateTime> paginationParams)
    {
        if (!await _unitOfWork.ChatRepository.IsProfileInChatAsync(profileId, chatId))
            return Result.Fail(ChatErrors.ProfileNotInChat);

        var messages = 
            await _unitOfWork.MessageRepository.GetUserMessages(chatId, paginationParams.PageSize, paginationParams.After, paginationParams.Before);

        foreach (var message in messages.Items)
        {
            message.IsMine = message.SenderId == profileId;
        }
        
        return messages;

    }



    public async Task<Result<UserMessage>> SendMessage(SendMessageRequest request)
    {
       
        if (request.Attachment is not null)
        {
            var valResult = await  _fileValidator.ValidateAsync(request.Attachment);
            if (!valResult.IsValid)
            {
                return Result.Fail(valResult.ToErrorList());
            }
        }
        
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
        
        var mediaFile = await UploadFileAsync(request.Attachment, chat , timeNow);
        
        var message = new Message()
        {
            Id = Guid.NewGuid(),
            Chat = chat,
            SenderId = request.SenderId,
            Text = request.Text,
            SentAt = timeNow,
            AttachmentFile = mediaFile,
        };
        
        await _unitOfWork.MessageRepository.AddAsync(message);


        chat.UpdateLastMessage(message.Id,message.SentAt, message.Text, profile);
        
        await _unitOfWork.SaveAsync();
        
        var userMessage = new UserMessage()
        {
            Id = message.Id,
            ChatId = message.ChatId,
            ChatType = chat.ChatType,
            SenderId = message.SenderId,
            Text = message.Text,
            SentAt = message.SentAt,
            EditedAt = message.EditedAt,
            DisplayName = profile.Name,
            AuthorAvatarId = profile.MainPictureId,
            AttachmentFileId = message.AttachmentFileId,
            IsMine = true
        };
        
        return userMessage;
    }

    public async Task<Result<Message>> EditMessage(EditMessageRequest request)
    {
        if (request.Attachment is not null)
        {
            var valResult = await  _fileValidator.ValidateAsync(request.Attachment);
            if (!valResult.IsValid)
            {
                return Result.Fail(valResult.ToErrorList());
            }
        }
        
        var message = await _unitOfWork.MessageRepository.GetWithDetailsById(request.MessageId);
        if (message is null)
            return Result.Fail(MessageErrors.MessageNotFound);
        
        if (message.SenderId != request.SenderId)
            return Result.Fail(MessageErrors.MessageNotBelongToProfile);
        
        message.EditedAt = DateTime.Now;
        message.Text = request.Text;
        
        if (message.AttachmentFile is not null)
            _fileStorageManager.DeleteFileAsync(message.AttachmentFile);
        
        message.AttachmentFile = await UploadFileAsync(request.Attachment, message.Chat, DateTime.Now);
        
        await _unitOfWork.SaveAsync();
        return message;
        
        
    }

    public async Task<Result<Message>> DeleteMessage(int profileId, Guid messageId)
    {
        var message = await _unitOfWork.MessageRepository.GetWithDetailsById(messageId);
        if (message is null)
            return Result.Fail(MessageErrors.MessageNotFound);
        
        if (message.SenderId != profileId)
            return Result.Fail(MessageErrors.MessageNotBelongToProfile);


        if (message.AttachmentFile is not null)
            _fileStorageManager.DeleteFileAsync(message.AttachmentFile);

        _unitOfWork.MessageRepository.Delete(message);

        if (message.Id == message.Chat.LastMessageId)
        {
            var newLastMessage = await  _unitOfWork.MessageRepository.GetLastMessage(message.ChatId, message.Id);
            if(newLastMessage is not null)
                message.Chat.UpdateLastMessage(newLastMessage.Id,newLastMessage.SentAt, newLastMessage.Text, newLastMessage.Sender);
            else
            {
                message.Chat.DeleteLastMessage();
            }
        }
        
        
        await _unitOfWork.SaveAsync();
        return message;

    }
    
    private async Task<MediaFile?> UploadFileAsync(FileDto? fileDto, Chat chat, DateTime now)
    {
        if (fileDto is null)
            return null;
        
        var mediaFile = await _fileStorageManager.UploadFileAsync(fileDto);
        
        if(chat.ChatType != ChatType.PublicChannel)
            mediaFile.IsInPrivateChat = true;
        
        await _unitOfWork.MediaFileRepository.AddAsync(mediaFile);
        return mediaFile;
        
    }
}