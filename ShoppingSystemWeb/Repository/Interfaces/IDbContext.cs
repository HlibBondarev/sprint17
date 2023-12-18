using Microsoft.EntityFrameworkCore;
using ShoppingSystemWeb.Models;

namespace ShoppingSystemWeb.Repository.Interfaces
{
    public interface IDbContext
    {
        DbSet<Product> Products { get; set; }
        // Add other DbSet properties if needed
        DbSet<T> Set<T>() where T : class;
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
