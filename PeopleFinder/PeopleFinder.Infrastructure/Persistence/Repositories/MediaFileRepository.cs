using Microsoft.EntityFrameworkCore;
using PeopleFinder.Domain.Entities;
using PeopleFinder.Domain.Entities.MessagingEntities;
using PeopleFinder.Domain.Repositories;
using PeopleFinder.Infrastructure.Persistence.Common;

namespace PeopleFinder.Infrastructure.Persistence.Repositories;

public class MediaFileRepository :  BaseRepo<MediaFile>,IMediaFileRepository
{
    public MediaFileRepository(PeopleFinderDbContext context) : base(context)
    {
    }
    
    public async Task<MediaFile?> GetByToken(Guid token)
    {
        return await Context.MediaFiles.FirstOrDefaultAsync(x => x.Id == token);
    }
    
}