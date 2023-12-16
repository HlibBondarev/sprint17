#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShoppingSystemWeb.Models;

namespace ShoppingSystemWeb.Data
{
    public interface IShoppingSystemWebContext
    {
        DbSet<Product> Product { get; set; }

        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }

}
