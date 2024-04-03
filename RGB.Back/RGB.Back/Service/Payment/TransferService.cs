//using SparkleAir.BLL.Service.Payment.PaymentMethod;
//using SparkleAir.IDAL.IRepository.Payment;
//using SparkleAir.Infa.Dto.Payment;

//namespace SparkleAir.BLL.Service.Payment
//{
//    public class TransferService
//    {
//        public readonly ITransferEFRepository _repo;
        
//        public TransferService(ITransferEFRepository repo)
//        { 
//            _repo = repo;
           
//        }
//        public List<TransferDto> GetAll()
//        {
//            var transfers = _repo.GetAll().Select(p=>new TransferDto
//            {
//                Id = p.Id,
//                PaymentMethod = p.PaymentMethod,
//                PaymentDate = p.PaymentDate,
//                PaymentAmount = p.PaymentAmount,
//                Info = p.Info
//            }).ToList();
//            return transfers;

//        }
//        public TransferDto Get(int id)
//        {
//            var result = GetAll().SingleOrDefault(x => x.Id == id);
//            return result;
//        }

//        //public void Update(TransferDto p) 
//        //{
//        //    TransferEntity entity = new TransferEntity
//        //    {
//        //        Id = p.Id,
//        //        TransferMethodsId = p.TransferMethodsId,
//        //        PaymentDate = p.PaymentDate,
//        //        PaymentAmount = p.PaymentAmount,
//        //        Info = p.Info
//        //    };

//        //    _repo.Update(entity);
//        //}




//        //確認是哪個付款方式??

        
        
//        private IPaymentMethod ChoosePaymentMethod(TransferDto dto)
//        {

//            switch (dto.PaymentMethod)
//            {
//                case "test":
//                    return new ByTest(); // 假設 TestPaymentMethod 實現了 IPaymentMethod 接口
//                case "Paypal":
//                    return new ByPaypal();
//                case "綠界科技":
//                    return new ByEcpay();
//                default:
//                    throw new Exception("未知的付款方式");
//            }
//        }

        




//        public async Task<int> SavePaymentInfo(TransferDto transferDto)
//        {
//            IPaymentMethod payment = ChoosePaymentMethod(transferDto);
//            var paymentId = payment.ProcessPayment(transferDto, _repo);

//            return paymentId;
//        }
//    }
    
//}
