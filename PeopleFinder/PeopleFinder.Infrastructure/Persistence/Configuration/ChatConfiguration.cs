using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PeopleFinder.Domain.Entities.MessagingEntities;

namespace PeopleFinder.Infrastructure.Persistence.Configuration
{
    public class ChatConfiguration : IEntityTypeConfiguration<Chat>
    {
        public void Configure(EntityTypeBuilder<Chat> builder)
        {
            /*builder.
            HasOne(u => u.FirstMember).
            WithMany(c => c.HostChats).
            HasForeignKey(u => u.FirstMemberId).OnDelete(DeleteBehavior.NoAction);


            builder.
                HasOne(u => u.SecondMember).
                WithMany(c => c.GuestChats).
                HasForeignKey(u => u.SecondMemberId).OnDelete(DeleteBehavior.NoAction);*/
            builder
                .HasOne(c => c.LastMessageAuthorProfile)
                .WithMany(p => p.LastAuthorSenderChats)
                .HasForeignKey(c => c.LastMessageAuthorProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(c => c.Messages)
                .WithOne(m => m.Chat)
                .HasForeignKey(m => m.ChatId);

            builder
                .HasMany(c=>c.Members)
                .WithMany(p=>p.Chats)
                .UsingEntity<ChatMember>(
                    cm=>cm
                        .HasOne(p=>p.Profile)
                        .WithMany(p=>p.ChatMembers)
                        .HasForeignKey(p=>p.ProfileId),
                    cm=>cm
                        .HasOne(c=>c.Chat)
                        .WithMany(c=>c.ChatMembers)
                        .HasForeignKey(c=>c.ChatId),
                    cm =>
                    {
                        cm.HasKey(c => new { c.ChatId, c.ProfileId });
                        
                    });
        }
    }
    
}
