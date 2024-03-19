using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RGB.Back.Models;
using RGB.Back.Models.ViewModels;
using RGB.Back.Repo;
using RGB.Back.Repos;
using RGB.Back.Service;

namespace RGB.Back.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly RizzContext _context;
       

        public CartController(RizzContext context)
        {
            _context = context;
        }
        public class RequestInfo
        {
            public int GameId { get; set; }
            public int Quantity { get; set; } = 1;
        }

        // POST api/<controller>
        [HttpPost]
        public IActionResult AddToCart(RequestInfo model, [FromServices] GameRepository productRepository, [FromServices] CartRepository cartRepository)
        {
            var productId = model.GameId;
            var quantity = model.Quantity;

            var product = productRepository.Get(productId);
            if (product == null)
            {
                return NotFound();
            }

            var cart = cartRepository.TryAddToCart(product, quantity);
            return Ok(cart);
        }

        // GET api/<controller>
        [HttpGet]
        public IActionResult GetCart([FromServices] CartRepository cartRepository)
        {
            var cart = cartRepository.GetCart();
            return Ok(cart);
        }

        // DELETE: 
        [HttpDelete("{id}")]
        public async Task<string> DeleteCart(int id)
        {
            var cart= await _context.Carts.FindAsync(id);
            if (cart == null)
            {
                return "刪除失敗";
            }

            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();

            return "刪除成功";
        }


    }

}
