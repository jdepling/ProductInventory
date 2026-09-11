using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ProductInventory.Models
{
    public class Product
    {
        // Primary Key
        [Key]
        public int Id { get; set; }

        // Product Name - Required, 1-100 characters
        [Required(ErrorMessage = "Product name is required")]
        [StringLength(100, MinimumLength = 1,
            ErrorMessage = "Product name must be between 1 and 100 characters")]
        [Display(Name = "Product Name")]
        public string Name { get; set; } = string.Empty;

        // Description - Optional, max 500 characters
        [StringLength(500,
            ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }

        // Price - Required, between 0.01 and 999,999.99
        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 999999.99,
            ErrorMessage = "Price must be between 0.01 and 999,999.99")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        // Quantity - Required, non-negative
        [Required(ErrorMessage = "Quantity is required")]
        [Range(0, int.MaxValue,
            ErrorMessage = "Quantity cannot be negative")]
        public int Quantity { get; set; }

        // Read-only computed property - In stock if quantity > 0
        [NotMapped]
        [Display(Name = "In Stock")]
        public bool IsInStock => Quantity > 0;

        // Timestamps for auditing
        [Display(Name = "Created On")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "Last Modified")]
        public DateTime? ModifiedDate { get; set; }
    }
}
