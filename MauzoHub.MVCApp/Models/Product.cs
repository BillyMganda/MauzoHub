namespace MauzoHub.MVCApp.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid BusinessId { get; set; }
    }
}
