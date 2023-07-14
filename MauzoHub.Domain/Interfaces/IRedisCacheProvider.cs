namespace MauzoHub.Domain.Interfaces
{
    public interface IRedisCacheProvider
    {
        Task<T> GetAsync<T>(string key);
        Task RemoveAsync(string key);
        Task SetAsync<T>(string key, T value, TimeSpan expiration);
    }
}
