namespace RGB.Back.Models.ViewModels
{
    public class CartItem
    {
        public int Id { get; set; }
        public Game Game { get; set; }
        public int Qty { get; set; }
        public decimal Total => Game.Price * Qty;
    }
}
