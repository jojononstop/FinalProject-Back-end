using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
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
        // GetAll 取得全部商品
        [HttpGet]
        public async Task<List<BonusProduct>> GetAllBonusProductAsync()
        {
            return await _context.BonusProducts.ToListAsync();
        }

        // GET: api/BonusProductsType
        // GetAllType 取得全部類型
        [HttpGet("Type")]
        public async Task<List<BonusProductType>> GetAllBonusProductTypeAsync()
        {
            return await _context.BonusProductTypes.ToListAsync();
        }

        // GET: api/BonusProducts/5
        // GetById 以Id搜尋商品
        [HttpGet("{id}")]
        public async Task<BonusDto> GetBonusProductAsync(int id)
        {
            return await _service.GetBonusProductAsync(id);
        }

        // GET: api/BonusProducts/MemberId/5
        // GetByMember 以會員搜尋商品
        [HttpGet("MemberId/{memberId}")]
        public async Task<List<MemberBonusItemDto>> GetBonusProductByMemberAsync(int memberId)
        {
            return await _service.GetBonusProductByMemberAsync(memberId);
        }

        // GET: api/BonusProducts/MemberId/5
        // GetByMember 變更會員蒐藏庫使用狀態
        [HttpPost("update")]
        public async Task<IActionResult> UpdateMemberBonusItemAsync(int memberId, int bonusId, int typeid, bool usingStatus)
        {
            try
            {
                await _service.UpdateMemberBonusItemAsync(memberId, bonusId, typeid, usingStatus);
                return Ok("BonusItem updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/BonusProducts/Type/5
        // GetByType 以類型搜尋商品
        [HttpGet("Type/{typeId}")]
        public async Task<List<BonusDto>> GetBonusProductByTypeAsync(int typeId)
        {
            return await _service.GetBonusProductByTypeAsync(typeId);
        }

        // GET: api/BonusProducts/Name/5
        // GetByName 以名稱搜尋商品
        [HttpGet("Name/{name}")]
        public async Task<List<BonusDto>> GetBonusProductByNameAsync(string name)
        {
            return await _service.GetBonusProductByNameAsync(name.ToLower());
        }

        // GET: api/BonusProducts/Name/5/MemberId/5
        // GetByName and GetByMemberId 以名稱及使用者ID搜尋商品
        [HttpGet("Name/{bonusProductName}/MemberId/{memberId}")]
        public async Task<List<MemberBonusItemDto>> GetBonusProductByMemberIdAsync(int memberId, string bonusProductName)
        {
            return await _service.GetBonusProductByMemberIdAsync(memberId, bonusProductName.ToLower());
        }

        // POST: api/BonusProducts/5
        // Add Product To BonusItem 新增商品到資料庫
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

            // 取得商品ll;
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
    }
}
