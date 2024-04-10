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
		//[HttpGet]
		//public async Task<IEnumerable<Order>> GetOrders()
		//{
		//    return _service.GetAll();
		//}

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

			_context.Orders.Add(order);
			await _context.SaveChangesAsync();

			return Ok("訂單已成功建立");
		}

		[HttpGet("GetLastOrderId")]
		public int GetLastOrderId()
		{
			return _context.Orders.OrderBy(x=>x.Id).Select(x => x.Id).LastOrDefault();
		}

	}
}