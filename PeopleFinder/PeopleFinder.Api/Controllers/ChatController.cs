using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PeopleFinder.Api.Common.Extensions;
using PeopleFinder.Api.Controllers.Common;
using PeopleFinder.Application.Models.Chat;
using PeopleFinder.Application.Models.File;
using PeopleFinder.Application.Models.Message;
using PeopleFinder.Application.Services.ChatServices;
using PeopleFinder.Application.Services.FileStorage;
using PeopleFinder.Contracts.Chats;
using PeopleFinder.Contracts.Friends;
using PeopleFinder.Domain.Common.Pagination.Cursor;

namespace PeopleFinder.Api.Controllers;

[Route("/chats")]
public class ChatController : ApiController
{
    private readonly IChatService _chatService;
    private readonly IMapper _mapper;
    private readonly IFileTypeResolver _fileTypeResolver;

    public ChatController(IChatService chatService, IMapper mapper, IFileTypeResolver fileTypeResolver)
    {
        _chatService = chatService;
        _mapper = mapper;
        _fileTypeResolver = fileTypeResolver;
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

                return Ok(_mapper.Map<IList<ChatResponse>>(chats.Items));
            },
            Problem
        );
    }
    
    [HttpPost("direct")]
    public async Task<IActionResult> StartDirectChat([FromForm] StartDirectChat request)
    {
        var chatRequest = new CreateDirectChatRequest(ProfileIdInClaims, request.FriendId, request.Text,
FileDto.FromFormFile(request.Attachment,
    _fileTypeResolver.Resolve(request.Attachment?.FileName, request.Attachment?.Length)));
        var chatResult = await _chatService.CreateDirectChat(chatRequest);
        

        return chatResult.Match(
            (chat) => CreatedAtAction("GetChat", new {ChatId = chat.Id} ,_mapper.Map<ChatResponse>(chat)),
            Problem
        );
    }
    
    [HttpGet("{chatId:guid}")]
    public async Task<IActionResult> GetChat([FromRoute]Guid chatId)
    {
        var chatResult = await _chatService.GetChat(ProfileIdInClaims, chatId);
        
        return chatResult.Match(
            (chat) => Ok(_mapper.Map<ChatResponse>(chat)),
            Problem
        );
    }
    
    [HttpGet("{profileId:int}")]
    public async Task<IActionResult> GetDirectChat([FromRoute]int profileId)
    {
        var chatResult = await _chatService.GetDirectChat(ProfileIdInClaims, profileId);
        
        return chatResult.Match(
            (chat) => Ok(_mapper.Map<ChatResponse>(chat)),
            Problem
        );
    }
   
    
    

}