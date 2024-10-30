using RestaurantOrderManagement.Dto;
using RestaurantOrderManagement.Model;

namespace RestaurantOrderManagement.Contracts
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<OrderDto> GetDetails(int id);
    }
}
