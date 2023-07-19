using MauzoHub.Domain.DTOs;
using MauzoHub.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MauzoHub.Infrastructure.Repositories
{
    public class OauthRepository : IOauthRepository    {
        
        private readonly IConfiguration _configuration;
        public OauthRepository(IConfiguration configuration)
        {
            
            _configuration = configuration;
        }

        public void CreatePasswordHash(string password, out string passwordHash, out string passwordSalt)
        {
            using (var hmac = new HMACSHA256())
            {
                passwordSalt = Convert.ToBase64String(hmac.Key);
                var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                passwordHash = Convert.ToBase64String(hashBytes);
            }
        }                       

        public bool VerifyPasswordHash(string password, string passwordHash, string passwordSalt)
        {
            byte[] saltBytes = Convert.FromBase64String(passwordSalt);
            byte[] hashBytes = Convert.FromBase64String(passwordHash);

            using (var hmac = new HMACSHA256(saltBytes))
            {
                byte[] computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(hashBytes);
            }
        }

        public string CreateJwtToken(string email)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, email)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JWT:Key").Value!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken
                (
                claims: claims,
                expires: DateTime.Now.AddHours(5),
                signingCredentials: creds
                );

            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            return jwtToken;
        }
    }
}
