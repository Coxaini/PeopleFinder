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
        Attachment,
        Event
    }
    public class Message
    {
        public long Id { get; set; }

        public long ChatId { get; set; }
        public int SenderId { get; set; }
        public string Text { get; set; } = null!;
        public DateTime SentAt { get; set; }
        public DateTime? EditedAt { get; set; }

        public Profile Sender { get; set; } = null!;

        public Chat Chat { get; set; } = null!;

    }
}
