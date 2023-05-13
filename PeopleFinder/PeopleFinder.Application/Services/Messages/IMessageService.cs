using FluentResults;
using PeopleFinder.Application.Models.Chat;
using PeopleFinder.Application.Models.Message;
using PeopleFinder.Domain.Common.Models;
using PeopleFinder.Domain.Common.Pagination.Cursor;
using PeopleFinder.Domain.Entities.MessagingEntities;

namespace PeopleFinder.Application.Services.Messages;

public interface IMessageService
{
    Task<Result<CursorList<UserMessage>>> GetMessages(int profileId, Guid chatId, CursorPaginationParams<DateTime> paginationParams);
    Task<Result<UserMessage>> SendMessage(SendMessageRequest request); 
    Task<Result<Message>> EditMessage(EditMessageRequest request);
    Task<Result<Message>> DeleteMessage(int profileId, Guid messageId);
}