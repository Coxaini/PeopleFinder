using FluentResults;

namespace PeopleFinder.Application.Common.Errors;

public static class AuthenticationErrors
{
    public static readonly BaseError UserNotFound = new("User not found", ErrorType.NotFound);
    public static readonly BaseError UserAlreadyExists = new("User already exists", ErrorType.Conflict);
    public static  readonly  BaseError  IncorrectPassword  =  new ( "Incorrect password" ,  ErrorType.Validation) ;
    public static  readonly  BaseError UserLoginAlreadyExists = new("User with given username already exists", ErrorType.Conflict);

    public static readonly IError UserEmailAndLoginAlreadyExists =
        new BaseError("User with given email and username already exists", ErrorType.Conflict);
    public static readonly BaseError UserEmailAlreadyExists = new("User with given email already exists", ErrorType.Conflict);
    public static readonly IError InvalidToken = new BaseError("Invalid token", ErrorType.Validation);
    public static readonly IError RefreshTokenExpired = new BaseError("Refresh token expired", ErrorType.Validation);
    public static readonly IError InvalidRefreshToken = new BaseError("Invalid refresh token", ErrorType.Validation);
}