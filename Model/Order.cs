using RestaurantOrderManagement.Enum;

namespace RestaurantOrderManagement.Model
{
    public class Order
    {
        public int Id { get; set; }
        public int TableNumber { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime OrderTime { get; set; }
        public decimal TotalAmount { get; set; }
        public ICollection<OrderItem> Items { get; set; }
    }
}
