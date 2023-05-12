using System.Security.Cryptography;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PeopleFinder.Api.Common.Extensions;
using PeopleFinder.Api.Controllers.Common;
using PeopleFinder.Application.Models.File;
using PeopleFinder.Application.Models.Message;
using PeopleFinder.Application.Services.FileStorage;
using PeopleFinder.Application.Services.Messages;
using PeopleFinder.Contracts.Chats;
using PeopleFinder.Contracts.Friends;
using PeopleFinder.Contracts.Messages;
using PeopleFinder.Domain.Common.Pagination.Cursor;

namespace PeopleFinder.Api.Controllers;

[Route("/messages")]
public class MessageController : ApiController
{
    private readonly IMessageService _messageService;
    private readonly IMapper _mapper;
    private readonly IFileTypeResolver _fileTypeResolver;

    public MessageController(IMessageService messageService, IMapper mapper, IFileTypeResolver fileTypeResolver)
    {
        _messageService = messageService;
        _mapper = mapper;
        _fileTypeResolver = fileTypeResolver;
    }
    
    [HttpGet("{chatId:guid}")]
    public async Task<IActionResult> GetMessages([FromQuery]CursorPagination<DateTime> pagination, [FromRoute]Guid chatId)
    {
        CursorPaginationParams<DateTime> pag = new(20)
            { PageSize = pagination.PageSize, After = pagination.After, Before = pagination.Before };

        var messagesResult = await _messageService.GetMessages(ProfileIdInClaims, chatId, pag);
        
        return messagesResult.Match(
            (messages) =>
            {
                var metadata = new
                {
                    NextCursor =  messages.Next?.SentAt,
                    
                };
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                
                return Ok(_mapper.Map<IList<UserMessageResponse>>(messages.Items));
            },
            Problem);
    }
    
    [HttpPost]
    public async Task<IActionResult> SendMessage([FromForm] SendMessage request)
    {
        var messageRequest = new SendMessageRequest(ProfileIdInClaims, request.ChatId, request.Text, 
            FileDto.FromFormFile(request.Attachment,
                _fileTypeResolver.Resolve(request.Attachment?.FileName, request.Attachment?.Length)));
        var messageResult = await _messageService.SendMessage(messageRequest);
        
        return messageResult.Match(
            (message) =>
            {
                
                return Ok(_mapper.Map<MessageResponse>(message));
            },
            Problem);
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeleteMessage([FromBody]Guid messageId)
    {
        var messageResult = await _messageService.DeleteMessage(ProfileIdInClaims, messageId);
        
        return messageResult.Match(
            () => Ok("Message deleted"),
            Problem);
    }
    
    [HttpPut]
    public async Task<IActionResult> EditMessage([FromForm] EditMessage request)
    {
        var messageRequest = new EditMessageRequest(ProfileIdInClaims, request.MessageId, request.Text, 
            FileDto.FromFormFile(request.Attachment,
                _fileTypeResolver.Resolve(request.Attachment?.FileName, request.Attachment?.Length)));
        var messageResult = await _messageService.EditMessage(messageRequest);
        
        return messageResult.Match(
            (message) => Ok(_mapper.Map<MessageResponse>(message)),
            Problem);
    }
    
    
   

}