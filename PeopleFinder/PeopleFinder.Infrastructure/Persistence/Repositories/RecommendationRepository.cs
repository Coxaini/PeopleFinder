using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PeopleFinder.Application.Common.Constants;
using PeopleFinder.Domain.Entities;
using PeopleFinder.Domain.Repositories;
using PeopleFinder.Infrastructure.Persistence.Common;

namespace PeopleFinder.Infrastructure.Persistence.Repositories;

public class RecommendationRepository : BaseRepo<Recommendation>, IRecommendationRepository
{
    public RecommendationRepository(PeopleFinderDbContext context) : base(context)
    {
    }

    public async Task<Recommendation?> GetRecommendationByRaterAndRatedProfileId(int raterProfileId, int ratedProfileId)
    {
        var rec = await Context.Recommendations
            .FirstOrDefaultAsync(r => r.ProfileId == raterProfileId &&
                                      r.RecommendedProfileId == ratedProfileId);

        return rec;
    }

    public async Task<IEnumerable<Profile>> GetProfileRecommendations(Profile profile)
    {
        var cachedRecs = await Context.Recommendations
            .Where(r => r.ProfileId == profile.Id)
            .OrderBy(c=>c.Id)
            .ToListAsync();

        var recs = await GetNewRecommendations(profile, cachedRecs.Select(c=>c.RecommendedProfileId));
                
        await Context.Recommendations.AddRangeAsync(recs.Select(r => new Recommendation()
        {
                ProfileId = profile.Id,
                RecommendedProfileId = r.Id
        }));
        
        //Delete old recs if there are too many
        if (cachedRecs.Count >= RecsConstants.RecsCountInOneRequest)
        {
            Context.Recommendations.RemoveRange(cachedRecs.Take(RecsConstants.RecsCountInOneRequest));
        }
        
        await Context.SaveChangesAsync();

        return recs;

    }
    
    private async Task<List<Profile>> GetNewRecommendations(Profile profile, IEnumerable<int> cachedRecs)
    {
        /*var recs = await Context.Profiles
                .Where(x => x.City == profile.City)
                .Where(IsInterestedFilter(profile))
                .Where(x => x.Id != profile.Id)
                .Where(x=>!Context.FriendRequests.Any(r=>(r.ProfileId== profile.Id && x.Id == r.ReceiverProfileId) 
                                                  || (r.ProfileId == x.Id && r.ReceiverProfileId == profile.Id)))
                .Where(x => !cachedRecs.Contains(x.Id))
                .OrderBy(x=>x.LastActivity.Date)
                .Take(50)
                .OrderByDescending(TagsIntersectionCount(profile))//To: write instead of this IQueryable extension methods
                .Take(RecsConstants.RecsCountInOneRequest)
                .Include(x=>x.Tags)
                .ToListAsync();
        return recs;*/
        throw new NotImplementedException();
    }
    
    private Expression<Func<Profile, int>> TagsIntersectionCount(Profile profile)
    {
        
        return (x) => Context.Tags
            .Count(t => profile.Tags.Contains(t) && x.Tags.Contains(t));

    }

    private Expression<Func<Profile, bool>> IsInterestedFilter(Profile profile)
    {
        return(x) =>((x.Gender & profile.GenderInterest) == x.Gender) && ((profile.Gender & x.GenderInterest) == profile.Gender);
    }
}