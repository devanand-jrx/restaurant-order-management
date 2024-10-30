namespace RestaurantOrderManagement.Dto
{
    public class CreateOrderItemDto
    {
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
