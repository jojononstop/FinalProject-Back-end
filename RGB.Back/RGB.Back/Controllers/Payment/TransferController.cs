//using Microsoft.AspNetCore.Mvc;
//using RGB.Back.Models;
//using SparkleAir.BLL.Service.Payment;
//using SparkleAir.DAL.EFRepository.Payment;
//using SparkleAir.IDAL.IRepository.Payment;
//using SparkleAir.Infa.Dto.Payment;

//namespace SparkleAir.Front.API.Controllers.Payment
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class TransferController : ControllerBase
//    {
//        private readonly ITransferEFRepository _repo;
//        private readonly TransferService _service;

//        public TransferController(RizzContext db)
//        {
//            _repo = new TransferEFRepository(db);
//            _service = new TransferService(_repo); 
            
//        }

//        /// <summary>
//        /// 付款方法"test"  
//        ///   
//        /// Amount欄位必填
//        /// </summary>
//        /// <param name="transferdto"></param>
//        /// <returns></returns>
//        /// <exception cref="Exception"></exception>
//        [HttpPost]
//        public async Task<int> ProcessPayment([FromBody] TransferDto transferdto)
//        {
//            try
//            {
            

//                // 调用 TransferService 中的方法将支付信息保存到数据库中
//                var paymentId = await _service.SavePaymentInfo(transferdto);

//                return paymentId;
//            }
//            catch (Exception ex)
//            {
//                // 处理异常
//                throw new Exception("未知付款方式");
//            }
//        }

//    }
//}
