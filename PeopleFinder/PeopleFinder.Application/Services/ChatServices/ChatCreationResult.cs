using PeopleFinder.Domain.Entities.MessagingEntities;

namespace PeopleFinder.Application.Services.ChatServices;

public record ChatCreationResult(Chat Chat, bool IsNew );