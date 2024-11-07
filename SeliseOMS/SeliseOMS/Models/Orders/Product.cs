namespace SeliseOMS.Models.Orders
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal UnitPrice { get; set; }
        public int AvailableQuantity { get; set; }

    }
}
