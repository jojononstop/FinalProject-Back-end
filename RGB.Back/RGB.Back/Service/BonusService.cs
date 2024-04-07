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

        // Get All Bonus Product
        // 取得全部商品
        public async Task<List<BonusDto>> GetAllBonusProductAsync()
        {
            var bonusProducts = await _context.BonusProducts.AsNoTracking().ToListAsync();
            return DBToBonusDto(bonusProducts);
        }

        // Get All Bonus Product Type
        // 取得全部類型
        public async Task<List<BonusDto>> GetAllBonusProductTypeAsync()
        {
            var bonusProductType = await _context.BonusProductTypes.AsNoTracking().ToListAsync();

            return DBToBonusDto(bonusProductType);
        }

        // Get Bonus Product
        // 依據 ID 查詢 BonusProduct
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

        // Get BonusItem By Member
        // 依據會員 ID 查詢會員的 BonusItem
        public async Task<List<MemberBonusItemDto>> GetBonusProductByMemberAsync(int memberId)
        {
            var bonusItem = await _context.BonusItems
                .AsNoTracking()
                .Include(db => db.Bonus)
                .Include(db => db.Bonus.ProductType)
                .Where(x => x.MemberId == memberId)
                .ToListAsync();
            return DBToBonusItemDto(bonusItem);
        }

        // Update MemberBonusItem Using
        public async Task UpdateMemberBonusItemAsync(int memberId, int bonusId, int typeid, bool usingStatus)
        {
            //遍歷所有相同 MemberId 的相同 BonusItem
            var bonusItems = await _context.BonusItems
                .Include(x => x.Bonus)
                .Where(x => x.MemberId == memberId)
                .ToListAsync();
            foreach (var item in bonusItems)
            {
                if (item.Bonus.ProductTypeId == typeid)
                {
                    //將與目標不同的 BonusItem 的 Using 屬性設為 false
                    if (item.BonusId != bonusId)
                    {
                        item.Using = false;
                    }
                    else
                    {
                        item.Using = usingStatus;
                    }
                }
                // 保存變更
                _context.SaveChanges();
            }
        }

        // Get Bonus Product Type
        // 依據類型 ID 查詢類型
        public async Task<List<BonusDto>> GetBonusProductByTypeAsync(int bonusProductTypeId)
        {
            var bonusProducts = await _context.BonusProducts
                .Include(bp => bp.ProductType)
                .AsNoTracking()
                .Where(x => x.ProductTypeId == bonusProductTypeId)
                .ToListAsync();
            return DBToBonusDto(bonusProducts);
        }

        // Get Bonus Product Name
        // 依據名稱查詢商品
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

        // Get Bonus Product Name By MemberId
        public async Task<List<MemberBonusItemDto>> GetBonusProductByMemberIdAsync(int memberId, string bonusProductName)
        {
            if (string.IsNullOrEmpty(bonusProductName))
            {
                return new List<MemberBonusItemDto>();
            }

            var bonusItem = await _context.BonusItems
                .AsNoTracking()
                .Where(x => x.Bonus.Name.ToLower().Contains(bonusProductName.ToLower()) &&  x.MemberId==memberId)
                .Include(bp => bp.Bonus.ProductType)
                .ToListAsync();
            return DBToBonusDto(bonusItem);
        }


        // Get User
        // 搜尋會員
        public async Task<bool> UserExistsAsync(int memberId)
        {
            // 異步搜尋在資料庫中查詢是否存在指定的會員ID
            var user = await _context.Members.FindAsync(memberId);
            return user != null;
        }

        // Add Product To BonusItem
        // 購買 新增商品到資料庫
        public async Task<bool> ProductToBonusItemAsync(int memberId, int productId)
        {
            try
            {
                var bonusItem = new BonusItem
                {
                    MemberId = memberId,
                    BonusId = productId,
                };

                _context.BonusItems.Add(bonusItem);

                var bonusProduct = _context.BonusProducts.Find(productId);
                var member = _context.Members.Find(memberId);
                member.Bonus -= bonusProduct.Price;
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        // DB to BonusDTO by BonusProduct
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

        //DB to BonusDTO by BonusProductType
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
                    ProductTypeId = item.Bonus.ProductType.Id,
                    ProductTypeName = item.Bonus.ProductType.Name,
                    Price = item.Bonus.Price,
                    URL = item.Bonus.Url,
                    Name = item.Bonus.Name,
                    MemberId = item.MemberId,
                    BonusId = item.BonusId,
                    ProductType = item.Bonus.ProductType.Id,
                    Using = item.Using
                };
                bonusItemList.Add(memberBonusItemDto);
            }
            return bonusItemList;
        }

        //DB To UserBonusItemByMemberDto
        private List<MemberBonusItemDto> DBToBonusDto(List<BonusItem> bonusItems)
        {
            var bonusItemByMemberList = new List<MemberBonusItemDto>();

            foreach (var item in bonusItems)
            {
                var MemberBonusItemDto = new MemberBonusItemDto
                {
                    Id = item.Id,
                    ProductTypeId = item.Bonus.ProductType.Id,
                    ProductTypeName = item.Bonus.ProductType.Name,
                    Price = item.Bonus.Price,
                    URL = item.Bonus.Url,
                    Name = item.Bonus.Name,
                    MemberId = item.MemberId,
                    BonusId = item.BonusId,
                    ProductType = item.Bonus.ProductType.Id,
                    Using = item.Using
                };
                bonusItemByMemberList.Add(MemberBonusItemDto);
            }
            return bonusItemByMemberList;
        }
    }
}
