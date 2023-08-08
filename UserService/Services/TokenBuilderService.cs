using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using UserService.Interfaces.Services;

namespace UserService.Services
{
    public class TokenBuilderService : ITokenBuilderService
    {
        private readonly IConfiguration _config;

        public TokenBuilderService(IConfiguration cfg)
        {
            _config = cfg;
        }

        public string BuildToken(string username, Guid identifier, string role)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Secrets:secretKey"]));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var claims = new Claim[]
            {
                new("username", username),
                new("scope", role),
                new("identifier", identifier.ToString())
            };

            var expires = DateTime.Now + new TimeSpan(0, 0, 0, int.Parse(_config["JwtExpiresSec"]));

            var jwt = new JwtSecurityToken(claims: claims, signingCredentials: signingCredentials, expires: expires);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }

        public string GenerateRefreshToken()
        {
            using var rng = new  RNGCryptoServiceProvider();

            var randomBytes = new byte[64];
            rng.GetBytes(randomBytes);

            return Convert.ToBase64String(randomBytes);
        }
    }
}
