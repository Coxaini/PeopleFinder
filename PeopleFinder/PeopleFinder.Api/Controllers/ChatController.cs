
using MapsterMapper;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Mvc;
using PeopleFinder.Api.Common.Extensions;
using PeopleFinder.Api.Controllers.Common;
using PeopleFinder.Application.Services.ChatServices;

namespace PeopleFinder.Api.Controllers
{
    [Route("/chats")]
    public class ChatController : ApiController
    {
        [HttpPost]
        public async Task<IActionResult> CreateChat()
        {
            return Ok();
        }
    }



}

