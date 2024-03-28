using SparkleAir.Infa.Entity.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparkleAir.IDAL.IRepository.Payment
{
    public  interface ITransferEFRepository
    {
        List<TransferEntity> GetAll();
        TransferEntity Get(int id);

        int SavePaymentInfo(TransferEntity entity);
    }
}
