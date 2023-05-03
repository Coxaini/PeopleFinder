using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleFinder.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Password { get; set; } = null!;
        
        public string Email { get; set; } = null!;

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public Profile? Profile { get; set; }

    }
}
