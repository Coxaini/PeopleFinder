using PeopleFinder.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PeopleFinder.Contracts.Profile
{
    public record ShortProfileResponse(
        int Id,
        string Username,
        string Name,
        int? Age,
        string Bio,
        string City,
        Gender Gender,
        string? MainPictureUrl, 
        string? RelationshipStatus,
        List<TagResponse> Tags);
    
    

    public record TagResponse(int Id, string Title);
}
