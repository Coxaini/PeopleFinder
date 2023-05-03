using Microsoft.AspNetCore.Mvc;
using PeopleFinder.Api.Common.Extensions;
using PeopleFinder.Api.Controllers.Common;
using PeopleFinder.Application.Services.FileStorage;

namespace PeopleFinder.Api.Controllers;

public class FileController : ApiController
{
    private readonly IFileService _fileService;

    public FileController(IFileService fileService)
    {
        _fileService = fileService;
    }
    
    [HttpGet("{token:guid}")]
    public async Task<IActionResult> GetFile(Guid token)
    {
        var result = await _fileService.GetFileAsync(token);

       return result.Match(
               (file) => File(file.Content, "application/octet-stream", file.OriginalFileName),
              Problem
         );
    }
}