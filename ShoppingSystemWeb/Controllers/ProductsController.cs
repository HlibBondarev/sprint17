#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingSystemWeb.Data;
using ShoppingSystemWeb.Models;
using ShoppingSystemWeb.Services;

namespace ShoppingSystemWeb.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IRepository<Product> _productRepository;

        public ProductsController(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        // GET: Products
        public async Task<IActionResult> Index(int? pageNumber, string searchString)
        {
            int pageSize = 3;

            var products = await _productRepository.GetAllAsync();

            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.Title!.Contains(searchString) || s.Category!.Contains(searchString));

            }

            return View(/*await*/ PaginatedList<Product>.Create/*Async*/(products.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,ExpiredDate,Category,Price")] Product product)
        {
            if (ModelState.IsValid)
            {
                await _productRepository.AddAsync(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var product = await _context.Product.FindAsync(id);
            var product = await _productRepository.GetByIdAsync(id ?? 0);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ExpiredDate,Category,Price")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _productRepository.UpdateAsync(product);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productRepository.GetByIdAsync(id ?? 0);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var product = await _productRepository.GetByIdAsync(id ?? 0);
            if (product == null)
            {
                return NotFound();
            }
            await _productRepository.DeleteAsync(product.Id);
            return RedirectToAction(nameof(Index));
        }

        private async ValueTask<bool> ProductExists(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            return (product is not null);
        }
    }
}
