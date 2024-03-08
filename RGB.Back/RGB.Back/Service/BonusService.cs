using Microsoft.EntityFrameworkCore;
using RGB.Back.DTOs;
using RGB.Back.Models;

namespace RGB.Back.Service
{
    //基本都做了異步處理
    public class BonusService
    {
        private readonly RizzContext _context;
        public BonusService(RizzContext context)
        {
            // 空值合併運算式
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        //Get all bonus product
        public async Task<List<BonusDto>> GetAllBonusProductAsync()
        {
            var bonusProducts = await _context.BonusProducts.AsNoTracking().ToListAsync();
            return DBtoBonusDto(bonusProducts);
        }


        //Get bonus product type
        public async Task<List<BonusDto>> GetBonusProductByTypeAsync(int bonusProductTypeId)
        {
            var bonusProducts = await _context.BonusProducts.AsNoTracking()
                .Where(x => x.ProductTypeId == bonusProductTypeId)
                .ToListAsync();
            return DBtoBonusDto(bonusProducts);
        }

        //DB to DTO
        private List<BonusDto> DBtoBonusDto(List<BonusProduct> bonusProduct)
        {
            var bonusList = new List<BonusDto>();

            foreach (var item in bonusProduct)
            {
                var bonusDto = new BonusDto 
                {
                    Id = item.Id,
                    ProductTypeId = item.ProductTypeId,
                    ProductTypeName = item.ProductType.Name,
                    URL = item.Url,
                    Name = item.Name
                };
                bonusList.Add(bonusDto);
            }

            return bonusList;
        }

        //search bonus product
        public async Task<List<BonusDto>> SearchBonusProductAsync(string search)
        {
            if (string.IsNullOrEmpty(search))
            {
                // 條件為空，返回空列表
                return new List<BonusDto>();
            }

            var bonusProducts = await _context.BonusProducts.AsNoTracking()
                .Where(x => x.Name.ToLower().Contains(search.ToLower()))
                .ToListAsync();

            return DBtoBonusDto(bonusProducts);
        }
    }
}
