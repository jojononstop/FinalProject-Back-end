using SparkleAir.IDAL.IRepository.Payment;
using SparkleAir.Infa.Dto.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparkleAir.BLL.Service.Payment.PaymentMethod
{
    public interface IPaymentMethod
    {
        int ProcessPayment(TransferDto dto, ITransferEFRepository _repo) ;
    }
}
