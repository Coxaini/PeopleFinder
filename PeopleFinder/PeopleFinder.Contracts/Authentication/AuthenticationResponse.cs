using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleFinder.Contracts.Authentication
{
    public record AuthenticationResponse(
    int Id,
    string Username,
    string Email);
    
}
