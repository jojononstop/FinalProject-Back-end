using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RGB.Back.DTOs;
using RGB.Back.Models;
using RGB.Back.Service;

namespace RGB.Back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BonusProductsController : ControllerBase
    {
        private readonly RizzContext _context;
        private readonly BonusService _service;

        public BonusProductsController(RizzContext context)
        {
            _context = context;
            _service = new BonusService(context);
        }

        // GET: api/BonusProducts
        //GetAll
        [HttpGet]
        public async Task<List<BonusProduct>> GetAllBonusProductAsync()
        {
            return await _context.BonusProducts.ToListAsync();
        }

        //GET: api/BonusProductsType
        //GetAllType
        [HttpGet("Type")]
        public async Task<List<BonusProductType>> GetAllBonusProductTypeAsync()
        {
            return await _context.BonusProductTypes.ToListAsync();
        }

        // GET: api/BonusProducts/5
        // GetById
        [HttpGet("{id}")]
        public async Task<BonusDto> GetBonusProductAsync(int id)
        {
            return await _service.GetBonusProductAsync(id);
        }

        // GET: api/BonusProducts/MemberId/5
        //GetByMember
        [HttpGet("MemberId/{memberId}")]
        public async Task<List<MemberBonusItemDto>> GetBonusProductByMemberAsync(int memberId)
        {
            return await _service.GetBonusProductByMemberAsync(memberId);
        }

        //GET: api/BonusProducts/Type/5
        //GetByType
        [HttpGet("Type/{typeId}")]
        public async Task<List<BonusDto>> GetBonusProductByTypeAsync(int typeId)
        {
            return await _service.GetBonusProductByTypeAsync(typeId);
        }

        //GET: api/BonusProducts/Name/5
        //GetByName
        [HttpGet("Name/{name}")]
        public async Task<List<BonusDto>> GetBonusProductByNameAsync(string name)
        {
            return await _service.GetBonusProductByNameAsync(name.ToLower());
        }

        // POST: api/BonusProducts/5
        // Add Product To BonusItem - PostBonusProductToBonusItem
        [HttpPost("{id}")]
        public async Task<IActionResult> PostBonusProductToBonusItem(int id, int memberId)
        {
            //檢查會員是否存在
            var userExists = await _service.UserExistsAsync(memberId);
            if (!userExists)
            {
                // 如果會員不存在，返回404 Not Found或其他適當的錯誤
                return NotFound("會員不存在");
            }

            // 取得商品
            var bonusProduct = await _context.BonusProducts.FindAsync(id);
            if (bonusProduct == null)
            {
                // 如果商品不存在
                return NotFound("未找到商品");
            }

            // 調用BonusService中的方法，將會員ID和商品ID發送到Bonus Item
            var success = await _service.ProductToBonusItemAsync(memberId, id);
            if (!success)
            {
                // 如果發送失敗，返回500
                return StatusCode(500, "無法傳送到正確的收藏庫");
            }

            // 返回200 OK
            return Ok(bonusProduct.Id);
        }

        #region 使用不到的PUT與POST
        // PUT: api/BonusProducts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutBonusProduct(int id, BonusProduct bonusProduct)
        //{
        //    if (id != bonusProduct.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(bonusProduct).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!BonusProductExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/BonusProducts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<BonusProduct>> PostBonusProduct(BonusProduct bonusProduct)
        //{
        //    _context.BonusProducts.Add(bonusProduct);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetBonusProduct", new { id = bonusProduct.Id }, bonusProduct);
        //}

        // DELETE: api/BonusProducts/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteBonusProduct(int id)
        //{
        //    var bonusProduct = await _context.BonusProducts.FindAsync(id);
        //    if (bonusProduct == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.BonusProducts.Remove(bonusProduct);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}
        //private bool BonusProductExists(int id)
        //{
        //    return _context.BonusProducts.Any(e => e.Id == id);
        //}
        #endregion
    }
}
