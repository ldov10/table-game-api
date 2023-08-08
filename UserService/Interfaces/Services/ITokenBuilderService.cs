using System;

namespace UserService.Interfaces.Services
{
    public interface ITokenBuilderService
    {
        string BuildToken(string username, Guid identifier, string role);
        string GenerateRefreshToken();
    }
}
