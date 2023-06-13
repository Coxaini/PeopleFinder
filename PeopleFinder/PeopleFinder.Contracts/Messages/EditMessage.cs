using Microsoft.AspNetCore.Http;

namespace PeopleFinder.Contracts.Messages;

public record EditMessage(Guid MessageId, Guid ChatId , string Text, IFormFile? Attachment);