using FluentResults;
using PeopleFinder.Application.Common.Errors;
using PeopleFinder.Application.Models.Rating;
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

        public async Task<Result<IEnumerable<Profile>>> GetNewRecommendedProfiles(int userId)
        {
            var profile = await _unitOfWork.ProfileRepository.GetOne(userId);

            if (profile is null)
                return Result.Fail(ProfileErrors.ProfileNotFound);

            var recs = await _unitOfWork.RecommendationRepository.GetProfileRecommendations(profile);

            return recs.ToResult();

        }

        public async Task<Result<IEnumerable<ProfileWithMutualFriends>>> GetMutualRecommendedProfiles(int profileId)
        {
            var profile = await _unitOfWork.ProfileRepository.GetOne(profileId);

            if (profile is null)
                return Result.Fail(ProfileErrors.ProfileNotFound);
            
            var recs = await _unitOfWork.ProfileRepository.GetRecommendedByMutualFriends(profile.Id);

            return recs.ToResult();

        }
    }
}
