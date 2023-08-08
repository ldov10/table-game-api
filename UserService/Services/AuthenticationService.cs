using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using UserService.Exceptions;
using UserService.Interfaces.Repositories;
using UserService.Interfaces.Services;
using UserService.Models.Dto;
using UserService.Models.Entities;
using UserService.Models.Enums;

namespace UserService.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IMapper _mapper;
        private readonly ITokenBuilderService _tokenBuilder;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;

        public AuthenticationService(ITokenBuilderService tokenBuilder,
            IUserRepository userRepository,
            IMapper mapper,
            IConfiguration config)
        {
            _tokenBuilder = tokenBuilder;
            _userRepository = userRepository;
            _mapper = mapper;
            _config = config;
        }

        public async Task<Tokens> LoginAsync(UserCredentials userCredentials)
        {
            var user = await _userRepository.GetUserAsync(userCredentials.Username);

            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }

            if (!BCrypt.Net.BCrypt.Verify(userCredentials.Password, user.Password))
            {
                throw new InternalException("Could not authenticate user.");
            }

            var token = _tokenBuilder.BuildToken(user.Username, user.Identifier, user.Role.ToString());

            var refreshToken = new RefreshToken
            {
                Token = _tokenBuilder.GenerateRefreshToken(),
                User = user,
                CreatedDtm = DateTime.Now
            };

            if (user.RefreshToken != null)
            {
                await _userRepository.RemoveRefreshTokenAsync(user.RefreshToken.Identifier);
            }

            await _userRepository.SaveRefreshTokenAsync(refreshToken);

            return new Tokens
            {
                AuthToken = token,
                RefreshToken = refreshToken.Token
            };
        }

        public async Task<Tokens> RegistrationAsync(UserCreationDto userCreationDto)
        {
            var dbUser = await _userRepository.GetUserIncludeInactiveAsync(userCreationDto.Username);

            if (dbUser != null)
            {
                throw new InternalException("User already registered");
            }

            userCreationDto.Username = userCreationDto.Username.Trim();
            userCreationDto.Email = userCreationDto.Email.Trim();
            userCreationDto.FirstName = userCreationDto.FirstName.Trim();
            userCreationDto.LastName = userCreationDto.LastName.Trim();
            userCreationDto.Password = userCreationDto.Password.Trim();

            var user = _mapper.Map<User>(userCreationDto);

            user.Password = BCrypt.Net.BCrypt.HashPassword(userCreationDto.Password);

            var refreshToken = new RefreshToken
            {
                Token = _tokenBuilder.GenerateRefreshToken(),
                User = user,
                CreatedDtm = DateTime.Now
            };

            await _userRepository.SaveUserAsync(user);
            await _userRepository.SaveRefreshTokenAsync(refreshToken);

            var token = _tokenBuilder.BuildToken(user.Username, user.Identifier, Roles.User.ToString());

            return new Tokens
            {
                AuthToken = token,
                RefreshToken = refreshToken.Token
            };
        }

        public async Task<Tokens> GetTokensAsync(Guid identifier)
        {
            var user = await _userRepository.GetUserAsync(identifier);

            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }

            var newRefreshToken = new RefreshToken
            {
                Token = _tokenBuilder.GenerateRefreshToken(),
                User = user,
                CreatedDtm = DateTime.Now
            };

            await _userRepository.RemoveRefreshTokenAsync(user.RefreshToken.Identifier);
            await _userRepository.SaveRefreshTokenAsync(newRefreshToken);

            var newJwtToken = _tokenBuilder.BuildToken(user.Username, user.Identifier, user.Role.ToString());

            return new Tokens
            {
                AuthToken = newJwtToken,
                RefreshToken = newRefreshToken.Token
            };
        }

        public async Task<Tokens> RefreshTokenVerificationAsync(string refreshToken)
        {
            var user = await _userRepository.GetUserByRefreshTokenAsync(refreshToken);

            if (user == null)
            {
                throw new InternalException("Invalid refresh token.");
            }

            var refreshTokenExpiresDays = int.Parse(_config["RefreshTokenExpiresDays"]);

            if (DateTime.Now - user.RefreshToken.CreatedDtm > new TimeSpan(refreshTokenExpiresDays, 0, 0, 0))
            {
                await _userRepository.RemoveRefreshTokenAsync(user.RefreshToken.Identifier);

                throw new InternalException("Invalid refresh token.");
            }

            var newRefreshToken = new RefreshToken
            {
                Token = _tokenBuilder.GenerateRefreshToken(),
                User = user,
                CreatedDtm = DateTime.Now
            };

            await _userRepository.SaveRefreshTokenAsync(newRefreshToken);

            var newJwtToken = _tokenBuilder.BuildToken(user.Username, user.Identifier, user.Role.ToString());

            return new Tokens
            {
                AuthToken = newJwtToken,
                RefreshToken = newRefreshToken.Token
            };
        }
    }
}
