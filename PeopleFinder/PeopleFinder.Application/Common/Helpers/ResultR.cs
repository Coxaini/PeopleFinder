
using FluentResults;
using PeopleFinder.Application.Common.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleFinder.Application.Common.Helpers
{
    public static class ResultR
    {
        public static Result Fail(string message, ErrorType errorType)
        {
            return Result.Fail(new BaseError(message, errorType));
        } 
    }
}
