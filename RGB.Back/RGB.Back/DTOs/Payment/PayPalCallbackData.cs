using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparkleAir.Infa.Dto.Payment
{
    public class PayPalCallbackData
    {
        public string Event_Type { get; set; }
        public string Event_ID { get; set; }
        public decimal PaymentAmount { get; set;}
    }
}
