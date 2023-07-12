using MauzoHub.Domain.Entities;

namespace MauzoHub.Domain.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {        
        User GetByEmail(string email);
    }
}
