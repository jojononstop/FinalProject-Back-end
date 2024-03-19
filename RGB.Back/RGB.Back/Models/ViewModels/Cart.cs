namespace RGB.Back.Models.ViewModels
{
    public class Cart
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public decimal Total => Items.Sum(i => i.Total);
    }
}
