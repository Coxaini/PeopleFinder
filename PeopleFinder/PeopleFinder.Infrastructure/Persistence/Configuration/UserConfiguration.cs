using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PeopleFinder.Domain.Entities;

namespace PeopleFinder.Infrastructure.Persistence.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
/*            builder.HasOne(u => u.Profile).
                WithOne(p => p.User).
                HasForeignKey<Profile>(up => up.Id);*/

           
        }
    }
}
