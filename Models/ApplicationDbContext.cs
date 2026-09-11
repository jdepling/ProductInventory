using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ProductInventory.Models
{  
/// <summary>
///     Database context - manages database connections and entity operations
///     Inherits from DbContext (from Entity Framework Core)
/// </summary>
public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; } = null!;

        /// <summary>
        ///     Configure the model (optional)
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Product table
            modelBuilder.Entity<Product>(entity =>
            {
                // Table name in database
                entity.ToTable("Products");

                // Primary key
                entity.HasKey(e => e.Id);

                // Properties
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Description)
                    .HasMaxLength(500);

                entity.Property(e => e.Price)
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CreatedDate)
                    .HasDefaultValue(DateTime.Now);

                // Ignore computed property (not stored in database)
                entity.Ignore(e => e.IsInStock);
            });

            SeedData(modelBuilder);
        }

        /// <summary>
        ///  Add sample data when database is created
        /// </summary>
        /// <param name="modelBuilder"></param>
        private void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Laptop",
                    Description = "High-performance laptop",
                    Price = 999.99m,
                    Quantity = 5,
                    CreatedDate = DateTime.Now
                },
                new Product
                {
                    Id = 2,
                    Name = "Mouse",
                    Description = "Wireless mouse",
                    Price = 29.99m,
                    Quantity = 50,
                    CreatedDate = DateTime.Now
                },
                new Product
                {
                    Id = 3,
                    Name = "Keyboard",
                    Description = "Mechanical keyboard",
                    Price = 89.99m,
                    Quantity = 20,
                    CreatedDate = DateTime.Now
                }
            );
        }
    }
}
