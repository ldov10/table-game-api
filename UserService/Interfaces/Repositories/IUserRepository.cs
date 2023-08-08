using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserService.Models.Dto;
using UserService.Models.Entities;
using UserService.Models.Pagination;

namespace UserService.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserAsync(string username);

        Task<User> GetUserIncludeInactiveAsync(string username);

        Task<User> GetUserAsync(Guid identifier);

        Task<User> GetUserIncludeInactiveAsync(Guid identifier);

        Task<User> GetUserByRefreshTokenAsync(string refreshToken);

        Task<long> GetUsersCountAsync();

        Task UpdateUserAsync(User user);

        Task<List<User>> GetUsersPageAsync(PaginationParameters pagination,
            UserSearchOptions userSearchOptions = null);

        Task SaveUserAsync(User user);

        Task RemoveRefreshTokenAsync(Guid identifier);

        Task SaveRefreshTokenAsync(RefreshToken token);

        Task SaveUserAddressAsync(Address address);

        Task UpdateUserAddressAsync(Address address);

        Task DeleteUserAddressAsync(Address address);

        Task<List<Address>> GetUserAddressesAsync(long userId);

        Task<Address> GetUserAddressAsync(Guid identifier);
    }
}
