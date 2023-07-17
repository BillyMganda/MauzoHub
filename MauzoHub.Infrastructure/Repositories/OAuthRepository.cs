using MauzoHub.Application.DTOs;
using MauzoHub.Domain.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MauzoHub.Infrastructure.Repositories
{
    public class OAuthRepository<T, V, W, X, Y, Z> : IOAuthRepository<T, V, W, X, Y, Z> where T : class
    {
        private readonly IUserRepository _userRepository;
        public OAuthRepository(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        private string GenerateJwtToken(string userId, string userRole)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your_secret_key_here"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, userId),
                    new Claim(ClaimTypes.Role, userRole)
                };

            var tokenOptions = new JwtSecurityToken(
                issuer: "your_issuer",
                audience: "your_audience",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
                );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return tokenString;
        }
        public async Task<T> LoginAsync(LoginRequestDto LoginRequest)
        {
            //var user = await _userRepository.GetByEmailAsync(LoginRequest.Email);
            //var userId = user.Id.ToString();
            //var userRole = user.Role;

            //var token = GenerateJwtToken(userId, userRole);

            //return Task.FromResult<T>(token);

            throw new NotImplementedException();
        }

        public Task<T> LogoutAsync(X LogoutRequest)
        {
            throw new NotImplementedException();
        }

        public Task<T> RefreshTokenAsync(W RefreshTokenRequest)
        {
            throw new NotImplementedException();
        }

        public Task<T> RememberPasswordAsync(Y RememberPasswordRequest)
        {
            throw new NotImplementedException();
        }

        public Task<T> ResetPasswordAsync(Z ResetPasswordRequest)
        {
            throw new NotImplementedException();
        }

        public Task<T> LoginAsync(V LoginRequest)
        {
            throw new NotImplementedException();
        }
    }
}
