using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PeopleFinder.Domain.Entities;

namespace PeopleFinder.Infrastructure.Persistence.Configuration;

public class RecommendationConfiguration : IEntityTypeConfiguration<Recommendation>
{
    public void Configure(EntityTypeBuilder<Recommendation> builder)
    {
        
        builder.HasOne(r => r.RecommendedProfile)
            .WithMany(p => p.PromotedRecommendations)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(r => r.Profile)
            .WithMany(p => p.ReceivedRecommendations)
            .OnDelete(DeleteBehavior.Cascade);

    }
}