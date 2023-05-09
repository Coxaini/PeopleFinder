using Microsoft.AspNetCore.Http;

namespace PeopleFinder.Contracts.Chats;

public record SendMessage(Guid ChatId, string Text, Guid? InReplyToMessageId, IFormFile? Attachment);