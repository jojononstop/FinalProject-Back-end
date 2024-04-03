//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using RGB.Back.DTOs;
//using RGB.Back.Models;
//using System;

//namespace RGB.Back.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class OrderController : ControllerBase
//    {
//        private readonly RizzContext _context;

//        public OrderController(RizzContext context)
//        {
//            _context = context;
//        }



//        [HttpGet("{memberId}")]
//        public async Task<ActionResult<OdersDTO>> Get(int memberId)
//        {
//            var memberOrder = await _context.Orders.FirstOrDefaultAsync(x => x.MemberId == memberId);

//            OdersDTO ordersDto = null;
//            if (memberOrder != null)
//            {
//                ordersDto = new OdersDTO
//                {
//                    MemberId = memberOrder.MemberId,
//                    OrderDate = memberOrder.OrderDate,
//                    PaymentMethod = memberOrder.PaymentMethod,
//                    TotalAmount = memberOrder.TotalAmount,
//                    DiscountAmount = memberOrder.DiscountAmount,
//                    Status = memberOrder.Status,
//                    Message = memberOrder.Message
//                };
//            }
//            return ordersDto;
//        }



//        // POST api/<OrderApiController>
//        [HttpPost]
//        public async Task<ActionResult<string>> Post([FromBody] OdersDTO ordersDto)
//        {
//            if (ordersDto == null)
//            {
//                return BadRequest("介面出錯請稍後");
//            }

//            try
//            {
//                var existingOrder = await _context.Orders
//                                        .FirstOrDefaultAsync(x => x.MemberId == ordersDto.MemberId);


//                var newOrder = new Order
//                {
//                    MemberId = ordersDto.MemberId,
//                    PaymentMethod = ordersDto.PaymentMethod,
//                    OrderDate = ordersDto.OrderDate,
//                    TotalAmount = ordersDto.TotalAmount,
//                    Status = ordersDto.Status,
//                    DiscountAmount = ordersDto.DiscountAmount,
//                    Message = ordersDto.Message,
//                };
//                _context.Orders.Add(newOrder);
//                await _context.SaveChangesAsync();

//                return Ok("訂單新增成功");

//            }
//            catch (Exception)
//            {
//                return BadRequest("訂單新增失敗");
//            }
//        }


//        // PUT api/<OrderApiController>/5
//        [HttpPut("{id}")]
//        public async Task<ActionResult<string>> Put(int id, Order order)
//        {
//            if (order == null)
//            {
//                return BadRequest("訂單未存在，請使用 POST方法新增訂單");
//            }

//            var existingOrder = await _context.Orders.FindAsync(id);
//            //假設購物車裡面沒有相同商品的話
//            if (existingOrder == null)
//            {
//                return NotFound("找不到對應的訂單");
//            }

//            try
//            {
//                existingOrder.TotalAmount = order.TotalAmount;
//                // 更新折扣金額
//                existingOrder.DiscountAmount = order.DiscountAmount;
//                // 更新備註信息
//                existingOrder.Message = order.Message;

//                _context.Orders.Update(existingOrder);
//                await _context.SaveChangesAsync();

//                return Ok("訂單內容已更新");
//            }
//            catch (Exception)
//            {
//                return BadRequest("訂單更新失敗");
//            }
//        }

//    }
//}
