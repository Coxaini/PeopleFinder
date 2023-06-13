using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeopleFinder.Api.Common.Extensions;
using PeopleFinder.Api.Controllers.Common;
using PeopleFinder.Application.Services.FileStorage;
using PeopleFinder.Domain.Entities.MessagingEntities;

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
               (file) =>
               {
                   string contentType = file.FileType switch
                   {
                       MediaFileType.Image => "image/" + file.Extension,
                       MediaFileType.Video => "video/" + file.Extension,
                       MediaFileType.Audio => "audio/" + file.Extension,
                       _ => "application/octet-stream"
                   };
                   

                   return File(file.ContentStream, contentType, file.OriginalFileName, true);
               },
              Problem
         );
    }
}