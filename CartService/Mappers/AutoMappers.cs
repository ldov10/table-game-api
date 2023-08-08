using AutoMapper;
using CartService.Models.Dto;
using CartService.Models.Entities;

namespace CartService.Mappers
{
    public class AutoMappers : Profile
    {
        public AutoMappers()
        {
            Cart();
        }

        private void Cart()
        {
            CreateMap<Cart, CartItem>();

            CreateMap<CartItemDto, Cart>();
        }
    }
}
