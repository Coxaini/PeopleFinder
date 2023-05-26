using PeopleFinder.Domain.Entities.MessagingEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleFinder.Domain.Entities
{

 
    public enum Gender
    {
        None = 0,
        Male = 1,
        Female = 2
    }
    public class Profile
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        
        public string Username { get; set; }
        public string Name { get; set; }
        
        public DateTime? BirthDate { get; set; }
        public string Bio { get; set; } = null!;

        public string City { get; set; } = null!;
        public Gender Gender { get; set; } 
        [NotMapped]
        public Gender? GenderInterest { get; set;} //delete
        
        public DateTime LastActivity { get; set; }
        public DateTime CreatedAt { get; set; }

        public List<Tag> Tags { get; set; } = new();


        public Guid? MainPictureId { get; set; }
        public MediaFile? MainPicture { get; set; } = null!;

        public List<Recommendation> ReceivedRecommendations = new();
        public List<Recommendation> PromotedRecommendations = new();
        
        public List<Relationship> InitiatedRelationships { get; set; } = new();
        public List<Relationship> ReceivedRelationships { get; set; } = new();
        
        public List<Chat> LastAuthorSenderChats { get; set; } = new();
        public List<Chat> Chats { get; set; } = null!;
        public List<ChatMember> ChatMembers { get; set; } = new();
        
        public User? User { get; set; }


        [NotMapped]
        public int? Age  => BirthDate != null ? (int)((DateTime.UtcNow.Date - BirthDate).Value.TotalDays / 365.25) : null;
       
        public static int? GetAge(DateTime? birthDate)
        {
            if (birthDate == null) return null;
            return (int)((DateTime.UtcNow.Date - birthDate).Value.TotalDays / 365.25);
        }




    }
}
