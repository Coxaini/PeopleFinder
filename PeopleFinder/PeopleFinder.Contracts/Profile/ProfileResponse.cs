using PeopleFinder.Domain.Common.Models;
using PeopleFinder.Domain.Entities;

namespace PeopleFinder.Contracts.Profile;

public record ProfileResponse(int Id, string Username, string Status, List<string> MutualFriends, string Name, int? Age, DateOnly? BirthDate , 
    string Bio, string City, Gender Gender, string MainPictureUrl , List<UserTag>  Tags);