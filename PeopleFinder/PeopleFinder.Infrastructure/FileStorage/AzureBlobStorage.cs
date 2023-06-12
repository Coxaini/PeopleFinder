using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using PeopleFinder.Application.Common.Interfaces.FileStorage;
using PeopleFinder.Application.Models.File;
using PeopleFinder.Domain.Entities.MessagingEntities;

namespace PeopleFinder.Infrastructure.FileStorage;

public class AzureBlobStorage : IFileStorageManager
{
    private readonly BlobContainerClient _filesContainer;

    public AzureBlobStorage(IConfiguration configuration)
    {
        var connectionString = configuration.GetValue<string>("azureBlob")!;
        var blobServiceClient = new BlobServiceClient(connectionString);
        _filesContainer = blobServiceClient.GetBlobContainerClient("files");
    }
    public async Task<MediaFile> UploadFileAsync(FileDto fileDto)
    {
        var uploadTime = DateTime.UtcNow;
        
        string ext = Path.GetExtension(fileDto.FileName);
        var token = Guid.NewGuid();
        
        string fileName = token + ext;
        
        var blobClient = _filesContainer.GetBlobClient(fileName);
        await blobClient.UploadAsync(fileDto.ContentStream);
        
        var mediaFile = new MediaFile()
        {
            Id = token,
            OriginalName = fileDto.FileName,
            Type = fileDto.Type,
            Extension = ext[1..],
            UploadTime = uploadTime
        };

        return mediaFile;
    }

    public async Task<Stream> GetFileAsync(MediaFile mediaFile)
    {
        string fileName = mediaFile.Id.ToString() + '.' + mediaFile.Extension;
        var file = _filesContainer.GetBlobClient(fileName);
        if (!await file.ExistsAsync())
            throw new FileNotFoundException("File not found");

        var data = await file.OpenReadAsync();
        
        return data;
    }

    public async Task DeleteFileAsync(MediaFile mediaFile)
    {
        string fileName = mediaFile.Id.ToString() + '.' + mediaFile.Extension;
        var file = _filesContainer.GetBlobClient(fileName);
        if (!await file.ExistsAsync())
            throw new FileNotFoundException("File not found");
        
        await file.DeleteAsync();
        
        
    }
}