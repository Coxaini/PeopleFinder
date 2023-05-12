using Microsoft.AspNetCore.Http;

namespace PeopleFinder.Contracts.Messages;

public record SendMessage(Guid ChatId, string Text, Guid? InReplyToMessageId, IFormFile? Attachment);