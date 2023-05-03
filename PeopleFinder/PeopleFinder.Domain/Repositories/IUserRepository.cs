using PeopleFinder.Domain.Entities;
using PeopleFinder.Domain.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleFinder.Domain.Repositories
{
    public interface IUserRepository : IRepo<User>
    {
        public  Task<User?> GetByIdAsync(int id);
        public Task<User?> GetByEmailOrLoginAsync(string emailOrLogin);
        
        public Task<User?> GetByEmailAsync(string email);
        public Task<User?> GetByUsernameAsync(string login);
        //public void Add(User user);

    }
}
