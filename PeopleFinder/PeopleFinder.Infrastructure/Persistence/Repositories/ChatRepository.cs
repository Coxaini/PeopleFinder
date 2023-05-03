using PeopleFinder.Domain.Entities.MessagingEntities;
using PeopleFinder.Domain.Repositories;
using PeopleFinder.Infrastructure.Persistence.Common;

namespace PeopleFinder.Infrastructure.Persistence.Repositories;

public class ChatRepository : BaseRepo<Chat>, IChatRepository
{
    public ChatRepository(PeopleFinderDbContext context) : base(context)
    {
    }
    
    
}