//using RGB.Back.Models;
//using RGB.Back.Service;

//namespace RGB.Back.Controllers
//{
//    using Microsoft.AspNetCore.Mvc;
//    using System.Threading.Tasks;

//    namespace CartControllers
//    {
//        [ApiController]
//        [Route("api/[controller]")]
//        public class CartController : ControllerBase
//        {
//            private readonly RizzContext _context;
//            private readonly GameService _gameService; // 添加GameService

//            public CartController(RizzContext context, GameService gameService) // 修改參數類型
//            {
//                _context = context;
//                _gameService = gameService; // 注入GameService
//            }

            // 購物車中商品的請求資訊模型
            public class RequestInfo
            {
                public int GameId { get; set; } // 遊戲 ID


                public int Quantity { get; set; } = 1; // 數量，默认為 1

//                public int CartId { get; set; } // 購物車 ID

//                public decimal Price { get; set; } // 商品價格
//            }

//            // 將商品添加到購物車
//            [HttpPost]
//            public IActionResult AddToCart(RequestInfo model, [FromServices] CartService cartService)
//            {
//                var gameId = model.GameId;
//                var quantity = model.Quantity;

//                var gameDetailDTO = _gameService.GetGameDetailByGameId(gameId); // 使用GameService中的方法
//                if (gameDetailDTO == null)
//                {
//                    return NotFound("找不到指定的遊戲。"); // 返回 404 錯誤
//                }

//                var cart = cartService.TryAddToCart(gameDetailDTO, quantity); // 嘗試將商品添加到購物車
//                return Ok(cart); // 返回成功添加商品的購物車內容
//            }

//            // 獲取購物車內容
//            [HttpGet]
//            public IActionResult GetCart([FromServices] CartService cartService)
//            {
//                var cart = cartService.GetCart(); // 獲取購物車內容
//                return Ok(cart); // 返回購物車內容
//            }

//            [HttpGet("{Id}")]
//            public IActionResult GetCartById(int Id, [FromServices] CartService cartService)
//            {
//                var cart = cartService.GetCartById(Id); // 根據指定的 Id 獲取購物車內容

//                // 假設 cart 為空表示找不到指定 Id 的購物車內容
//                if (cart == null)
//                {
//                    return NotFound(); // 返回 404 Not Found 錯誤
//                }

//                return Ok(cart); // 返回購物車內容
//            }



//            // 刪除購物車中的指定項目
//            [HttpDelete("{id}")]
//            public async Task<IActionResult> DeleteCartItem(int id)
//            {
//                var cartItem = await _context.Carts.FindAsync(id); // 根據 ID 查找購物車項目
//                if (cartItem == null)
//                {
//                    return NotFound("找不到指定的購物車項目。"); // 返回 404 錯誤
//                }

//                _context.Carts.Remove(cartItem); // 從資料庫中移除購物車項目
//                await _context.SaveChangesAsync(); // 確認更改

//                return Ok("成功從購物車中刪除項目。"); // 返回成功刪除消息
//            }
//        }
//    }
//}
