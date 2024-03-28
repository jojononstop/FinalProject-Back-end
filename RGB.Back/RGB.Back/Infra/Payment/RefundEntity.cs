using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparkleAir.Infa.Entity.Payment
{
    public class RefundEntity
    {
        public int Id { get; set; }

        public int TransferImethodsId { get; set; }

        public DateTime RefundDate { get; set; }

        public decimal RefundtAmount { get; set; }

        public int TransferPaymentsId { get; set; }
    }
}
