using PeopleFinder.Domain.Entities;
using PeopleFinder.Domain.Entities.MessagingEntities;
using PeopleFinder.Domain.Repositories.Common;

namespace PeopleFinder.Domain.Repositories;

public interface IMediaFileRepository : IRepo<MediaFile>
{
   Task<MediaFile?> GetByToken(Guid token);
}