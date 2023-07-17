using MauzoHub.Domain.DTOs;

namespace MauzoHub.Domain.Interfaces
{
    public interface IUserServiceRepository
    {
        Task<bool> IsValidUserAsync(LoginRequest request);
        UserRefreshTokens AddUserRefreshTokens(UserRefreshTokens user);
        UserRefreshTokens GetSavedRefreshTokens(string username, string refreshtoken);
        void DeleteUserRefreshTokens(string username, string refreshToken);
        int SaveCommit();
    }
}
