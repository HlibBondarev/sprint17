using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ShoppingSystemWeb.Repository.Interfaces;

namespace ShoppingSystemWeb.Data
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly IShoppingSystemWebContext _context;
        private readonly DbSet<T> _set;

        public Repository(IShoppingSystemWebContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _set = context.Set<T>();
        }

        public IQueryable<T> Query() => _set.AsQueryable();

        public async Task<IQueryable<T>> GetAllAsync() => (await _set.ToListAsync()).AsQueryable();

        public async Task<T?> GetByIdAsync(int? id) => await _set.FindAsync(id);

        public async ValueTask<EntityEntry<T>> AddAsync(T entity)
        {
            //await _set.AddAsync(entity);
            //await _context.SaveChangesAsync();
            var entry = await _set.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entry;
        }

        public async Task UpdateAsync(T entity)
        {
            _set.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int? id)
        {
            var entity = await _set.FindAsync(id);
            if (entity != null)
            {
                _set.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
