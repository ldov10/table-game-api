using System;
using AutoMapper;
using System.Threading.Tasks;
using UserService.Exceptions;
using UserService.Interfaces.Repositories;
using UserService.Interfaces.Services;
using UserService.Models.Entities;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using UserService.Models.Dto;
using UserService.Models.Pagination;
using UserService.Models.Enums;

namespace UserService.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IImageRepository _imageRepository;

        private const long FileMaxSize = 5242880;

        public UserService(IMapper mapper,
            IUserRepository userRepository,
            IImageRepository imageRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _imageRepository = imageRepository;
        }

        public async Task<UserInfo> GetUserInfoAsync(Guid identifier)
        {
            var user = await _userRepository.GetUserAsync(identifier);

            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }

            return _mapper.Map<User, UserInfo>(user);
        }

        public async Task<PagedList<UserInfo>> GetUserPageAsync(PaginationParameters paginationParameters,
            UserSearchOptions searchOptions)
        {
            var usersCount = await _userRepository.GetUsersCountAsync();

            var users = await _userRepository.GetUsersPageAsync(paginationParameters, searchOptions);

            var pagedUsers = _mapper.Map<List<UserInfo>>(users);

            return new PagedList<UserInfo>(pagedUsers, usersCount, paginationParameters);
        }

        public List<RoleInfo> GetRolesAsync()
        {
            var result = new List<RoleInfo>();

            var roles = Enum.GetValues(typeof(Roles)).Cast<Roles>();

            foreach (var role in roles)
            {
                result.Add(new RoleInfo
                {
                    RoleId = (int)role,
                    RoleName = role.ToString()
                });
            }

            return result;
        }

        public async Task UpdateUserAsync(Guid identifier, UserUpdateDto userUpdate)
        {
            var user = await _userRepository.GetUserAsync(identifier);

            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }

            var userByUsername = await _userRepository.GetUserAsync(userUpdate.Username);

            if (userByUsername != null)
            {
                throw new InternalException("Invalid username.");
            }

            user.Email = userUpdate.Email.Trim();
            user.Username = userUpdate.Username.Trim();
            user.FirstName = userUpdate.FirstName.Trim();
            user.LastName = userUpdate.LastName.Trim();

            await _userRepository.UpdateUserAsync(user);
        }

        public async Task UpdateUserPermissionsAsync(Guid identifier, Guid adminIdentifier, UserPermissions userPermissions)
        {
            var user = await _userRepository.GetUserIncludeInactiveAsync(identifier);

            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }

            if (user.Identifier == adminIdentifier)
            {
                throw new NotFoundException("Invalid user.");
            }

            if ((int)userPermissions.Role <= 0 || (int)userPermissions.Role > Enum.GetNames(typeof(Roles)).Length)
            {
                throw new InternalException("Invalid Role.");
            }

            user.Role = userPermissions.Role;

            if (userPermissions.Role != Roles.Admin)
            {
                user.IsActive = userPermissions.IsActive;
            }

            await _userRepository.UpdateUserAsync(user);
        }

        public async Task PostUserImageAsync(Guid userIdentifier, IFormFile file)
        {
            var user = await _userRepository.GetUserAsync(userIdentifier);

            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }

            if (file.Length > FileMaxSize)
            {
                throw new InternalException("File is more then 5mb.");
            }

            if (file.Length == 0)
            {
                throw new InternalException("File length is 0.");
            }

            using var memoryStream = new MemoryStream();

            await file.CopyToAsync(memoryStream);

            var image = new Image
            {
                User = user,
                Data = memoryStream.ToArray()
            };

            await _imageRepository.RemoveUserImagesAsync(user.Id);
            await _imageRepository.SaveImageAsync(image);
        }

        public async Task<byte[]> GetUserImageAsync(Guid identifier)
        {
            var user = await _userRepository.GetUserAsync(identifier);

            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }

            var images = await _imageRepository.GetUserImagesAsync(user.Id);

            if (images == null || !images.Any())
            {
                throw new NotFoundException("Images not found.");
            }

            return images.First().Data;
        }

        public async Task CreateAddressAsync(Guid identifier, AddressCreationDto address)
        {
            var user = await _userRepository.GetUserAsync(identifier);

            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }

            address.Phone = address.Phone.Trim();
            address.City = address.City.Trim();
            address.AddressString = address.AddressString.Trim();
            address.Country = address.Country.Trim();
            address.Zip = address.Zip.Trim();

            var newAddress = _mapper.Map<Address>(address);

            newAddress.User = user;

            await _userRepository.SaveUserAddressAsync(newAddress);
        }

        public async Task UpdateAddressAsync(Guid identifier, AddressUpdateDto address)
        {
            var user = await _userRepository.GetUserAsync(identifier);

            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }

            var existingAddress = await _userRepository.GetUserAddressAsync(address.Identifier);

            if (existingAddress == null)
            {
                throw new NotFoundException("Address not found.");
            }

            if (existingAddress.UserId != user.Id)
            {
                throw new InternalException("Invalid user.");
            }

            existingAddress.Phone = address.Phone.Trim();
            existingAddress.Zip = address.Zip.Trim();
            existingAddress.City = address.City.Trim();
            existingAddress.AddressString = address.AddressString.Trim();
            existingAddress.Country = address.Country.Trim();

            await _userRepository.UpdateUserAddressAsync(existingAddress);
        }

        public async Task DeleteAddressAsync(Guid userIdentifier, Guid identifier)
        {
            var user = await _userRepository.GetUserAsync(userIdentifier);

            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }

            var existingAddress = await _userRepository.GetUserAddressAsync(identifier);

            if (existingAddress == null)
            {
                throw new NotFoundException("Address not found.");
            }

            if (existingAddress.UserId != user.Id)
            {
                throw new InternalException("Invalid user.");
            }

            await _userRepository.DeleteUserAddressAsync(existingAddress);
        }

        public async Task<List<AddressInfo>> GetUserAddressesAsync(Guid userIdentifier)
        {
            var user = await _userRepository.GetUserAsync(userIdentifier);

            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }

            var addresses = await _userRepository.GetUserAddressesAsync(user.Id);

            return _mapper.Map<List<AddressInfo>>(addresses);
        }
    }
}
