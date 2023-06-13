using PeopleFinder.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleFinder.Application.Models.Profile
{
    public record ProfileEditRequest(string Name,Gender Gender, string Username , string Bio, string City , DateOnly? BirthDate, List<string> Tags);

   /* public record ProfileEditRequest(int ProfileId, string Name,
      Gender Gender, string Bio, string City, Gender GenderInterest, DateOnly BirthDate, List<int> Tags);*/
  
}
