using AutoMapper;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ProductInventory.Controllers;
using ProductInventory.Models;

namespace ProductInventory.Services
{
    public interface IProductService
    {
        Task AddProduct(ProductViewModel product);
        Task UpdateProduct(ProductViewModel product);
        Task<ProductViewModel?> GetProductByIdAsync(int? id);
        Task<List<ProductViewModel>> ListAllProductsAsync();
        Task DeleteProductAsync(ProductViewModel product);
    }

    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProductController> _logger;
        private readonly IMapper _mapper;

        public ProductService(ApplicationDbContext context, ILogger<ProductController> logger, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger  = logger  ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task AddProduct(ProductViewModel product)
        {
            var productEntity = _mapper.Map<Product>(product);
            productEntity.CreatedDate = DateTime.Now;
            _context.Products.Add(productEntity);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Product created: {product.Name}");
        }

        public async Task DeleteProductAsync(ProductViewModel product)
        {
            var productEntity = _mapper.Map<Product>(product);
            _context.Products.Remove(productEntity);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Product deleted: {product.Name}");
        }

        public async Task<ProductViewModel?> GetProductByIdAsync(int? id)
        {
            var product = await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

            return product != null ? _mapper.Map<ProductViewModel>(product) : null;
        }

        public async Task<List<ProductViewModel>> ListAllProductsAsync()
        {
            _logger.LogInformation("Getting all products");

            var products = await _context.Products
                .AsNoTracking()
                .ToListAsync();

            return products.Select(p => _mapper.Map<ProductViewModel>(p)).ToList();
        }

        public async Task UpdateProduct(ProductViewModel product)
        {
            var productEntity = _mapper.Map<Product>(product);
            productEntity.ModifiedDate = DateTime.Now;

            _context.Update(productEntity);

            await _context.SaveChangesAsync();

            _logger.LogInformation($"Product updated: {product.Name}");
        }
    }

   
}
