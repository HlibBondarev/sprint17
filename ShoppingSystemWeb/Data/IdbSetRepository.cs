using Microsoft.EntityFrameworkCore.ChangeTracking;
using ShoppingSystemWeb.Models;
using System.Linq.Expressions;

namespace ShoppingSystemWeb.Data
{
    public interface IdbSetRepository<T> : IQueryable<T> where T : class
    {
        int Count();
        Task<int> CountAsync();
        T FirstOrDefault(Expression<Func<T, bool>> predicate);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
        EntityEntry<T> Add(T entity);
        ValueTask<EntityEntry<T>> AddAsync(T entity);
        T Find(T entity);
        T Find(params object[] keyValue);
        ValueTask<T> FindAsync(params object[] keyValue);
        EntityEntry<T> Update(T entity);
        EntityEntry<T> Remove(T entity);
    }
}
