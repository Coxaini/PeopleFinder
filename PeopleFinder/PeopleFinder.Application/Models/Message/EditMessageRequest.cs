using PeopleFinder.Application.Models.File;

namespace PeopleFinder.Application.Models.Message;

public record EditMessageRequest(int SenderId,Guid MessageId, string Text, FileDto? Attachment=null, Guid? InReplyToMessageId = null);