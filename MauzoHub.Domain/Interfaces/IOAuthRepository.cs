namespace MauzoHub.Domain.Interfaces
{
    public interface IOAuthRepository<T, V, W, X, Y, Z> where T : class
    {
        Task<T> LoginAsync(V LoginRequest);
        Task<T> RefreshTokenAsync(W RefreshTokenRequest);
        Task<T> LogoutAsync(X LogoutRequest);
        Task<T> RememberPasswordAsync(Y RememberPasswordRequest);
        Task<T> ResetPasswordAsync(Z ResetPasswordRequest);
    }
}
