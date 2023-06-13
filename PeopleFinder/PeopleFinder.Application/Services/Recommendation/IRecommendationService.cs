using FluentResults;
using PeopleFinder.Application.Models.Rating;
using PeopleFinder.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PeopleFinder.Domain.Common.Pagination.Page;
using PeopleFinder.Domain.Common.Recommendation;

namespace PeopleFinder.Application.Services.Recommendation
{
    public interface IRecommendationService
    {
        Task<Result<IList<Profile>>> GetNewRecommendedProfiles(int profileId);
        Task<Result<PagedList<ProfileWithMutualFriends>>> GetMutualRecommendedProfiles(int profileId, PagedPaginationParams pag);

        

    }
}
