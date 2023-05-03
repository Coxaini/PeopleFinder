using FluentResults;
using PeopleFinder.Application.Models.Rating;
using PeopleFinder.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PeopleFinder.Domain.Common.Recommendation;

namespace PeopleFinder.Application.Services.Recommendation
{
    public interface IRecommendationService
    {
        Task<Result<IEnumerable<Profile>>> GetNewRecommendedProfiles(int userId);
        Task<Result<IEnumerable<ProfileWithMutualFriends>>> GetMutualRecommendedProfiles(int profileId);

        

    }
}
