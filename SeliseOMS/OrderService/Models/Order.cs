namespace OrderService.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string Status { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public decimal Total { get; set; }

    }
}
