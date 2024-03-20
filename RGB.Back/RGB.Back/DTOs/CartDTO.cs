using RGB.Back.Models;

namespace RGB.Back.DTOs
{
    public class CartDTO
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public decimal Total => Items.Sum(i => i.Total);
    }
}
