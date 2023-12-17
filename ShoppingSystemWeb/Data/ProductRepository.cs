using Microsoft.EntityFrameworkCore;
using ShoppingSystemWeb.Models;
using System.Linq.Expressions;

namespace ShoppingSystemWeb.Data
{
    public class ProductRepository : IRepository
    {
        private readonly ShoppingSystemWebContext _context;

        public ProductRepository(ShoppingSystemWebContext context)
        {
            _context = context;
        }

        public async Task<int> CountProductsAsync(Expression<Func<Product, bool>> predicate)
        {
            return await _context.Product.CountAsync(predicate);
        }
    }
}
