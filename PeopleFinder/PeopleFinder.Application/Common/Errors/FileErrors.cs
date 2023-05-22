using FluentResults;

namespace PeopleFinder.Application.Common.Errors;

public static class FileErrors
{
    public static readonly IError FileNotFound = new BaseError("File not found", ErrorType.NotFound);
    public static readonly IError FileNotDeleted = new BaseError("File not deleted", ErrorType.Conflict);
    
}