namespace MauzoHub.Domain.Entities
{
    public class CheckoutItem
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }

        public CheckoutItem(Guid productId, int quantity)
        {
            ProductId = productId;
            Quantity = quantity;
        }
    }
}
