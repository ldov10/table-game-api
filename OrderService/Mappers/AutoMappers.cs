using AutoMapper;
using OrderService.Models.Dto;
using OrderService.Models.Entities;
using OrderService.Models.Messages;

namespace OrderService.Mappers
{
    public class AutoMappers : Profile
    {
        public AutoMappers()
        {
            Address();
            Product();
        }

        private void Address()
        {
            CreateMap<OrderCreationAddressDto, Address>();
        }

        private void Product()
        {
            CreateMap<ProductActivatedMessage, ActiveProduct>();

            CreateMap<OrderProductMapping, ProductInfo>();
        }
    }
}
