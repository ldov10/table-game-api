using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserService.Interfaces.Services;
using UserService.Models.Dto;
using UserService.Models.Pagination;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("getUserInfo/{identifier}")]
        public async Task<IActionResult> GetUserInfo(Guid identifier)
        {
            return Ok(await _userService.GetUserInfoAsync(identifier));
        }
 
        [HttpGet("getUserPage/pageIndex/{pageIndex}/pageSize/{pageSize}")]
        public async Task<IActionResult> GetUserPage(int pageIndex, int pageSize, [FromQuery] UserSearchOptions userSearchOptions)
        {
            var pagination = new PaginationParameters(pageIndex, pageSize);

            return Ok(await _userService.GetUserPageAsync(pagination, userSearchOptions));
        }

        [HttpGet("getRoles")]
        public async Task<IActionResult> GetRoles()
        {
            return Ok(_userService.GetRolesAsync());
        }

        [HttpPut("updateUser/{identifier}")]
        public async Task<IActionResult> UpdateUser(Guid identifier, [FromBody] UserUpdateDto user)
        {
            await _userService.UpdateUserAsync(identifier, user);
            return Ok();
        }

        [HttpPut("updateUserPermissions/{identifier}/adminIdentifier/{adminIdentifier}")]
        public async Task<IActionResult> UpdateUserPermissions(Guid identifier, Guid adminIdentifier, [FromBody] UserPermissions userPermissions)
        {
            await _userService.UpdateUserPermissionsAsync(identifier, adminIdentifier, userPermissions);
            return Ok();
        }

        [HttpPost("postUserImage/userIdentifier/{userIdentifier}")]
        public async Task<IActionResult>PostUserImage(Guid userIdentifier, IFormFile file)
        {
            await _userService.PostUserImageAsync(userIdentifier, file);
            return Ok();
        }

        [HttpGet("getUserImage/{identifier}")]
        public async Task<IActionResult> PostUserImage(Guid identifier)
        {
            var image = await _userService.GetUserImageAsync(identifier);
            return File(image, "image/jpeg");
        }

        [HttpPut("updateUserAddress/{identifier}")]
        public async Task<IActionResult> UpdateUserAddress(Guid identifier, [FromBody] AddressUpdateDto address)
        {
            await _userService.UpdateAddressAsync(identifier, address);
            return Ok();
        }

        [HttpPost("postUserAddress/{identifier}")]
        public async Task<IActionResult> PostUserAddress(Guid identifier, AddressCreationDto address)
        {
            await _userService.CreateAddressAsync(identifier, address);
            return Ok();
        }

        [HttpGet("getUserAddresses/{identifier}")]
        public async Task<IActionResult> GetUserAddresses(Guid identifier)
        {
            return Ok(await _userService.GetUserAddressesAsync(identifier));
        }

        [HttpDelete("deleteUserAddress/userIdentifier/{userIdentifier}/address/{addressIdentifier}")]
        public async Task<IActionResult> DeleteUserAddress(Guid userIdentifier, Guid addressIdentifier)
        {
            await _userService.DeleteAddressAsync(userIdentifier, addressIdentifier);
            return Ok();
        }
    }
}
