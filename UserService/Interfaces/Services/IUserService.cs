using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http;
using UserService.Models.Dto;
using UserService.Models.Pagination;
using System.Collections.Generic;

namespace UserService.Interfaces.Services
{
    public interface IUserService
    {
        Task<UserInfo> GetUserInfoAsync(Guid identifier);

        Task<PagedList<UserInfo>> GetUserPageAsync(PaginationParameters paginationParameters,
            UserSearchOptions searchOptions);

        List<RoleInfo> GetRolesAsync();

        Task UpdateUserAsync(Guid identifier, UserUpdateDto userUpdate);

        Task UpdateUserPermissionsAsync(Guid identifier, Guid adminIdentifier, UserPermissions userPermissions);

        Task PostUserImageAsync(Guid userIdentifier, IFormFile file);

        Task<byte[]> GetUserImageAsync(Guid identifier);

        Task CreateAddressAsync(Guid identifier, AddressCreationDto address);

        Task UpdateAddressAsync(Guid identifier, AddressUpdateDto address);

        Task DeleteAddressAsync(Guid userIdentifier, Guid identifier);

        Task<List<AddressInfo>> GetUserAddressesAsync(Guid userIdentifier);
    }
}
