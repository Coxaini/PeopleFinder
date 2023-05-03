using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleFinder.Application.Common.Errors;


public enum ErrorType
{
    Failure,
    Unexpected,
    Validation,
    Conflict,
    NotFound,
    AccessDenied
}

public class BaseError : Error
{


    public BaseError(string message , ErrorType errorType = ErrorType.Failure) : base(message)
    {

        Metadata.Add("Type", errorType);
        Metadata.Add("Code", errorType.ToString());
    }


}
