using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparkleAir.Infa.Dto.Payment
{
    public  class TransferDto
    {
        public int? Id { get; set; }

        public int? TransferMethodsId { get; set; }

        public string? PaymentMethod {  get; set; }

        public DateTime? PaymentDate { get; set; }

        public decimal PaymentAmount { get; set; }

        public string? Info { get; set; }
    }
}
