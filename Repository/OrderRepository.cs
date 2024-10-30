using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using RestaurantOrderManagement.Contracts;
using RestaurantOrderManagement.Data;
using RestaurantOrderManagement.Dto;
using RestaurantOrderManagement.Exceptions;
using RestaurantOrderManagement.Model;

namespace RestaurantOrderManagement.Repository
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly OrderManagementDbContext _context;
        private readonly IMapper _mapper;
        public OrderRepository(OrderManagementDbContext context, IMapper mapper) : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<OrderDto> GetDetails(int id)
        {
            var order = await _context.Orders.Include(o => o.Items)
                .ProjectTo<OrderDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(o => o.Id == id);
            if (order == null)
            {
                throw new NotFoundException(nameof(GetDetails), id);
            }
            return order;
        }
    }
}
