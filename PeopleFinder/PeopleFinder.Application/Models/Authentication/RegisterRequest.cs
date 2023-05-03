using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PeopleFinder.Domain.Entities;

namespace PeopleFinder.Application.Models.Authentication
{
    public record RegisterRequest(
        string Username,
        string Email,
        string Password);
}
 