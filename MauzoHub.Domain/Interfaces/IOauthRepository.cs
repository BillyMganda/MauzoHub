namespace MauzoHub.Domain.Interfaces
{
    public interface IOauthRepository
    {
        void CreatePasswordHash(string password, out string passwordHash, out string passwordSalt);
    }
}
