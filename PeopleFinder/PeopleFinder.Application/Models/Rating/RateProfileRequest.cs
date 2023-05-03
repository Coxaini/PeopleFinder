using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleFinder.Application.Models.Rating
{
    public record RateProfileRequest(int RatedUserId, bool IsLiked, string Comment);
 
}
