using FluentResults;
using PeopleFinder.Application.Common.Errors;
using PeopleFinder.Application.Models.Rating;
using PeopleFinder.Domain.Common.Pagination.Page;
using PeopleFinder.Domain.Common.Recommendation;
using PeopleFinder.Domain.Entities;
using PeopleFinder.Domain.Repositories.Common;


namespace PeopleFinder.Application.Services.Recommendation
{
    public class RecommendationService : IRecommendationService
    {

        private readonly IUnitOfWork _unitOfWork;
        

        public RecommendationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<IList<Profile>>> GetNewRecommendedProfiles(int profileId)
        {
            var profile = await _unitOfWork.ProfileRepository.GetWithTagsByIdAsync(profileId);

            if (profile is null)
                return Result.Fail(ProfileErrors.ProfileNotFound);

            var recs = await _unitOfWork.ProfileRepository.GetRecommendedByTags(profile, 15);

            return recs.ToResult();

        }

        public async Task<Result<PagedList<ProfileWithMutualFriends>>> GetMutualRecommendedProfiles(int profileId, PagedPaginationParams pag)
        {
            var profile = await _unitOfWork.ProfileRepository.GetOne(profileId);

            if (profile is null)
                return Result.Fail(ProfileErrors.ProfileNotFound);
            
            var recs = 
                await _unitOfWork.ProfileRepository.GetRecommendedByMutualFriends(profile.Id, pag.PageNumber, pag.PageSize);

            return recs.ToResult();

        }
    }
}
