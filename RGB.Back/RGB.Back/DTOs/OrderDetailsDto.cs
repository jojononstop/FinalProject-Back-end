using RGB.Back.ViewModels;

namespace RGB.Back.DTOs
{
    public class OrderDetailsDto
    {
        public int OrderId { get; set; }

        public int GameId { get; set; }
      
        public int Qty { get; set; }
        public int UnitPrice { get; set; }

        public int Price { get; set; }

        internal OrderDetailVm OrderDetailsVm()
        {
            throw new NotImplementedException();
        }
    }
}
