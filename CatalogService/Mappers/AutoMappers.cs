using AutoMapper;
using CatalogService.Models.Dto;
using CatalogService.Models.Entities;
using CatalogService.Models.Messages;

namespace CatalogService.Mappers
{
    public class AutoMappers : Profile
    {
        public AutoMappers()
        {
            Products();
            Brands();
            Categories();
        }

        private void Products()
        {
            CreateMap<ProductCreationDto, Product>()
                .ForMember(x => x.Rating, opt => opt.MapFrom(src => 0))
                .ForMember(x => x.IsActive, opt => opt.MapFrom(src => true));

            CreateMap<Product, ProductsPageItem>()
                .ForMember(x => x.Brand, opt => opt.MapFrom(src => src.Brand.Title))
                .ForMember(x => x.Category, opt => opt.MapFrom(src => src.Category.Title)); ;

            CreateMap<Product, ProductDetails>()
                .ForMember(x => x.Brand, opt => opt.MapFrom(src => src.Brand.Title))
                .ForMember(x => x.Category, opt => opt.MapFrom(src => src.Category.Title));

            CreateMap<Product, ProductActivatedMessage>()
                .ForMember(x => x.ProductIdentifier, opt => opt.MapFrom(src => src.Identifier));

            CreateMap<Product, ProductInfo>();
        }

        private void Brands()
        {
            CreateMap<Brand, BrandDetails>();

            CreateMap<BrandCreationDto, Brand>();
        }

        private void Categories()
        {
            CreateMap<Category, CategoryDetails>();

            CreateMap<CategoryCreationDto, Category>();
        }
    }
}
