using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RGB.Back.DTOs;
using RGB.Back.Models;
using System;

namespace RGB.Back.Controllers
{
    public class OrderDetailController : Controller
    {
        [Route("api/[controller]")]
        [ApiController]
        public class OrdersDetailsController : ControllerBase
        {
            private readonly RizzContext _context;

            public OrdersDetailsController(RizzContext context)
            {
                _context = context;
            }


            // GET: api/<OrdersDetailsController>
            [HttpGet("{Id}")]
            public async Task<ActionResult<IEnumerable<OrderDetailsDto>>> Get(int Id)
            {
                var orderDetailsDto = await _context.Orders.Where(x => x.Id == Id).FirstOrDefaultAsync();

                var orderDetails = await _context.OderDetails
                                                .Include(x => x.GameId)
                                                .Where(x => x.OrderId == orderDetailsDto.Id)
                                                .Select(x => new OrderDetailsDto
                                                {

                                                    OrderId = x.OrderId,
                                                    GameId = x.GameId,
                                                   
                                                    Qty = x.Qty,
                                                    UnitPrice = x.UnitPrice,
                                                })
                                                .ToListAsync();

                return orderDetails;
            }



            // POST api/<OrdersDetailsController>
            [HttpPost]
            public async Task<ActionResult<string>> Post(OrderDetailsDto orderDetailsDto)
            {
                // 判斷是否購物車細節為空，若是則新增一個商品細節
                if (orderDetailsDto == null)
                {
                    return BadRequest("無效的資料");
                }

                try
                {
                    var existingOrderDetail = await _context.OderDetails
                                            .FirstOrDefaultAsync(x => x.GameId == orderDetailsDto.GameId && x.OrderId == orderDetailsDto.OrderId);


                    if (existingOrderDetail == null)
                    {
                        var newOrderDetail = new OderDetail
                        {
                            OrderId = orderDetailsDto.OrderId,
                            GameId = orderDetailsDto.GameId,
                            Qty = orderDetailsDto.Qty,
                            UnitPrice = orderDetailsDto.UnitPrice,
                        };
                        _context.OderDetails.Add(newOrderDetail);
                        await _context.SaveChangesAsync();

                        return Ok("訂單細項新增成功");
                    }
                    else
                    {
                        //沒有空的要更新數量
                        existingOrderDetail.Qty = orderDetailsDto.Qty;

                        existingOrderDetail.OrderId = orderDetailsDto.OrderId;

                        existingOrderDetail.GameId = orderDetailsDto.GameId;
                        await _context.SaveChangesAsync();
                        return Ok("數量更新成功");
                    }
                }
                catch (Exception)
                {
                    return "新增失敗";
                }
            }

            // PUT api/<OrdersDetailsController>/5
            [HttpPut("{id}")]
            public async Task<ActionResult<string>> Put(int id, OderDetail orderDetail)
            {
                if (id != orderDetail.Id)
                {
                    return "更新失敗";
                };
                try
                {
                    var existingOrderDetail = await _context.OderDetails.FindAsync(id);
                    //假設購物車裡面沒有想同商品的話
                    if (existingOrderDetail == null)
                    {
                        return NotFound("購物車商品未存在，請使用 POST方法新增購物車");
                    }


                    //更新數量
                    existingOrderDetail.Qty = orderDetail.Qty;

                    await _context.SaveChangesAsync();
                    return Ok("數量更新成功");
                }
                catch (Exception)
                {
                    return ("失敗");
                }

            }

        }
    }
}
