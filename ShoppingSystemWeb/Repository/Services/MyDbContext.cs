using Microsoft.EntityFrameworkCore;
using ShoppingSystemWeb.Data;
using ShoppingSystemWeb.Models;
using ShoppingSystemWeb.Repository.Interfaces;

namespace ShoppingSystemWeb.Repository.Services
{
    public class MyDbContext : DbContext, IDbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
            //Database.EnsureCreated();
        }
        public DbSet<Product> Products { get; set; }
    }
}
