using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductInventory.Models;
using ProductInventory.Services;

namespace ProductInventory.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
            _logger         = logger         ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        ///    Displays a list of all products in the inventory.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            try
            {
                var products = await _productService.ListAllProductsAsync();
                return View(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading products");
                return View(new List<ProductViewModel>());
            }
        }

        /// <summary>
        ///     Displays the details of a specific product by its ID.
        ///     GET: /Product/Details/5
        /// </summary>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            try
            {
                var product = await _productService.GetProductByIdAsync(id);

                if (product == null)
                    return NotFound();

                return View(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error loading product {id}");
                return BadRequest("Error loading product");
            }
        }

        /// <summary>
        ///    Displays the form to create a new product.
        ///    GET: /Product/Create
        /// </summary>
        public IActionResult Create()
        {
            return View();
        }
        /// <summary>
        ///   Receives the form data to create a new product and saves it to the database.
        ///   POST: /Product/Create
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,Price,Quantity")] ProductViewModel product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _productService.AddProduct(product);

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

        /// <summary>
        ///   Displays the form to edit an existing product by its ID.
        ///   GET: /Product/Edit/5
        /// </summary>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            try
            {
                var product = await _productService.GetProductByIdAsync(id);

                if (product == null)
                    return NotFound();

                return View(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error loading product {id} for edit");
                return BadRequest("Error loading product");
            }
        }

        /// <summary>
        ///   Receives the updated product data from the form and saves changes to the database.
        ///   POST: /Product/Edit/5
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,Quantity")] ProductViewModel product)
        {
            if (id != product.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    await _productService.UpdateProduct(product);

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    _logger.LogError(ex, "Concurrency error updating product");

                    if (!await ProductExists(product.Id))
                        return NotFound();
                    else
                        throw;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating product");
                    ModelState.AddModelError("", "Error updating product: " + ex.Message);
                }
            }

            return View(product);
        }

        /// <summary>
        ///   Displays the confirmation page to delete a specific product by its ID.
        ///   GET: /Product/Delete/5
        /// </summary>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            try
            {
                var product = await _productService.GetProductByIdAsync(id);

                if (product == null)
                    return NotFound();

                return View(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error loading product {id} for deletion");
                return BadRequest("Error loading product");
            }
        }

        /// <summary>
        ///  Receives the confirmation to delete a product and removes it from the database.
        ///  POST: /Product/Delete/5
        /// </summary>
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);

                if (product != null)
                    await _productService.DeleteProductAsync(product);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting product {id}");
                return BadRequest("Error deleting product");
            }
        }


        /// <summary>
        ///     Check if product exists in database
        /// </summary>
        private async Task<bool> ProductExists(int id)
        {
            return await _productService.GetProductByIdAsync(id) != null;
        }
    }
}
