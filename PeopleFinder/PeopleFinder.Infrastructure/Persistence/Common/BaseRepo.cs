using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PeopleFinder.Domain.Repositories.Common;

namespace PeopleFinder.Infrastructure.Persistence.Common
{
    public class BaseRepo<T> : IDisposable, IRepo<T> where T : class, new()
    {
        private readonly DbSet<T> _table;
        private readonly PeopleFinderDbContext _db;
        protected PeopleFinderDbContext Context => _db;


        public BaseRepo(PeopleFinderDbContext context)
        {
            _db = context;
            _table = _db.Set<T>();
        }
        public void Dispose()
        {
            _db?.Dispose();
        }

        public async Task AddAsync(T entity)
        {
            
            await _table.AddAsync(entity);
        }

        public async Task AddAsync(IList<T> entities)
        {
            await _table.AddRangeAsync(entities);
        }

        public void Update(T entity)
        {
            _table.Update(entity);

        }
        public void Update(IList<T> entities)
        {
            _table.UpdateRange(entities);

        }

        public void Delete(T entity)
        {
            _db.Entry(entity).State = EntityState.Deleted;
        }

        public async Task<T?> GetOne<TKey>(TKey id) where TKey : struct => await _table.FindAsync(id);

        public async Task<List<T>> GetAll()
        {
            return await _table.ToListAsync();
        }

        public async Task<List<T>> GetAll<TSortField>(Expression<Func<T, TSortField>> orderBy, bool ascending)
           => await (ascending ? _table.OrderBy(orderBy) : _table.OrderByDescending(orderBy)).ToListAsync();

        public async Task<List<T>> GetSome(Expression<Func<T, bool>> where)
        {
            return await _table.Where(where).ToListAsync(); 
        }

        public async Task<T?> GetFirstOrDefault(Expression<Func<T, bool>> match)
        {
            return await _table.FirstOrDefaultAsync(match);
        }
    }
}
