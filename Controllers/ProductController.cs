using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductInventory.Models;

namespace ProductInventory.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProductController> _logger;

        public ProductController(ApplicationDbContext context, ILogger<ProductController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                _logger.LogInformation("Getting all products");

                var products = await _context.Products.ToListAsync();

                return View(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading products");
                ViewBag.Error = "Error loading products: " + ex.Message;
                return View(new List<Product>());
            }
        }

        // GET: /Product/Details/5
        // Shows details of a single product
        // id parameter comes from URL: /Product/Details/{id}
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

                if (product == null)
                {
                    return NotFound();
                }

                return View(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error loading product {id}");
                return BadRequest("Error loading product");
            }
        }

        // ==================== CREATE OPERATIONS ====================

        // GET: /Product/Create
        // Shows empty form for creating new product
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Product/Create
        // Receives form data and saves new product
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,Price,Quantity")] Product product)
        {
            // [Bind] specifies which properties to accept from form
            // Prevents mass assignment attacks

            // Check if model data is valid
            // Validates all [Required], [StringLength], etc. attributes
            if (ModelState.IsValid)
            {
                try
                {
                    product.CreatedDate = DateTime.Now;

                    // Add product to database
                    _context.Products.Add(product);

                    // Save changes asynchronously
                    await _context.SaveChangesAsync();

                    _logger.LogInformation($"Product created: {product.Name}");

                    // Redirect to list view
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating product");
                    ModelState.AddModelError("", "Error saving product: " + ex.Message);
                }
            }

            // If validation fails, show form again with error messages
            return View(product);
        }

        // ==================== UPDATE OPERATIONS ====================

        // GET: /Product/Edit/5
        // Shows form pre-filled with product data
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var product = await _context.Products.FindAsync(id);

                if (product == null)
                {
                    return NotFound();
                }

                return View(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error loading product {id} for edit");
                return BadRequest("Error loading product");
            }
        }

        // POST: /Product/Edit/5
        // Receives updated form data and saves changes
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,Quantity,CreatedDate")] Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Set the modified timestamp
                    product.ModifiedDate = DateTime.Now;

                    // Mark entity as modified and update
                    _context.Update(product);

                    // Save changes asynchronously
                    await _context.SaveChangesAsync();

                    _logger.LogInformation($"Product updated: {product.Name}");

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    _logger.LogError(ex, "Concurrency error updating product");

                    // Check if product still exists
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating product");
                    ModelState.AddModelError("", "Error updating product: " + ex.Message);
                }
            }

            return View(product);
        }

        // ==================== DELETE OPERATIONS ====================

        // GET: /Product/Delete/5
        // Shows confirmation page before deleting
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

                if (product == null)
                {
                    return NotFound();
                }

                return View(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error loading product {id} for deletion");
                return BadRequest("Error loading product");
            }
        }

        // POST: /Product/Delete/5
        // Confirms deletion and removes product from database
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);

                if (product != null)
                {
                    _context.Products.Remove(product);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation($"Product deleted: {product.Name}");
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting product {id}");
                return BadRequest("Error deleting product");
            }
        }

        // ==================== HELPER METHODS ====================

        // Check if product exists in database
        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
