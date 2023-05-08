using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PeopleFinder.Domain.Entities.MessagingEntities;

namespace PeopleFinder.Infrastructure.Persistence.Configuration;

public class MediaFileConfiguration: IEntityTypeConfiguration<MediaFile>
{
    public void Configure(EntityTypeBuilder<MediaFile> builder)
    {
        builder.HasIndex(x => x.Id).IsUnique();
        
        
    }
}