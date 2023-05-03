using PeopleFinder.Domain.Entities;

namespace PeopleFinder.Contracts.Profile;

public record ProfileResponse(int Id, string Username, string Status, List<string> MutualFriends, string Name, int? Age,
    string Bio, string City, Gender Gender, string MainPicture , List<TagResponse>  Tags);