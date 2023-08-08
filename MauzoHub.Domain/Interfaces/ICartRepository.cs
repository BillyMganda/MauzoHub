using MauzoHub.Domain.Entities;

namespace MauzoHub.Domain.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart> GetByUserIdAsync(Guid userId);
        Task<Cart> AddAsync(Cart cart);
        Task<Cart> UpdateAsync(Cart cart);
    }
}
