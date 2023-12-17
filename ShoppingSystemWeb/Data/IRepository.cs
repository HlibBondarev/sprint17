using Microsoft.EntityFrameworkCore;
using ShoppingSystemWeb.Models;
using System.Linq.Expressions;

namespace ShoppingSystemWeb.Data
{
    public interface IRepository
    {
        IdbSetRepository<Product> Product { get; set; }

        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
