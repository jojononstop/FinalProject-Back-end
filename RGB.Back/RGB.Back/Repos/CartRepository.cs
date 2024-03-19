using RGB.Back.Models.ViewModels;

namespace RGB.Back.Repos
{
    public class CartRepository
    {
    
            private static Cart _cart = new Cart();

            /// <summary> 
            /// 增減購買商品數量,若商品不存在則新增, 若數量<=0則移除 
            /// </summary> 
            /// <param name="game"></param> 
            /// <param name="qty"></param> 
            public Cart TryAddToCart(Game game, int qty = 1)
            {
                var item = _cart.Items.FirstOrDefault(i => i.Game.Id == game.Id);
                if (qty <= 0)
                {
                    RemoveFromCart(item.Id);
                    return _cart;
                }
                if (item == null)
                {
                    _cart.Items.Add(new CartItem
                    {
                        Id = _cart.Items.Count == 0 ? 1 : _cart.Items.Max(i => i.Id) + 1,
                       Game = game,
                        Qty = qty
                    });
                }
                else
                {
                    item.Qty += qty;
                    // 如果更新後數量<=0,則移除 
                    if (item.Qty <= 0)
                    {
                        RemoveFromCart(item.Id);
                    }
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
            public Cart GetCart()
            {
                return _cart;
            }
    }
}
