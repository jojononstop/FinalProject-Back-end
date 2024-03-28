using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparkleAir.Infa.Dto.Payment
{
    public class ECPayCallbackData
    {
        public string MemberID { get; set; }
        public int RtnCode { get; set; }
        public string RtnMsg { get; set; }
        public string TradeNo { get; set; }
        public decimal TradeAmt { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentType { get; set; }
        public decimal PaymentTypeChargeFee { get; set; }
        public DateTime TradeDate { get; set; }
        public string MerchantTradeNo { get; set; }
        public int SimulatePaid { get; set; }
    }
}
