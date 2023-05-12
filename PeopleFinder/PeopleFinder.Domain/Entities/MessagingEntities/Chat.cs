using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PeopleFinder.Domain.Entities.MessagingEntities
{
    public enum ChatType
    {
        Direct,
        Group,
        PublicChannel
    }

    public class Chat
    {
        public Guid Id { get; set; }
        public List<Message> Messages { get; set; } = new();
        public string? Title { get; set; } = null!;
        
        public MediaFile? Logo { get; set; }
        public ChatType ChatType { get; set; }
        public int MembersCount { get; set; }
        public List<Profile> Members { get; set; } = new();
        public  List<ChatMember> ChatMembers { get; set; } = new();
        public Guid? LastMessageId { get; private set; }
        public DateTime? LastMessageAt { get; private set; }
        public string? LastMessage { get; private set; }
        public int? LastMessageAuthorProfileId { get; private set; }
        public Profile? LastMessageAuthorProfile { get; private set; }
        
        [Precision(2)]
        public DateTime CreatedAt { get; set; }
        
        public void UpdateLastMessage(Guid id, DateTime sentAt, string text, Profile authorProfile)
        {
            LastMessageId = id;
            LastMessageAt = sentAt;
            LastMessage = text;
            LastMessageAuthorProfile = authorProfile;
        }
        public void DeleteLastMessage()
        {
            LastMessageId = null;
            LastMessageAt = null;
            LastMessage = null;
            LastMessageAuthorProfileId = null;
            LastMessageAuthorProfile = null;
        }
       
        
    }
}
