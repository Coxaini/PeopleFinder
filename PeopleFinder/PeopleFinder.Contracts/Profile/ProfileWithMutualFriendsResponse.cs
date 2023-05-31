using PeopleFinder.Domain.Common.Models;
using PeopleFinder.Domain.Entities;

namespace PeopleFinder.Contracts.Profile;

public record ProfileWithMutualFriendsResponse(int Id,string Username,  string Name, int Age,
    string Bio, string City, Gender Gender,string MainPictureUrl , List<UserTag>  Tags, List<string> MutualFriends);
