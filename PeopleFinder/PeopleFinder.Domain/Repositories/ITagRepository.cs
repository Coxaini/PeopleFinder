using PeopleFinder.Domain.Entities;
using PeopleFinder.Domain.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleFinder.Domain.Repositories
{
    public interface ITagRepository : IRepo<Tag>
    {
        public Task<List<Tag>> GetByNames(ICollection<string> names);
    }
}
