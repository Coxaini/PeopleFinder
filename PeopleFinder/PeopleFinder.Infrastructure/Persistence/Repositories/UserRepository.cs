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
    public class UserRepository : BaseRepo<User>, IUserRepository
    {

        //private static readonly List<User> _users = new List<User>();

       // protected readonly PeopleFinderDbContext _db;
        public UserRepository(PeopleFinderDbContext dbContext) : base(dbContext)
        {
           
        }

        public async Task<User?> GetByEmailOrLoginAsync(string emailOrLogin)
        {

            return await Context.Users
                .Include(u=>u.Profile)
                .FirstOrDefaultAsync((x) => x.Email == emailOrLogin || x.Profile!.Username == emailOrLogin);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await Context.Users.FirstOrDefaultAsync((x) => x.Email == email);
        }

        public async Task<User?> GetByUsernameAsync(string login)
        {
            return await Context.Users
                .Include(u=>u.Profile)
                .FirstOrDefaultAsync((x) => x.Profile!.Username == login);
        }

        public async Task<User?> GetByIdAsync(int id)
        {

            return await Context.Users.Include(u=>u.Profile).FirstOrDefaultAsync((x) => x.Id == id);
        }


    }
}
