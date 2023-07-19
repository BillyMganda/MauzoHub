using MauzoHub.Domain.DTOs;

namespace MauzoHub.Domain.Interfaces
{
    public interface IOauthRepository
    {
        void CreatePasswordHash(string password, out string passwordHash, out string passwordSalt);        
        bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
        string CreateJwtToken(LoginRequest request);
    }
}
