using MauzoHub.Domain.Interfaces;

namespace MauzoHub.Infrastructure.Repositories
{
    public class OAuthRepository<T, V, W, X, Y, Z> : IOAuthRepository<T, V, W, X, Y, Z> where T : class
    {
        private readonly IUserRepository _userRepository;
        public OAuthRepository(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public Task<T> LoginAsync(V LoginRequest)
        {
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
    }
}
