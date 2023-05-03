using Microsoft.EntityFrameworkCore;
using PeopleFinder.Domain.Entities;
using PeopleFinder.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PeopleFinder.Infrastructure.Persistence.Common;

namespace PeopleFinder.Infrastructure.Persistence.Repositories
{
    public class TagRepository : BaseRepo<Tag>, ITagRepository
    {
        public TagRepository(PeopleFinderDbContext context) : base(context)
        {
        }

        public Task<List<Tag>> GetByNames(ICollection<string> names)
        {
            return Context.Tags.Where(x => names.Contains(x.Name)).ToListAsync();
        }
    }
}
