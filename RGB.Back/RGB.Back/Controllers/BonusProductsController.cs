using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RGB.Back.Models;

namespace RGB.Back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BonusProductsController : ControllerBase
    {
        private readonly RizzContext _context;

        public BonusProductsController(RizzContext context)
        {
            _context = context;
        }

        // GET: api/BonusProducts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BonusProduct>>> GetBonusProducts()
        {
            return await _context.BonusProducts.ToListAsync();
        }

        // GET: api/BonusProducts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BonusProduct>> GetBonusProduct(int id)
        {
            var bonusProduct = await _context.BonusProducts.FindAsync(id);

            if (bonusProduct == null)
            {
                return NotFound();
            }

            return bonusProduct;
        }

        // PUT: api/BonusProducts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBonusProduct(int id, BonusProduct bonusProduct)
        {
            if (id != bonusProduct.Id)
            {
                return BadRequest();
            }

            _context.Entry(bonusProduct).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BonusProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/BonusProducts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BonusProduct>> PostBonusProduct(BonusProduct bonusProduct)
        {
            _context.BonusProducts.Add(bonusProduct);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBonusProduct", new { id = bonusProduct.Id }, bonusProduct);
        }

        // DELETE: api/BonusProducts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBonusProduct(int id)
        {
            var bonusProduct = await _context.BonusProducts.FindAsync(id);
            if (bonusProduct == null)
            {
                return NotFound();
            }

            _context.BonusProducts.Remove(bonusProduct);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BonusProductExists(int id)
        {
            return _context.BonusProducts.Any(e => e.Id == id);
        }
    }
}
