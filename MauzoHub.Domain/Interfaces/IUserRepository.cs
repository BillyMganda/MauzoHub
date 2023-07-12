using MauzoHub.Domain.Entities;

namespace MauzoHub.Domain.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {        
        Task<User> GetByEmailAsync(string email);
    }
}
