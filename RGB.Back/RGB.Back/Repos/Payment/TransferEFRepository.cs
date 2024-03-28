using Microsoft.EntityFrameworkCore;
using RGB.Back.Models;
using SparkleAir.IDAL.IRepository.Payment;
using SparkleAir.Infa.Entity.Payment;

namespace SparkleAir.DAL.EFRepository.Payment
{
    public class TransferEFRepository: ITransferEFRepository
    {
        private readonly RizzContext db;
        public TransferEFRepository(RizzContext context)
        {
            db = context;
        }

        public List<TransferEntity> GetAll()
        { 
            var transfers = db.TransferPayments.AsNoTracking().Include(p => p.TransferMethodsId).Select(p => new TransferEntity
        {
            Id = p.Id,
            //TransferMethodsId = p.TransferMethodsId,
            PaymentDate = p.PaymentDate,
            PaymentAmount = p.PaymentAmount,
            Info = p.Info
        }).ToList();
        return transfers;

        }
        public TransferEntity Get(int id)
        {
            var result = GetAll().SingleOrDefault(x => x.Id == id);
            return result;
        }

        public int SavePaymentInfo(TransferEntity entity)
        {

            var transferPayment = new TransferPayment
            {
                TransferMethodsId = entity.TransferMethodsId,
                PaymentDate = entity.PaymentDate,
                PaymentAmount = entity.PaymentAmount,
                Info = entity.Info
            };

            db.TransferPayments.Add(transferPayment);
            db.SaveChanges();

            return transferPayment.Id;
        }
    }
}
