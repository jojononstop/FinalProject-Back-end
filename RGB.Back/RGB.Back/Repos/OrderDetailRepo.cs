using Microsoft.EntityFrameworkCore;
using RGB.Back.DTOs;
using RGB.Back.Interfaces;
using RGB.Back.Models;

namespace RGB.Back.Repos
{
    public class OrderDetailRepo : IOderDetailRepo
    {
        private readonly RizzContext _context;
        public OrderDetailsDto Get(int id)
        {
            var model = _context.OderDetails.FirstOrDefault(x => x.OrderId == id);
            if (model == null)
            {
                return null;
            }
            var dto = new OrderDetailsDto
            {
                OrderId = model.OrderId,
                GameId = model.GameId,
                Qty = model.Qty,
                UnitPrice = model.UnitPrice,
            };
            return dto;
        }

        public List<OrderDetailsDto> Search(int OrderId)
        {
            return _context.OderDetails
               .Include(x => x.GameId)
                .Where(x => x.OrderId == OrderId)
                                .Select(x => new OrderDetailsDto()
                                {
                                    OrderId = x.OrderId,
                                   GameId = x.GameId,
                                    Qty = x.Qty,
                                    UnitPrice = x.UnitPrice,

                                }).ToList();
        }
    }
}
