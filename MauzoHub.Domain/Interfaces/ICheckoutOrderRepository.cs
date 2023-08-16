using MauzoHub.Domain.Entities;

namespace MauzoHub.Domain.Interfaces
{
    public interface ICheckoutOrderRepository
    {
        Task<IEnumerable<CheckoutOrder>> GetAllAsync();
        Task<CheckoutOrder> GetByIdAsync(Guid orderId);
        Task<IEnumerable<CheckoutOrder>> GetByUserIdAsync(Guid userId);
        Task<IEnumerable<CheckoutOrder>> GetOrdersBetweenDatesAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<CheckoutOrder>> GetOrdersForADateAsync(DateTime date);
        Task<IEnumerable<CheckoutOrder>> GetOrdersByProductIdAsync(Guid productId);
    }
}
