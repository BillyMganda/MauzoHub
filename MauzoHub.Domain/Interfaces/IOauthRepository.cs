namespace MauzoHub.Domain.Interfaces
{
    public interface IOauthRepository
    {
        // Login
        void CreatePasswordHash(string password, out string passwordHash, out string passwordSalt);        
        bool VerifyPasswordHash(string password, string passwordHash, string passwordSalt);
        string CreateJwtToken(string email);

        // Forgot Password
        string GeneratePasswordResetTokenAsync(string email);
        Task SendPasswordResetTokenEmailAsync(string email, string token);
        Task UpdateResetTokenFieldInDatabase(string email, string token);
        Task<bool> ValidatePasswordResetTokenAsync(string token);
        Task<bool> ResetPasswordAsync(string token, string newPassword);
    }
}
