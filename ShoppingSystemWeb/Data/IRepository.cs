using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ShoppingSystemWeb.Data
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> Query();
        Task<IQueryable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int? id);
        //Task AddAsync(T entity);
        ValueTask<EntityEntry<T>> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int? id);
    }
}
