using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentResults;

namespace PeopleFinder.Application.Common.Errors
{
    public static class ProfileErrors
    {
        public static readonly BaseError ProfileNotFound = new("Profile not found", ErrorType.NotFound);

        public static readonly BaseError ProfileAlreadyExists = new("Profile already exists", ErrorType.Conflict);

        public static readonly BaseError RatedProfileNotFound = new("Rated profile doesn't exists", ErrorType.NotFound);

        public static readonly BaseError AccessToRateDenied =
            new("You don't have access to rate this profile", ErrorType.AccessDenied);
        
        public static readonly BaseError ProfileAlreadyRated = new("You already rated this profile", ErrorType.Conflict);
        
        
        
        
    }
}
