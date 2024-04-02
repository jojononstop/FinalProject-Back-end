using Microsoft.AspNetCore.Mvc;
using RGB.Back.DTOs;
using RGB.Back.Models;
using RGB.Back.Service;

namespace RGB.Back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly RizzContext _context;
        private readonly OrdersService _service;

        public OrderController(RizzContext context)
        {
            _context = context;
            _service = new OrdersService(context);
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<IEnumerable<Order>> GetOrders()
        {
            return _service.GetAll();
        }

        // GET: api/Orders/member/5
        [HttpGet("member/{memberId}")]
        public async Task<IEnumerable<Order>> GetOrdersByMemberId(int memberId)
        {
            var orders = _service.GetOrdersByMemberId(memberId);
            return orders;
        }

        // POST: api/Orders
        [HttpPost]
        public async Task<IActionResult> PostOrder(Order order)
        {
            // 檢查訂單 ID 是否已存在
            if (_context.Orders.Any(o => o.Id == order.Id))
            {
                return BadRequest("訂單 ID 已存在");
            }

            // 檢查遊戲 ID 是否已存在
            if (_context.Orders.Any(o => o.GameId == order.GameId))
            {
                return BadRequest("遊戲 ID 已存在");
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return Ok("訂單已成功建立");
        }

    }

}
