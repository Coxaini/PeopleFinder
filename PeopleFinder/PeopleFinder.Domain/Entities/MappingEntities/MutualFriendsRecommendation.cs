using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PeopleFinder.Domain.Entities.MappingEntities;

[Keyless]
public class MutualFriendsRecommendation
{
    public int Id { get; set; }
    public int MutualCount { get; set; }
    public string Usernames { get; set; } = null!;
      
    
}