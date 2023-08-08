using AutoMapper;
using UserService.Models.Dto;
using UserService.Models.Entities;
using UserService.Models.Enums;

namespace UserService.Mappers
{
    public class AutoMappers : Profile
    {
        public AutoMappers()
        {
            Users();
            Reviews();
        }

        private void Users()
        {
            CreateMap<UserCreationDto, User>()
                .ForMember(x => x.Role, opt => opt.MapFrom(src => Roles.User))
                .ForMember(x => x.IsActive, opt => opt.MapFrom(src => true));

            CreateMap<User, UserInfo>();

            CreateMap<AddressCreationDto, Address>();

            CreateMap<Address, AddressInfo>();
        }

        private void Reviews()
        {
            CreateMap<ReviewCreationDto, Review>();

            CreateMap<Review, ProductReview>()
                .ForMember(x => x.UserFirstName, opt => opt.MapFrom(src => src.User.FirstName))
                .ForMember(x => x.UserIdentifier, opt => opt.MapFrom(src => src.User.Identifier));
        }
    }
}
