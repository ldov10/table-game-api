using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserService.Context;
using UserService.Helpers;
using UserService.Interfaces.Repositories;
using UserService.Models.Dto;
using UserService.Models.Entities;
using UserService.Models.Pagination;

namespace UserService.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ServiceDbContext _context;

        public UserRepository(ServiceDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserAsync(string username)
        {
            username = username.Trim().ToUpper();

            return await _context.Users
                .Include(x => x.RefreshToken)
                .FirstOrDefaultAsync(x => string.Equals(x.Username, username) && x.IsActive);
        }

        public async Task<User> GetUserIncludeInactiveAsync(string username)
        {
            username = username.Trim().ToUpper();

            return await _context.Users
                .Include(x => x.RefreshToken)
                .FirstOrDefaultAsync(x => string.Equals(x.Username, username));
        }

        public async Task<User> GetUserAsync(Guid identifier)
        {
            return await _context.Users
                .Include(x => x.RefreshToken)
                .FirstOrDefaultAsync(x => x.Identifier == identifier && x.IsActive);
        }

        public async Task<User> GetUserIncludeInactiveAsync(Guid identifier)
        {
            return await _context.Users
                .Include(x => x.RefreshToken)
                .FirstOrDefaultAsync(x => x.Identifier == identifier);
        }

        public async Task<User> GetUserByRefreshTokenAsync(string refreshToken)
        {
            return await _context.Users
                .Include(x => x.RefreshToken)
                .FirstOrDefaultAsync(x => x.RefreshToken.Token == refreshToken && x.IsActive);
        }

        public async Task<long> GetUsersCountAsync()
        {
            return await _context.Users.CountAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);

            await _context.SaveChangesAsync();
        }

        public async Task<List<User>> GetUsersPageAsync(PaginationParameters pagination,
            UserSearchOptions userSearchOptions = null)
        {
            var baseQuery = _context.Users;

            if (userSearchOptions == null)
            {
                return await PaginationHelper.GetPagedListAsync(baseQuery, pagination);
            }

            var searchQuery = SearchHelper.BuildSearchQuery(baseQuery, userSearchOptions);

            return await PaginationHelper.GetPagedListAsync(searchQuery, pagination);
        }

        public async Task SaveUserAsync(User user)
        {
            await _context.Users.AddAsync(user);

            await _context.SaveChangesAsync();
        }

        public async Task RemoveRefreshTokenAsync(Guid identifier)
        {
            var token = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Identifier == identifier);

            if (token != null)
            {
                _context.RefreshTokens.Remove(token);

                await _context.SaveChangesAsync();
            }
        }

        public async Task SaveRefreshTokenAsync(RefreshToken token)
        {
            await _context.RefreshTokens.AddAsync(token);

            await _context.SaveChangesAsync();
        }

        public async Task SaveUserAddressAsync(Address address)
        {
            await _context.Addresses.AddAsync(address);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAddressAsync(Address address)
        {
            _context.Addresses.Update(address);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAddressAsync(Address address)
        {
            _context.Addresses.Remove(address);

            await _context.SaveChangesAsync();
        }

        public async Task<List<Address>> GetUserAddressesAsync(long userId)
        {
            return await _context.Addresses.Where(x => x.UserId == userId && x.User.IsActive).ToListAsync();
        }

        public async Task<Address> GetUserAddressAsync(Guid identifier)
        {
            return await _context.Addresses.FirstOrDefaultAsync(x => x.Identifier == identifier && x.User.IsActive);
        }
    }
}
