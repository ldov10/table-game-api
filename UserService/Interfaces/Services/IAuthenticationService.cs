using System;
using System.Threading.Tasks;
using UserService.Models.Dto;

namespace UserService.Interfaces.Services
{
    public interface IAuthenticationService
    {
        Task<Tokens> LoginAsync(UserCredentials userCredentials);

        Task<Tokens> RegistrationAsync(UserCreationDto user);

        Task<Tokens> RefreshTokenVerificationAsync(string refreshToken);

        Task<Tokens> GetTokensAsync(Guid identifier);
    }
}
