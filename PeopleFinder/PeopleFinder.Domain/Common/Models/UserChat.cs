using PeopleFinder.Domain.Entities.MessagingEntities;

namespace PeopleFinder.Domain.Common.Models;

public class UserChat
{
    public Guid Id { get; init; }
    public string DisplayTitle { get; set; } = null!;
    public string? UniqueTitle { get; set; }
    public Guid? DisplayLogoId { get; set; }
    public ChatType ChatType { get; init; }
    public DateTime? LastMessageAt { get; init; }
   
    public string? LastMessageAuthorName { get; init; }
    public string? LastMessage { get; init; }
    public int MembersCount { get; init; }
    public DateTime CreatedAt { get; init; }


}