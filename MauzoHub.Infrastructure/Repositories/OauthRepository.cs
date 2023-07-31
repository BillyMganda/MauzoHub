using MauzoHub.Domain.Entities;
using MauzoHub.Domain.Interfaces;
using MauzoHub.Infrastructure.Databases;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MauzoHub.Infrastructure.Repositories
{
    public class OauthRepository : IOauthRepository    {
        
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly IMongoCollection<RefreshTokens> _refreshTokensCollection;
        public OauthRepository(IConfiguration configuration, IUserRepository userRepository, IOptions<MauzoHubDatabaseSettings> databaseSettings)
        {
            _configuration = configuration;
            _userRepository = userRepository;

            var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
            var database = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);

            _refreshTokensCollection = database.GetCollection<RefreshTokens>(databaseSettings.Value.RefreshTokensCollectionName);
        }

        // Login
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
                new Claim(ClaimTypes.Email, email)
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

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public async Task<bool> ValidateRefreshToken(string token)
        {
            var refreshToken = await _refreshTokensCollection.Find(t => t.Token == token).FirstOrDefaultAsync();
            
            if(refreshToken == null || refreshToken.IsActive == false || refreshToken.ExpiryDate < DateTime.Now)
            {
                return false;
            }

            return true;
        }
        public async Task<RefreshTokens> SaveRefreshRoken(RefreshTokens refreshToken)
        {
            await _refreshTokensCollection.InsertOneAsync(refreshToken);
            return refreshToken;
        }
        // Forgot Password
        public string GeneratePasswordResetTokenAsync(string email)
        {
            const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var resetToken = new string(Enumerable.Repeat(characters, 6)
                                            .Select(s => s[random.Next(s.Length)])
                                            .ToArray());
            return resetToken;
        }
        public async Task SendPasswordResetTokenEmailAsync(string email, string token)
        {
            var smtpServer = _configuration.GetSection("SmtpSettings:SmtpServer").Value!;
            var smtpPort = Convert.ToInt32(_configuration.GetSection("SmtpSettings:SmtpPort").Value!);
            var smtpUsername = _configuration.GetSection("SmtpSettings:SsmtpUsername").Value!;
            var smtpPassword = _configuration.GetSection("SmtpSettings:SmtpPassword").Value!;
            

            using (var mailMessage = new MailMessage())
            {
                mailMessage.From = new MailAddress(smtpUsername);
                mailMessage.To.Add(email);
                mailMessage.Subject = "Password Reset Token";
                mailMessage.Body = $"Your password reset token is: {token}";

                using (var smtpClient = new SmtpClient(smtpServer, smtpPort))
                {
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                    smtpClient.EnableSsl = true;
                    
                    await smtpClient.SendMailAsync(mailMessage);
                }
            }
        }

        public async Task UpdateResetTokenFieldInDatabase(string email, string token)
        {
            var user = await _userRepository.GetByEmailAsync(email);

            user.LastModified = DateTime.Now;
            user.PasswordResetToken = token;
            user.ResetTokenExpiration = DateTime.Now.AddMinutes(30);
            
            await _userRepository.UpdateAsync(user);
        }

        public async Task<bool> ValidatePasswordResetTokenAsync(string token)
        {
            var user = await _userRepository.GetByTokenAsync(token);

            if (user == null || !string.Equals(user.PasswordResetToken, token, StringComparison.Ordinal))
            {
                return false;
            }

            if (DateTime.Now > user.ResetTokenExpiration)
            {                
                return false;
            }

            return true;
        }

        public async Task ResetPasswordAsync(string token, string newPassword)
        {
            var user = await _userRepository.GetByTokenAsync(token);

            (string salt, string hash) = GenerateSaltAndHash(newPassword);

            user.PasswordSalt = salt;
            user.PasswordHash = hash;
            user.LastModified = DateTime.Now;
            user.PasswordResetToken = "";
            user.ResetTokenExpiration = DateTime.MinValue;

            await _userRepository.UpdateAsync(user);            
        }

        private (string salt, string hash) GenerateSaltAndHash(string password)
        {
            using (var hmac = new HMACSHA256())
            {
                var salt = Convert.ToBase64String(hmac.Key);
                var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                var hash = Convert.ToBase64String(hashBytes);

                return (salt, hash);
            }
        }
    }
}
