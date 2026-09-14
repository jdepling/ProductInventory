using Microsoft.EntityFrameworkCore;
using ProductInventory.Controllers;
using ProductInventory.Models;

namespace ProductInventory.Services
{
    public interface IProductService
    {
        Task AddProduct(Product product);
        Task UpdateProduct(Product product);
        Task<Product?> GetProductByIdAsync(int? id);
        Task<List<Product>> ListAllProductsAsync();
        Task DeleteProductAsync(Product product);
    }

    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProductController> _logger;

        public ProductService(ApplicationDbContext context, ILogger<ProductController> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger  = logger  ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task AddProduct(Product product)
        {
            product.CreatedDate = DateTime.Now;
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Product created: {product.Name}");
        }

        public async Task DeleteProductAsync(Product product)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Product deleted: {product.Name}");
        }

        public async Task<Product?> GetProductByIdAsync(int? id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<List<Product>> ListAllProductsAsync()
        {
            _logger.LogInformation("Getting all products");
            return await _context.Products.ToListAsync();
        }

        public async Task UpdateProduct(Product product)
        {
            product.ModifiedDate = DateTime.Now;

            _context.Update(product);

            await _context.SaveChangesAsync();

            _logger.LogInformation($"Product updated: {product.Name}");
        }
    }

   
}
