using PeopleFinder.Domain.Entities.MessagingEntities;
using PeopleFinder.Domain.Repositories;
using PeopleFinder.Infrastructure.Persistence.Common;

namespace PeopleFinder.Infrastructure.Persistence.Repositories;

public class MessageRepository : BaseRepo<Message>, IMessageRepository
{
    public MessageRepository(PeopleFinderDbContext context) : base(context)
    {
    }
}