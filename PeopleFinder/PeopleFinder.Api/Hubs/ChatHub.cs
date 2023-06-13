using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using PeopleFinder.Application.Common.Interfaces.ConnectionStorage;
using PeopleFinder.Application.Services.ChatServices;
using PeopleFinder.Application.Services.ProfileServices;
using PeopleFinder.Contracts.Chats;

namespace PeopleFinder.Api.Hubs;

[Authorize]
public class ChatHub : Hub<IChatHub>
{
    private readonly IChatService _chatService;
    private readonly IProfileService _profileService;
    private readonly ILogger<ChatHub> _logger;
    private readonly IConnectionStorage _connectionStorage;

    public ChatHub(IChatService chatService, IProfileService profileService,
        ILogger<ChatHub> logger, IConnectionStorage connectionStorage)
    {
        _chatService = chatService;
        _profileService = profileService;
        _logger = logger;
        _connectionStorage = connectionStorage;
    }

    public override async Task OnConnectedAsync()
    {
        if (Context.UserIdentifier is { } userIdentifier&& int.TryParse(userIdentifier, out int userId))
        {
            var chats = await _chatService.GetAllChats(userId);
            
            _connectionStorage.Add(userId, Context.ConnectionId, out bool isNewUser);
            if (isNewUser) // if user is new, set him online
            {
                var profile = await _profileService.SetProfileOnline(userId);
                if(profile.IsSuccess)
                    await Clients.Group("Status_" + profile.Value.Username).UserOnline(profile.Value.Username);
                
                _logger.LogInformation("User ({UserId}) connected", userId);
            }
            
            if (chats.IsSuccess)
            {
                foreach (var chat in chats.Value)
                {
                    _logger.LogInformation("User ({UserId}) connected to the chat ({ChatId})", userId, chat.Id);
                    await Groups.AddToGroupAsync(Context.ConnectionId, chat.Id.ToString());
                }
            }
            else
            {
                chats.Errors
                    .ForEach(e=> _logger
                        .LogError("Error with retrieving user ({UserId}) chats with message : '{Message}'",
                            userId ,e.Message));
            }
            
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (Context.UserIdentifier is { } userIdentifier && int.TryParse(userIdentifier, out int userId))
        {
            _connectionStorage.Remove(userId, Context.ConnectionId, out bool userRemoved);
            
            if (userRemoved)
            {
                var profileResult = await _profileService.SetProfileOffline(userId);
                if(profileResult.IsSuccess){ 
                    await Clients.Group("Status_" + profileResult.Value.Username)
                        .UserOffline(profileResult.Value.Username,profileResult.Value.LastActivity);
                }
                _logger.LogInformation("User ({UserId}) disconnected", userId);
            }
        }
        await base.OnDisconnectedAsync(exception);
    }

    public async Task WatchUsersOnlineStatus(IEnumerable<string> usernames)
    {
        foreach (string username in usernames)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "Status_"+username );
            _logger.LogInformation("User ({UserId}) started watching user ({Username}) online status", Context.UserIdentifier, username);
        }
    }
    
    public async Task StopWatchingUserOnlineStatus(string username)
    {
        _logger.LogInformation("User ({UserId}) stopped watching user ({Username}) online status", Context.UserIdentifier, username);
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, "Status_"+ username);
    }
    
    public async Task JoinChat(Guid chatId)
    {
        if (Context.UserIdentifier is { } userIdentifier && int.TryParse(userIdentifier, out int userId))
        {
            var chat = await _chatService.GetChat(userId, chatId);
            if (chat.IsSuccess)
            {
                _logger.LogInformation("User ({UserId}) connected to the chat ({ChatId})", userId, chatId);
                await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
            }
            else
            {
                throw new HubException("Chat not found or you are not a member of this chat");
            }
        }
    }
    public async Task JoinGroups()
    {
        
    }

    public async Task LeaveGroup()
    {
        
    }
    
}