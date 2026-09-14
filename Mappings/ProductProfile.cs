namespace ProductInventory.Mappings
{
    using AutoMapper;
    using ProductInventory.Models;

    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductViewModel>();
            CreateMap<ProductViewModel, Product>();
        }
    }
}
