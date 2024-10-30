using AutoMapper;
using RestaurantOrderManagement.Dto;
using RestaurantOrderManagement.Model;

namespace RestaurantOrderManagement.Configuration
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<Order, CreateOrderDto>().ReverseMap();
            CreateMap<Order, UpdateOrderStatusDto>().ReverseMap();
            CreateMap<OrderItem, OrderItemDto>().ReverseMap();
            CreateMap<OrderItem, CreateOrderItemDto>().ReverseMap();
            CreateMap<DailyReportDto, DailyReportDto>().ReverseMap();
            CreateMap<DailyReportItemDto, DailyReportItemDto>().ReverseMap();
        }
    }
}
