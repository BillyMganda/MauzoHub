using MauzoHub.Domain.Entities;

namespace MauzoHub.Domain.Interfaces
{
    public interface ICheckoutRepository
    {
        Task<Checkout> GetByUserIdAsync(Guid userId);
        Task<Checkout> AddAsync(Checkout checkout);
        Task<Checkout> UpdateAsync(Checkout checkout);
    }
}
