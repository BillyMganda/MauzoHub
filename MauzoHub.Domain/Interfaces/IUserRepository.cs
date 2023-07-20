using MauzoHub.Domain.Entities;

namespace MauzoHub.Domain.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {        
        Task<User> GetByEmailAsync(string email);
        Task<bool> DisableUser(Guid Id);
        Task<bool> EnableUser(Guid Id);
        Task<User> GetByTokenAsync(string token);
    }
}
