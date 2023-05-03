using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PeopleFinder.Domain.Entities;

namespace PeopleFinder.Infrastructure.Persistence.Configuration;

public class RelationshipConfiguration: IEntityTypeConfiguration<Relationship>
{
    public void Configure(EntityTypeBuilder<Relationship> builder)
    {

        builder.HasIndex(f => f.Status);
        
        builder
            .HasOne(c => c.InitiatorProfile)
            .WithMany(p => p.InitiatedRelationships)
            .HasForeignKey(c => c.InitiatorProfileId)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder
            .HasOne(c => c.ReceiverProfile)
            .WithMany(p => p.ReceivedRelationships)
            .HasForeignKey(c => c.ReceiverProfileId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}