using RGB.Back.Models;

namespace RGB.Back.DTOs
{
    public class CartItemDTO
    {
        public int Id { get; set; }
        public Game Game { get; set; }
        public int Qty { get; set; }
        public decimal Total => Game.Price * Qty;
    }
}
