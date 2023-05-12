using FluentResults;

namespace PeopleFinder.Application.Common.Errors;

public static class MessageErrors
{
    public static readonly IError MessageNotFound = new BaseError("Message not found", ErrorType.NotFound);
    public static readonly IError MessageNotBelongToProfile = new BaseError("Message does not belong to profile",
        ErrorType.AccessDenied);
    
}