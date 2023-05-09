using PeopleFinder.Application.Models.File;

namespace PeopleFinder.Application.Models.Chat;

public record SendMessageRequest(int SenderId,Guid ChatId, string Text, FileDto? Attachment=null, Guid? InReplyToMessageId = null);