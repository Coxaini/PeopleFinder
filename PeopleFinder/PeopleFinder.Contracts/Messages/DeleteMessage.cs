namespace PeopleFinder.Contracts.Messages;

public record DeleteMessage(Guid ChatId, Guid MessageId);