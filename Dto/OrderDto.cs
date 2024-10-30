using RestaurantOrderManagement.Enum;

namespace RestaurantOrderManagement.Dto
{
    public class OrderDto
    {
        public int Id { get; set; }
        public int TableNumber { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime OrderTime { get; set; }
        public decimal TotalAmount { get; set; }
        public ICollection<OrderItemDto> Items { get; set; }
    }
}
