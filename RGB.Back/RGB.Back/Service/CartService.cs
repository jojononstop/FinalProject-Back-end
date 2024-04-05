using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using RGB.Back.DTOs;


namespace RGB.Back.Service
{
    public class CartService
    {
        private static CartDTO _cart = new CartDTO();

        ///// <summary> 
        ///// 增減購買商品數量,若商品不存在則新增, 若數量<=0則移除 
        ///// </summary> 
        ///// <param name="game"></param> 
        ///// <param name="qty"></param> 
        //public CartDTO TryAddToCart(Models.Game game, int qty = 1)
        //{
        //    var item = _cart.Items.FirstOrDefault(i => i.GameId == game.Id);
        //    if (qty <= 0)
        //    {
        //        RemoveFromCart(item.Id);
        //        return _cart;
        //    }
        //    if (item == null)
        //    {
        //        _cart.Items.Add(new Models.CartItem
        //        {
        //            Id = _cart.Items.Count == 0 ? 1 : _cart.Items.Max(i => i.Id) + 1,
        //            GameId = game.Id,
        //            Qty = qty
        //        });
        //    }
        //    else
        //    {
        //        item.Qty += qty;
        //        // 如果更新後數量<=0,則移除 
        //        if (item.Qty <= 0)
        //        {
        //            RemoveFromCart(item.Id);
        //        }
        //    }
        //    return _cart;
        //}

        /// <summary>
        /// 增減購買商品數量,若商品不存在則新增, 若數量<=0則移除 
        /// </summary>
        /// <param name="gameDetailDTO"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public CartDTO TryAddToCart(int gameId, int quantity)
        {
            var item = _cart.Items.FirstOrDefault(i => i.GameId == gameId);
            //if (quantity <= 0)
            //{
            //    RemoveFromCart(item.Id);
            //    return _cart;
            //}
            if (item == null)
            {
                _cart.Items.Add(new Models.CartItem
                {
                    Id = _cart.Items.Count == 0 ? 1 : _cart.Items.Max(i => i.Id) + 1,
                    GameId = gameId,
                    //Qty = quantity
                });
            }
            else
            {
                //item.Qty += quantity;
                //// 如果更新後數量<=0,則移除 
                //if (item.Qty <= 0)
                //{
                //    RemoveFromCart(item.Id);
                //}
            }
            return _cart;
        }

        public void RemoveFromCart(int id)
        {
            var item = _cart.Items.FirstOrDefault(i => i.Id == id);
            if (item != null)
            {
                _cart.Items.Remove(item);
			}
        }

        //public CartDTO GetCart()
        //{
        //    return _cart;
        //}

        public CartDTO GetCartById(int Id)
        {
            var cartDto = new CartDTO();

            // 假設在 _cart 中存在具有指定 Id 的購物車資料
            var cartItem = _cart.Items.FirstOrDefault(item => item.Id == Id);

            cartDto.Id = Id;
            cartDto.memberId = Id;
     


            // 將找到的購物車資料添加到 cartDto 中，如果找不到則維持 cartDto 為空
            if (cartItem != null)
            {
                cartDto.Items.Add(cartItem);
            }

            return cartDto;
        }

    }
}

