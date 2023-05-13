using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using PeopleFinder.Application.Services.ChatServices;
using PeopleFinder.Contracts.Chats;

namespace PeopleFinder.Api.Hubs;

[Authorize]
public class ChatHub : Hub<IChatHub>
{
    private readonly IChatService _chatService;
    private readonly ILogger<ChatHub> _logger;

    public ChatHub(IChatService chatService, ILogger<ChatHub> logger)
    {
        _chatService = chatService;
        _logger = logger;
    }

    public override async Task OnConnectedAsync()
    {
        if (Context.UserIdentifier is { } userIdentifier&& int.TryParse(userIdentifier, out int userId))
        {
            var chats = await _chatService.GetAllChats(userId);

            if (chats.IsSuccess)
            {
                foreach (var chat in chats.Value)
                {
                    _logger.LogDebug("User ({UserId}) connected to the chat ({ChatId})", userId, chat.Id);
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

    public async Task JoinGroups()
    {
        
    }

    public async Task LeaveGroup()
    {
        
    }
    
}