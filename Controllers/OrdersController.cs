using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantOrderManagement.Contracts;
using RestaurantOrderManagement.Dto;
using RestaurantOrderManagement.Enum;
using RestaurantOrderManagement.Model;

namespace RestaurantOrderManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public OrdersController(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        // GET: api/orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders([FromQuery] OrderStatus? status,
            [FromQuery] int? tableNumber, [FromQuery] DateTime? date)
        {
            var orders = await _orderRepository.GetAllAsync<OrderDto>();
            if (status.HasValue)
                orders = orders.Where(o => o.Status == status.Value).ToList();
            if (tableNumber.HasValue)
                orders = orders.Where(o => o.TableNumber == tableNumber.Value).ToList();
            if (date.HasValue)
                orders = orders.Where(o => o.OrderTime.Date == date.Value.Date).ToList();
            return Ok(new { totalOrders = orders.Count, orders });
        }

        // GET: api/orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetOrder(int id)
        {
            var order = await _orderRepository.GetDetails(id);
            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        // POST: api/orders
        [HttpPost]
        public async Task<ActionResult<OrderDto>> PostOrder(CreateOrderDto createOrderDto)
        {
            if (createOrderDto.TableNumber <= 0 || createOrderDto.Items == null || !createOrderDto.Items.Any())
            {
                return BadRequest("Invalid order details.");
            }

            var order = _mapper.Map<Order>(createOrderDto);
            order.Status = OrderStatus.New;
            order.OrderTime = DateTime.UtcNow;
            order.TotalAmount = order.Items.Sum(i => i.Quantity * i.UnitPrice) * 1.1m; // Including 10% tax
            var createdOrder = await _orderRepository.AddAsync<Order, OrderDto>(order);
            return CreatedAtAction(nameof(GetOrder), new { id = createdOrder.Id }, createdOrder);
        }

        // PUT: api/orders/{id}/status
        [HttpPut("{id}/status")]
        public async Task<IActionResult> PutOrderStatus(int id, UpdateOrderStatusDto updateOrderStatusDto)
        {
            var order = await _orderRepository.GetAsync(id);
            if (order == null)
            {
                return NotFound("Order not found.");
            }

            // Business rules for status update
            if (order.Status == OrderStatus.Delivered)
            {
                return BadRequest("Cannot update delivered orders.");
            }

            if (updateOrderStatusDto.Status == OrderStatus.Preparing && order.Status != OrderStatus.New)
            {
                return BadRequest("Status changes must be sequential.");
            }

            order.Status = updateOrderStatusDto.Status;
            await _orderRepository.UpdateAsync(order);
            return NoContent();
        }

        // DELETE: api/orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _orderRepository.GetAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            await _orderRepository.DeleteAsync(id);
            return NoContent();
        }

        // GET: api/orders/daily-report
        [HttpGet("daily-report")]
        public async Task<ActionResult<DailyReportDto>> GetDailyReport([FromQuery] DateTime date)
        {
            var orders = await _orderRepository.GetAllAsync<Order>();
            var dailyOrders = orders.Where(o => o.OrderTime.Date == date.Date).ToList();
            var totalRevenue = dailyOrders.Sum(o => o.TotalAmount);
            var itemsSold = dailyOrders.SelectMany(o => o.Items)
                .GroupBy(i => i.ItemName)
                .Select(g => new DailyReportItemDto
                {
                    ItemName = g.Key,
                    Quantity = g.Sum(i => i.Quantity),
                    Revenue = g.Sum(i => i.Quantity * i.UnitPrice)
                }).ToList();
            var report = new DailyReportDto
            {
                Date = date,
                TotalOrders = dailyOrders.Count,
                TotalRevenue = totalRevenue,
                ItemsSold = itemsSold
            };
            return Ok(report);
        }

    }
}
