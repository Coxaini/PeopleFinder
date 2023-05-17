using PeopleFinder.Application.Models.Relationship;
using PeopleFinder.Contracts.Profile;
using PeopleFinder.Domain.Common.Models;
using PeopleFinder.Domain.Common.Pagination.Cursor;
using PeopleFinder.Domain.Entities;

namespace PeopleFinder.Application.Services.ProfileServices;

public record FriendProfileResult(
    int Id,
    string Username,
    RelationshipResult? Relationship,
    string Name,
    int? Age,
    string Bio,
    string City,
    Gender? Gender,
    Guid? MainPictureId,
    List<TagResponse> Tags
);