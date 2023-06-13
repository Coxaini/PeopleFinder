using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using PeopleFinder.Api.Common.Extensions;
using PeopleFinder.Api.Controllers.Common;
using PeopleFinder.Api.Hubs;
using PeopleFinder.Application.Models.Chat;
using PeopleFinder.Application.Models.File;
using PeopleFinder.Application.Models.Message;
using PeopleFinder.Application.Services.ChatServices;
using PeopleFinder.Application.Services.FileStorage;
using PeopleFinder.Contracts.Chats;
using PeopleFinder.Contracts.Notifications;
using PeopleFinder.Contracts.Pagination;
using PeopleFinder.Domain.Common.Pagination.Cursor;

namespace PeopleFinder.Api.Controllers;

[Route("/chats")]
public class ChatController : ApiController
{
    private readonly IChatService _chatService;
    private readonly IMapper _mapper;
    private readonly IHubContext<ChatHub, IChatHub> _hubContext;

    public ChatController(IChatService chatService, IMapper mapper, IHubContext<ChatHub,IChatHub> hubContext)
    {
        _chatService = chatService;
        _mapper = mapper;
        _hubContext = hubContext;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetChats([FromQuery]CursorPagination<DateTime> pagination)
    {
        CursorPaginationParams<DateTime> paginationParams = new(20)
            { PageSize = pagination.PageSize, After = pagination.After, Before = pagination.Before };
        
        var result = await _chatService.GetChats(ProfileIdInClaims , paginationParams);

        return result.Match(
            (chats) =>
            {
                var metadata = new
                {
                    NextCursor =  chats.Next?.LastMessageAt ?? chats.Next?.CreatedAt,
                };
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

                return Ok(_mapper.Map<IList<UserChatResponse>>(chats.Items));
            },
            Problem
        );
    }
    
    [HttpPost("{friendId:int}")]
    public async Task<IActionResult> StartDirectChat([FromRoute]int friendId)
    {
        var chatRequest = new CreateDirectChatRequest(ProfileIdInClaims, friendId);
        var chatResult = await _chatService.CreateDirectChat(chatRequest);
        

        return chatResult.Match(
            (chat) =>
            {
                if(chat.IsNew) 
                    _hubContext.Clients.User(friendId.ToString()).DirectChatCreated(chat.Chat.Id);
                
                return CreatedAtAction("GetChat", new { ChatId = chat.Chat.Id }, 
                    new ChatResponse(chat.Chat.Id, chat.IsNew));
            },
            Problem
        );
    }
    
    [HttpGet("{chatId:guid}")]
    public async Task<IActionResult> GetChat([FromRoute]Guid chatId)
    {
        var chatResult = await _chatService.GetChat(ProfileIdInClaims, chatId);
        
        return chatResult.Match(
            (chat) => Ok(_mapper.Map<UserChatResponse>(chat)),
            Problem
        );
    }
    
    [HttpGet("{profileId:int}")]
    public async Task<IActionResult> GetDirectChat([FromRoute]int profileId)
    {
        var chatResult = await _chatService.GetDirectChat(ProfileIdInClaims, profileId);
        
        return chatResult.Match(
            (chat) => Ok(_mapper.Map<UserChatResponse>(chat)),
            Problem
        );
    }
    
    [HttpDelete("{chatId:guid}")]
    public async Task<IActionResult> DeleteChat([FromRoute]Guid chatId)
    {
        var deleteResult = await _chatService.DeleteChat(ProfileIdInClaims, chatId);
        
        await _hubContext.Clients.Group(chatId.ToString()).ChatDeleted(chatId);
        
        return deleteResult.Match(
            Ok,
            Problem
        );
    }
   
    
    

}