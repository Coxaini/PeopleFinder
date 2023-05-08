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
        Male = 1,
        Female = 2
    }
    public class Profile
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        
        public string Username { get; set; } = null!;
        public string Name { get; set; } = null!;
        
        public DateTime? BirthDate { get; set; }
        public string Bio { get; set; } = null!;

        public string City { get; set; } = null!;
        public Gender? Gender { get; set; }
        [NotMapped]
        public Gender? GenderInterest { get; set;} //delete
        
        public DateTime LastActivity { get; set; }
        public DateTime CreatedAt { get; set; }

        public List<Tag> Tags { get; set; } = null!;


        public Guid? MainPictureId { get; set; }
        public MediaFile? MainPicture { get; set; } = null!;

        public List<Recommendation> ReceivedRecommendations = null!;
        public List<Recommendation> PromotedRecommendations = null!;
        
        public List<Relationship> InitiatedRelationships { get; set; } = null!;
        public List<Relationship> ReceivedRelationships { get; set; } = null!;
        
        public List<Chat> Chats { get; set; } = null!;
        public List<ChatMember> ChatMembers { get; set; } = null!;
        
        public User? User { get; set; }


        [NotMapped]
        public int? Age  => BirthDate != null ? (int)((DateTime.Today - BirthDate).Value.TotalDays / 365.25) : null;
       




    }
}
