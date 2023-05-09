using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleFinder.Domain.Entities.MessagingEntities
{
    public enum MessageType
    {
        Text,
        Event
    }
    public class Message
    {
        public Guid Id { get; set; }

        public Guid ChatId { get; set; }
        public int SenderId { get; set; }
        public string Text { get; set; } = null!;
        public DateTime SentAt { get; set; }
        public DateTime? EditedAt { get; set; }
        
        public Guid? AttachmentFileId { get; set; }
        public MediaFile? AttachmentFile { get; set; }

        public Profile Sender { get; set; } = null!;

        public Chat Chat { get; set; } = null!;

    }
}
