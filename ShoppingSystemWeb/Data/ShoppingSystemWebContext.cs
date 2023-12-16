#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShoppingSystemWeb.Models;

namespace ShoppingSystemWeb.Data
{
    public class ShoppingSystemWebContext : DbContext, IShoppingSystemWebContext
    {
        public ShoppingSystemWebContext(DbContextOptions<ShoppingSystemWebContext> options)
            : base(options)
        {
            //Database.EnsureCreated();
        }

        public virtual DbSet<Product> Product { get; set; }
    }
}
