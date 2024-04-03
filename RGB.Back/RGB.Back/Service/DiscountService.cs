using RGB.Back.DTOs;
using RGB.Back.Models;

namespace RGB.Back.Service
{
    public class DiscountService
    {
        private readonly RizzContext _context;

        public DiscountService(RizzContext context)
        {
            _context = context;
        }
        

        public List<DiscountDto> GetAllDiscount()
        {
           
            var discountProducts = _context.Discounts                                           
                .Select(d => new DiscountDto
                {
                    Id = d.Id,
                    StartTime = d.StartDate,
                    EndTime = d.EndDate,
                    Percent = d.Percent,
                    DiscountType = d.DiscountType,
                    DiscountName = d.Name,
                    DiscountDescription = d.Desciption,
                    Image = d.Image
                }).ToList();

            return discountProducts;
        }



    }
}
