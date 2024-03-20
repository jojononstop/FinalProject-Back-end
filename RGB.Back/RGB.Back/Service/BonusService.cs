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

        //Get All Bonus Product
        public async Task<List<BonusDto>> GetAllBonusProductAsync()
        {
            var bonusProducts = await _context.BonusProducts.AsNoTracking().ToListAsync();
            return DBToBonusDto(bonusProducts);
        }

        //Get All Bonus Product Type
        public async Task<List<BonusDto>> GetAllBonusProductTypeAsync()
        {
            var bonusProductType = await _context.BonusProductTypes.AsNoTracking().ToListAsync();

            return DBToBonusDto(bonusProductType);
        }

        //Get Member BonusItem
        public async Task<List<MemberBonusItemDto>> GetMemberBonusItemAsync(int memberId)
        {
            var bonusItems = await _context.BonusItems
                .AsNoTracking()
                .Where(x => x.MemberId == memberId)
                .ToListAsync();
            return DBToBonusItemDto(bonusItems);
        }

        //Get Bonus Product
        public async Task<BonusDto> GetBonusProductAsync(int id)
        {
            var bonusProduct = await _context.BonusProducts
                .AsNoTracking()
                .Include(bp => bp.ProductType)
                .Where(bp => bp.Id == id)
                .FirstOrDefaultAsync();

            var bonusDto = new BonusDto
            {
                Id = bonusProduct.Id,
                ProductTypeId = bonusProduct.ProductTypeId,
                ProductTypeName = bonusProduct.ProductType.Name,
                Price = bonusProduct.Price,
                URL = bonusProduct.Url,
                Name = bonusProduct.Name
            };
            return bonusDto;
        }

        // Get MemberBonusItem
        public async Task<List<MemberBonusItemDto>> GetBonusProductByMemberAsync(int memberId)
        {
            var bonusItem = await _context.BonusItems
                .AsNoTracking()
                .Where(x => x.MemberId == memberId)
                .ToListAsync();
            return DBToBonusItemDto(bonusItem);
        }


        //Get Bonus Product Type
        public async Task<List<BonusDto>> GetBonusProductByTypeAsync(int bonusProductTypeId)
        {
            var bonusProducts = await _context.BonusProducts
                .Include(bp => bp.ProductType) // 加載 ProductType 導航屬性
                .AsNoTracking()
                .Where(x => x.ProductTypeId == bonusProductTypeId)
                .ToListAsync();
            return DBToBonusDto(bonusProducts);
        }

        //Get Bonus Product Name
        public async Task<List<BonusDto>> GetBonusProductByNameAsync(string bonusProductName)
        {
            if (string.IsNullOrEmpty(bonusProductName))
            {
                // 條件為空，返回空列表
                return new List<BonusDto>();
            }

            var bonusProducts = await _context.BonusProducts
                .AsNoTracking()
                .Include(bp => bp.ProductType)
                .Where(x => x.Name.ToLower().Contains(bonusProductName.ToLower()))
                .ToListAsync();
            return DBToBonusDto(bonusProducts);
        }

        //Find User Funtion
        public async Task<bool> UserExistsAsync(int memberId)
        {
            // 異步搜尋在資料庫中查詢是否存在指定的會員ID
            var user = await _context.Members.FindAsync(memberId);
            return user != null;
        }

        //Add Product To BonusItem - Bonus Product To UserItem
        public async Task<bool> ProductToBonusItemAsync(int memberId, int productId)
        {
            try
            {
                // 創建一個新的Bonus Item
                var bonusItem = new BonusItem
                {
                    MemberId = memberId,
                    BonusId = productId,
                    // 可能還有其他相關屬性的設定，例如時間戳記等等
                };

                // 將Bonus Item添加到資料庫
                _context.BonusItems.Add(bonusItem);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                // 處理錯誤
                return false;
            }
        }

        //DB to BonusDTO
        private List<BonusDto> DBToBonusDto(List<BonusProduct> bonusProduct)
        {
            var bonusList = new List<BonusDto>();

            foreach (var item in bonusProduct)
            {
                var bonusDto = new BonusDto
                {
                    Id = item.Id,
                    ProductTypeId = item.ProductTypeId,
                    ProductTypeName = item.ProductType.Name,
                    Price = item.Price,
                    URL = item.Url,
                    Name = item.Name
                };
                bonusList.Add(bonusDto);
            }
            return bonusList;
        }

        //DB to BonusDTO
        private List<BonusDto> DBToBonusDto(List<BonusProductType> bonusProductType)
        {
            var bonusList = new List<BonusDto>();

            foreach (var item in bonusProductType)
            {
                var bonusDto = new BonusDto
                {
                    Id = item.Id,
                    ProductTypeName = item.Name
                };
                bonusList.Add(bonusDto);
            }
            return bonusList;
        }

        //DB To UserBonusItemDto
        private List<MemberBonusItemDto> DBToBonusItemDto(List<BonusItem> bonusItems)
        {
            var bonusItemList = new List<MemberBonusItemDto>();

            foreach (var item in bonusItems)
            {
                var memberBonusItemDto = new MemberBonusItemDto
                {
                    Id = item.Id,
                    MemberId = item.MemberId,
                    BonusId = item.BonusId
                };
                bonusItemList.Add(memberBonusItemDto);
            }
            return bonusItemList;
        }
    }
}
