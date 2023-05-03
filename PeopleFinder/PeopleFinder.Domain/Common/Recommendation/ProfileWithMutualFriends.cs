using PeopleFinder.Domain.Entities;

namespace PeopleFinder.Domain.Common.Recommendation;

public record ProfileWithMutualFriends(Profile Profile, IEnumerable<string> MutualFriends);