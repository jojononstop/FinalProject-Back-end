using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using RGB.Back.DTOs;
using RGB.Back.Models;

namespace RGB.Back.Service
{
	public class OrdersService
	{
		private readonly RizzContext _context;
		public OrdersService(RizzContext context)
		{
			_context = context;
		}


		//// 取全部
		//public List<Order> GetAll()
		//{
		//	var orders = _context.Orders.AsNoTracking().ToList();

		//	return orders;
		//}



		public async Task<bool> CreateOrder(int memberId, int gameId)
		{
			try
			{
				// 檢查是否存在與會員ID關聯的訂單
				var existingOrder = _context.Orders.FirstOrDefault(i => i.MemberId == memberId);

				// 如果不存在訂單，則創建新訂單
				if (existingOrder == null)
				{
					// 創建新訂單實例
					var newOrder = new Order
					{
						MemberId = memberId,
						GameId = gameId,
						OrderDate = DateOnly.FromDateTime(DateTime.Now)
					};

					// 添加到資料庫
					_context.Orders.Add(newOrder);
					await _context.SaveChangesAsync();

					return true;
				}
				else
				{
					// 如果已經存在訂單，可以選擇更新訂單或拋出異常，這裡簡單地拋出了一個異常
					throw new Exception("該會員已經存在訂單。");
				}
			}
			catch (Exception ex)
			{
				// 處理錯誤
				Console.WriteLine("在創建訂單時發生錯誤：" + ex.Message);
				return false;
			}
		}




        //取得一筆
        public List<Order> GetOrdersByMemberId(int memberId)
        {// 從資料庫中取得指定會員ID的訂單資訊
            var orders = _context.Orders
                                 .Include(o => o.Game)
                                 .Include(o => o.Member)
                                 .Where(o => o.MemberId == memberId)
                                 .Select(o => new Order
                                 {
                                     Id = o.Id,
                                     MemberId = o.MemberId,
                                     GameId = o.GameId,
                                     OrderDate = o.OrderDate,
                                     Member = _context.Members.FirstOrDefault(m => m.Id == o.MemberId),
                                     Game = _context.Games.FirstOrDefault(g => g.Id == o.GameId)
                                 })
                                 .ToList();

            return orders;
        }

        internal Task<bool> UserExistsAsync(int memberId)
		{
			throw new NotImplementedException();
		}
	}
}
