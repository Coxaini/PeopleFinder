using Microsoft.AspNetCore.Http;

namespace PeopleFinder.Contracts.Messages;

public record EditMessage(Guid MessageId, string Text, IFormFile? Attachment);