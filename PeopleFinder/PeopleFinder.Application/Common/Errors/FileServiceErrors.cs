using FluentResults;

namespace PeopleFinder.Application.Common.Errors;

public static class FileServiceErrors
{
    public static readonly IError FileNotFound = new BaseError("File not found", ErrorType.NotFound);
    
}