using System.Diagnostics.CodeAnalysis;
using PeopleFinder.Application.Models.Relationship;
using PeopleFinder.Contracts.Profile;
using PeopleFinder.Domain.Common.Models;
using PeopleFinder.Domain.Common.Pagination.Cursor;
using PeopleFinder.Domain.Entities;
// ReSharper disable All

namespace PeopleFinder.Application.Services.ProfileServices;

public record ProfileResult(int Id, string Username, RelationshipResult? Relationship, CursorList<FriendProfile>? MutualFriends
    , string Name, int? Age, string Bio, string City, Gender? Gender, string MainPicture , List<TagResponse>  Tags);