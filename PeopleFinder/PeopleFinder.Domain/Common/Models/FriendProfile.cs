using PeopleFinder.Domain.Entities;

namespace PeopleFinder.Domain.Common.Models;

public record FriendProfile(Profile Profile, Relationship? Relationship);