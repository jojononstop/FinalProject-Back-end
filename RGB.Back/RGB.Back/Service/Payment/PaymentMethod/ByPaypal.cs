//using SparkleAir.IDAL.IRepository.Payment;
//using SparkleAir.Infa.Dto.Payment;
//using SparkleAir.Infa.Entity.Payment;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace SparkleAir.BLL.Service.Payment.PaymentMethod
//{
//    public class ByPaypal : IPaymentMethod
//    {
//        public int ProcessPayment(TransferDto dto, ITransferEFRepository _repo)
//        {


//            var data = new TransferEntity
//            {
//                TransferMethodsId = 2,
//                PaymentDate = DateTime.Now,
//                PaymentAmount = dto.PaymentAmount,
//                Info = dto.Info,
//            };

//            var paymentId = _repo.SavePaymentInfo(data);
//            return paymentId;

//        }
//    }
//}
