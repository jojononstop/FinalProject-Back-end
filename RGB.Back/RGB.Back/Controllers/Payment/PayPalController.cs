//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Newtonsoft.Json;
//using RGB.Back.Models;
//using SparkleAir.BLL.Service.Payment;
//using SparkleAir.DAL.EFRepository.Payment;
//using SparkleAir.IDAL.IRepository.Payment;
//using SparkleAir.Infa.Dto.Payment;

//namespace SparkleAir.Front.API.Controllers.Payment
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class PayPalController : ControllerBase
//    {

//        private readonly ITransferEFRepository _repo;
//        private readonly TransferService _service;
//        private readonly RizzContext _db;
//        ILogger<PayPalController> _logger;

//        public PayPalController(ILogger<PayPalController> logger, RizzContext db)
//        {
//            _repo = new TransferEFRepository(db);
//            _service = new TransferService(_repo);
//            _db = db;
//            _logger = logger;
//        }
//        [HttpPost("api/PayPal/webhook")]
//        [AllowAnonymous]
//        public async Task<IActionResult> PalPayBack([FromBody] PayPalCallbackData callbackData) {

//            try
//            {
//                // 构造 TransferPayment 对象
//                var transferPayment = new TransferPayment
//                {
//                    TransferMethodsId = 2,
//                    PaymentDate = DateTime.Now,
//                    PaymentAmount = callbackData.PaymentAmount,
//                    Info = JsonConvert.SerializeObject(new
//                    {
//                        Event_ID = callbackData.Event_ID,
//                        Event_Type = callbackData.Event_Type
//                    })
//                };

               

//                // 将 transferPayment 添加到数据库中
//                _db.TransferPayments.Add(transferPayment);

//                // 保存更改到数据库
//                await _db.SaveChangesAsync();

//                // 返回成功响应
//                return Ok("Success");
//            }
//            catch (Exception ex)
//            {
//                // 如果发生错误，返回错误响应
//                _logger.LogError($"Error occurred while processing PayPal callback: {ex.Message}");
//                return StatusCode(500, $"Error occurred while processing PayPal callback: {ex.Message}");
//            }
//        }
//        //[HttpPost("callback")]
//        //public async Task<IActionResult> PayPalCallback([FromBody] PayPalCallbackData callbackData)
//        //{
//        //    try
//        //    {
//        //        // 根據你的邏輯處理 PayPal 回調數據
//        //        // 例如：更新訂單狀態、發送確認郵件等操作

//        //        // 這裡只是一個示例，你需要根據你的業務邏輯來實現相應的處理

//        //        Console.WriteLine("Received PayPal callback:");
//        //        Console.WriteLine(JsonConvert.SerializeObject(callbackData));

//        //        // 返回成功狀態碼
//        //        return Ok();
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        // 如果發生錯誤，返回內部服務器錯誤
//        //        Console.WriteLine($"Error occurred while processing PayPal callback: {ex.Message}");
//        //        return StatusCode(500, "Internal server error");
//        //    }
//        //}
//    } 
//}

