using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PeopleFinder.Api.Common.Extensions;
using PeopleFinder.Api.Controllers.Common;
using PeopleFinder.Application.Models.Chat;
using PeopleFinder.Application.Models.File;
using PeopleFinder.Application.Services.ChatServices;
using PeopleFinder.Contracts.Chats;
using PeopleFinder.Contracts.Friends;
using PeopleFinder.Domain.Common.Pagination.Cursor;

namespace PeopleFinder.Api.Controllers;

[Route("/chats")]
public class ChatController : ApiController
{
    private readonly IChatService _chatService;
    private readonly IMapper _mapper;

    public ChatController(IChatService chatService, IMapper mapper)
    {
        _chatService = chatService;
        _mapper = mapper;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetChats([FromQuery]CursorPagination<DateTime> paginationParams)
    {
        CursorPaginationParams<DateTime> pag = new(20)
            { PageSize = paginationParams.PageSize, After = paginationParams.After, Before = paginationParams.Before };
        
        var result = await _chatService.GetChats(ProfileIdInClaims , pag);

        return result.Match(
            (chats) =>
            {
                var metadata = new
                {
                    NextCursor =  chats.Next?.LastMessageAt ?? chats.Next?.CreatedAt,
                };
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

                return Ok(_mapper.Map<IEnumerable<ChatResponse>>(chats.Items));
            },
            Problem
        );
    }
    
    [HttpPost("direct")]
    public async Task<IActionResult> StartDirectChat([FromForm] StartDirectChat request)
    {
        var chatRequest = new CreateDirectChatRequest(ProfileIdInClaims, request.FriendId, request.Text,
FileDto.FromFormFile(request.Attachment));
        var chatResult = await _chatService.CreateDirectChat(chatRequest);

        if (chatResult.IsFailed)
        {
            return Problem(chatResult.Errors);
        }
        var chat = chatResult.Value;
        
        var messageRequest = new SendMessageRequest(ProfileIdInClaims,chat.Id,
            request.Text, FileDto.FromFormFile(request.Attachment));
        
        var messageResult = await _chatService.SendMessage(messageRequest);

        return messageResult.Match(
            (message) => Ok(_mapper.Map<ChatResponse>(message)),
            Problem
        );
    }
    
    

}