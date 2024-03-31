using SparkleAir.IDAL.IRepository.Payment;
using SparkleAir.Infa.Dto.Payment;
using SparkleAir.Infa.Entity.Payment;

namespace SparkleAir.BLL.Service.Payment.PaymentMethod
{
    public class ByTest : IPaymentMethod
    {
        public int ProcessPayment(TransferDto dto, ITransferEFRepository _repo)
        {
            var data = new TransferEntity
            {
                TransferMethodsId = 3,
                PaymentDate = DateTime.Now,
                PaymentAmount = dto.PaymentAmount,
                Info = "test"
            };

            var paymentId=_repo.SavePaymentInfo(data);
            return paymentId;
        }

       
    }
}
