using MauzoHub.Domain.Entities;

namespace MauzoHub.Domain.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart> GetByUserIdAsync(Guid userId);
        Task AddAsync(Cart cart);
        Task UpdateAsync(Cart cart);
    }
}
