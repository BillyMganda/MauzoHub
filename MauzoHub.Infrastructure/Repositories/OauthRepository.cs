using MauzoHub.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
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

        public Task<string> GeneratePasswordResetTokenAsync(string email)
        {
            const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var resetToken = new string(Enumerable.Repeat(characters, 6)
                                            .Select(s => s[random.Next(s.Length)])
                                            .ToArray());
            return Task.FromResult(resetToken);
        }
        public async Task SendPasswordResetTokenEmailAsync(string email, string resetToken)
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
                mailMessage.Body = $"Your password reset token is: {resetToken}";

                using (var smtpClient = new SmtpClient(smtpServer, smtpPort))
                {
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                    smtpClient.EnableSsl = true;
                    
                    await smtpClient.SendMailAsync(mailMessage);
                }
            }
        }

        public Task<bool> ValidatePasswordResetTokenAsync(string email, string token)
        {

        }

        public Task<bool> ResetPasswordAsync(string email, string token, string newPassword)
        {

        }
    }
}
