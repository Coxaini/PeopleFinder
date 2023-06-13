namespace PeopleFinder.Contracts.Chats;

public record ChatResponse(
    Guid Id,
    bool IsNew
);