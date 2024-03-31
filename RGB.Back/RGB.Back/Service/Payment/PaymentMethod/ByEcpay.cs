using Newtonsoft.Json;
using SparkleAir.DAL.EFRepository.Payment;
using SparkleAir.IDAL.IRepository.Payment;
using SparkleAir.Infa.Dto.Payment;
using SparkleAir.Infa.Entity.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparkleAir.BLL.Service.Payment.PaymentMethod
{
    public class ByEcpay : IPaymentMethod
    {
        private readonly ITransferEFRepository _repo;
        //private readonly ECPayCallbackDataToTransferDtoConverter _converter;



        //public void Convert(ECPayCallbackData ecPayData)
        //{
        //    var transferDto = _converter.Convert(ecPayData);
        //}

        public int ProcessPayment(TransferDto dto, ITransferEFRepository repo)
        {
            var transferEntity = new TransferEntity
            {
                
                TransferMethodsId = dto.TransferMethodsId??1,
                PaymentMethod = dto.PaymentMethod,
                PaymentDate = dto.PaymentDate??DateTime.Now,
                PaymentAmount = dto.PaymentAmount,
                Info = dto.Info
            };
            var paymentId = repo.SavePaymentInfo(transferEntity);
            return paymentId;
        }
    }
}
