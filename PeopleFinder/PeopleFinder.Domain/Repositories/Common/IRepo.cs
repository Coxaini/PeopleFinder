using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PeopleFinder.Domain.Repositories.Common
{
    public interface IRepo<T>
    {
        Task AddAsync(T entity);
        Task AddAsync(IList<T> entities);
        void Update(T entity);
        void Update(IList<T> entities);
        //Task<int> Delete(int id);
        void Delete(T entity);
        Task<T?> GetOne<TKey>(TKey id) where TKey : struct;

        Task<List<T>> GetAll();
        Task<List<T>> GetSome(Expression<Func<T, bool>> where);
        Task<List<T>> GetAll<TSortField>(Expression<Func<T, TSortField>> orderBy, bool ascending);

        Task<T?> GetFirstOrDefault(Expression<Func<T, bool>> match);

        /*        List<T> GetSome(Expression<Func<T, bool>> where);
                List<T> GetAll();
                List<T> GetAll<TSortField>(Expression<Func<T, TSortField>> orderBy, bool ascending);

                List<T> ExecuteQuery(string sql);
                List<T> ExecuteQuery(string sql, object[] sqlParametersObjects);*/
    }
}
