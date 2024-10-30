namespace RestaurantOrderManagement.Dto
{
    public class CreateOrderDto
    {
        public int TableNumber { get; set; }
        public ICollection<CreateOrderItemDto> Items { get; set; }
    }
}
