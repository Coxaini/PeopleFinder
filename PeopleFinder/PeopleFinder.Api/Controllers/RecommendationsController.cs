using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using PeopleFinder.Api.Common.Extensions;
using PeopleFinder.Api.Controllers.Common;
using PeopleFinder.Application.Models.Rating;
using PeopleFinder.Application.Services.Recommendation;
using PeopleFinder.Contracts.Profile;
using PeopleFinder.Contracts.Recommendation;

namespace PeopleFinder.Api.Controllers
{
    [Route("recs")]
    public class RecommendationsController : ApiController
    {

        private readonly IRecommendationService _recommendationService;
        private readonly IMapper _mapper;

        public RecommendationsController(IRecommendationService recommendationService, IMapper mapper)
        {
            _recommendationService = recommendationService;
            _mapper = mapper;
        }

        [HttpGet("mutual")]
        public async Task<IActionResult> GetMutualFriendsRecommendation()
        {
            var recsResult = await _recommendationService.GetMutualRecommendedProfiles(ProfileIdInClaims);
            return recsResult.Match(
                (recs) => Ok(_mapper.Map<IEnumerable<ProfileWithMutualFriendsResponse>>(recs)),
                Problem);
        }

    }
}
