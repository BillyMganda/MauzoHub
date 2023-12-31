﻿namespace MauzoHub.Application.DTOs
{
    public class GetProductsDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public List<string> Images { get; set; } = new List<string>();
        public Guid BusinessId { get; set; }
    }
}
