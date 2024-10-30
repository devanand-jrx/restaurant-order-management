namespace RestaurantOrderManagement.Dto
{
    public class DailyReportDto
    {
        public DateTime Date { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public ICollection<DailyReportItemDto> ItemsSold { get; set; }
    }
}
