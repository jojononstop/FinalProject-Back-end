using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparkleAir.Infa.Dto.Payment
{
    public class ECPayDTO
    {
        public string? MerchantID { get; set; }
        public string? MerchantTradeNo { get; set; }
        public string? MerchantTradeDate { get; set; }
        public string? PaymentType { get; set; }
        public int? TotalAmount { get; set; }
        public string? TradeDesc { get; set; }
        public string? ItemName { get; set; }
    }
}
