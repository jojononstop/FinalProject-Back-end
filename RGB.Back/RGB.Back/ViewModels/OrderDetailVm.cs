using System.ComponentModel.DataAnnotations;

namespace RGB.Back.ViewModels
{
    public class OrderDetailVm
    {
        [Display(Name = "訂單編號")]
        public int OrderId { get; set; }

        [Display(Name = "商品")]
        public string GameName { get; set; }
        [Display(Name = "數量")]
        public int Qty { get; set; }
        [Display(Name = "單價")]
        public decimal UnitPrice { get; set; }

    }
}
