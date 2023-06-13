using PeopleFinder.Domain.Entities;

namespace PeopleFinder.Infrastructure.Persistence.Common.Extensions;

public static class ProfileExtensions
{
    public static IOrderedQueryable<Profile> OrderByTagsIntersection(this IQueryable<Profile> profiles, IEnumerable<Tag> tags)
    {
        return profiles.OrderByDescending(p => p.Tags.Count(t => tags.Contains(t)));
    }

}