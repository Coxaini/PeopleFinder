using PeopleFinder.Domain.Entities;

namespace PeopleFinder.Contracts.Profile;

public record ProfileWithMutualFriendsResponse(int UserId,string Username,  string Name, int Age,
    string Bio, string City, Gender Gender,List<string> PictureUrls , List<TagResponse>  Tags, List<string> MutualFriends);
