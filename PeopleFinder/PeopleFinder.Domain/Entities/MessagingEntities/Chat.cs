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
        Private,
        Group
    }

    public class Chat
    {
        public Guid Id { get; set; }
        
        public List<Message> Messages { get; set; } = null!;
        
        public ChatType ChatType { get; set; }
        
        public List<Profile> Members { get; set; } = null!;
        public  List<ChatMember> ChatMembers { get; set; } = null!;
        
        [Precision(2)]
        public DateTime CreatedAt { get; set; }
    }
}
