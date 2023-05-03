namespace PeopleFinder.Domain.Entities;

public class Recommendation
{
    public long Id { get; set; }
    public int ProfileId { get; set; }
    public int RecommendedProfileId { get; set; }
    
    public Profile? Profile { get; set; }
    public  Profile? RecommendedProfile { get; set; }

}