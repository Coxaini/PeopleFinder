using FluentResults;

namespace PeopleFinder.Application.Common.Errors;

public static class ChatErrors
{
    public static readonly IError NotFriends = 
        new BaseError("Profile is not your friend. Chat is not created", ErrorType.Conflict);
    public static readonly IError CannotCreateChatWithSelf = 
        new BaseError("Cannot create chat with yourself", ErrorType.Conflict);
    
    public static readonly IError CannotGetChatWithSelf = 
        new BaseError("Cannot get chat with yourself", ErrorType.Conflict);
    
    public static readonly IError ChatNotFound = 
        new BaseError("Chat not found", ErrorType.NotFound);
    
    public static readonly IError ProfileNotInChat = 
        new BaseError("User is not in chat", ErrorType.NotFound);
    
    public static readonly IError ChatAlreadyExists = 
        new BaseError("Chat already exists", ErrorType.Conflict);


}