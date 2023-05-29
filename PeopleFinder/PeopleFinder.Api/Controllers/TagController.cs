using Microsoft.AspNetCore.Mvc;
using PeopleFinder.Api.Common.Extensions;
using PeopleFinder.Api.Controllers.Common;
using PeopleFinder.Application.Services.Tags;

namespace PeopleFinder.Api.Controllers;

[Route("/tags")]
public class TagController : ApiController
{
    private readonly ITagService _tagService;

    public TagController(ITagService tagService)
    {
        _tagService = tagService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetTags()
    {
        var tagsResult = await _tagService.GetTags();

        return tagsResult.Match(
            tags => { return Ok(tags); },
            Problem);
    }
    
}