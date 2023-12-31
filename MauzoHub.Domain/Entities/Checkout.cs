﻿namespace MauzoHub.Domain.Entities
{
    public class Checkout : BaseEntity
    {
        public Guid UserId { get; set; }
        public List<CheckoutItem> Items { get; set; } = new List<CheckoutItem>();
        public Payment Payment { get; set; } = new Payment();
        public ShippingAddress ShippingAddress { get; set; } = new ShippingAddress();
    }
}
