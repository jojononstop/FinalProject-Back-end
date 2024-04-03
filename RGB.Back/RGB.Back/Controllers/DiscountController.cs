using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RGB.Back.DTOs;
using RGB.Back.Models;
using RGB.Back.Service;

namespace RGB.Back.Controllers
{
    [Route("api/[controller]")]
    public class DiscountController : Controller
    {


        private readonly RizzContext _context;

        public DiscountController(RizzContext context)
        {
     
            _context = context;
        }

        [HttpGet("GetDiscount", Name = "GetDiscount")]
        public List<DiscountDto> GetDiscounts()
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

        [HttpGet("GetDiscountItem", Name = "GetDiscountItem")]
        public DiscountDto GetDiscountItem(int id)
        {
            var discount = _context.Discounts
                .Where(d => d.Id == id)
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
                }).FirstOrDefault();

            return discount;
        }


    }
}
