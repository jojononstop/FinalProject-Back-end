using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparkleAir.Infa.Dto.Payment
{
    public class PaypalcallbackDto
    {
        public string PaymentStatus { get; set; }

        // 交易 ID：唯一标识支付交易的 ID。
        public string TransactionId { get; set; }

        // 支付金额：表示用户支付的金额。
        public decimal PaymentAmount { get; set; }

        // 货币：支付所使用的货币。
        public string Currency { get; set; }

        // 付款方信息：包括付款方的姓名、电子邮件地址等。
        public PayerInformation PayerInfo { get; set; }

        // 收款方信息：包括收款方（你的平台）的信息。
        public PayeeInformation PayeeInfo { get; set; }

        // 支付时间：表示支付完成的时间。
        public DateTime PaymentTime { get; set; }

        // 其他相关信息：如订单号、商品信息等。
        public string AdditionalInformation { get; set; }
    }
    public class PayerInformation
    {
        public string Name { get; set; }
        public string Email { get; set; }
        // 其他可能的信息
    }

    public class PayeeInformation
    {
        public string ClientId { get; set; }
        public string Email { get; set; }
    }
}
