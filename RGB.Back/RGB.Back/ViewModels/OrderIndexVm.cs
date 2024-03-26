using System.ComponentModel.DataAnnotations;

namespace RGB.Back.ViewModels
{
    public class OrderIndexVm
    {
        public int Id { get; set; }

        [Display(Name = "客戶名稱")]
        public string MemberId { get; set; }

        [Display(Name = "下單時間")]
        public DateTime OrderDate { get; set; }

        [Display(Name = "支付方式")]
        public string PaymentMethod { get; set; }

        [Display(Name = "總計")]
        public int TotalAmount { get; set; }

        [Display(Name = "訂單狀態")]
        public string Status { get; set; }

        [Display(Name = "折扣")]
        public int? DiscountAmount { get; set; }

        [Display(Name = "備忘錄")]
        public string Message { get; set; }
    }
}
