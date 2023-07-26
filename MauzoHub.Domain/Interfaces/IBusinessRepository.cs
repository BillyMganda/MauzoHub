using MauzoHub.Domain.Entities;

namespace MauzoHub.Domain.Interfaces
{
    public interface IBusinessRepository : IRepository<Businesses>
    {
        Task<Businesses> GetBusinessByNameAsync(string name);
    }
}
