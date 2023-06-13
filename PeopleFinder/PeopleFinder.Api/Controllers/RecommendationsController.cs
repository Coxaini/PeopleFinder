using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PeopleFinder.Api.Common.Extensions;
using PeopleFinder.Api.Controllers.Common;
using PeopleFinder.Application.Models.Rating;
using PeopleFinder.Application.Services.Recommendation;
using PeopleFinder.Contracts.Pagination;
using PeopleFinder.Contracts.Profile;
using PeopleFinder.Contracts.Recommendation;
using PeopleFinder.Domain.Common.Pagination.Page;

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
        public async Task<IActionResult> GetMutualFriendsRecommendation([FromQuery]PagedPagination pag)
        {
            PagedPaginationParams pagParams = new()
            {
                PageNumber = pag.PageNumber,
                PageSize = pag.PageSize
            };
            var recsResult = await _recommendationService.GetMutualRecommendedProfiles(ProfileIdInClaims, pagParams);
            return recsResult.Match(
                (recs) =>
                {
                    var metadata = new
                    {
                        recs.TotalPages,
                        recs.TotalCount,
                        recs.HasNext,
                        recs.HasPrevious,
                    };
                    Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                    
                    return Ok(_mapper.Map<IList<ProfileWithMutualFriendsResponse>>(recs));
                },
                Problem);
        }
        
        [HttpGet("new")]
        public async Task<IActionResult> GetNewRecommendation()
        {
            var recsResult = await _recommendationService.GetNewRecommendedProfiles(ProfileIdInClaims);
            return recsResult.Match(
                (recs) => Ok(_mapper.Map<IList<ShortProfileResponse>>(recs)),
                Problem);
        }

    }
}
