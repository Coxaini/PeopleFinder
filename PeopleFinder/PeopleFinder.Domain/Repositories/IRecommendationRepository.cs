using PeopleFinder.Domain.Entities;
using PeopleFinder.Domain.Repositories.Common;

namespace PeopleFinder.Domain.Repositories;

public interface IRecommendationRepository : IRepo<Recommendation>
{
    Task<Recommendation?> GetRecommendationByRaterAndRatedProfileId(int rateeProfileId, int ratedProfileId);
    
    Task<IEnumerable<Profile>> GetProfileRecommendations(Profile profile);
    
}