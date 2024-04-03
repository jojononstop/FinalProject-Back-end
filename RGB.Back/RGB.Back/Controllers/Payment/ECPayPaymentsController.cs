//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Newtonsoft.Json;
//using RGB.Back.Models;
//using SparkleAir.BLL.Service.Payment;
//using SparkleAir.DAL.EFRepository.Payment;
//using SparkleAir.IDAL.IRepository.Payment;
//using SparkleAir.Infa.Dto.Payment;
//using System.Security.Cryptography;
//using System.Text;
//using System.Web;

//namespace SparkleAir.Front.API.Controllers.Payment
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class ECPayPaymentsController : ControllerBase
//    {
//        private readonly ITransferEFRepository _repo;
//        private readonly TransferService _service;
//        private readonly RizzContext _db;
//        ILogger<ECPayPaymentsController> _logger;


//        public ECPayPaymentsController(ILogger<ECPayPaymentsController>  logger, RizzContext db)
//        {
//            _repo = new TransferEFRepository(db);
//            _service = new TransferService(_repo);
//            _db = db;
//            _logger = logger;
//        }

//        // GET: api/ECPayPayments/Index
//        [HttpGet("Index")]
//        public async Task<List<OrderDetail>> Index(string TotalAmount, string ItemName)
//        {
//            ECPayDTO Ec = new ECPayDTO
//            {
//                MerchantID = "3002607",
//                MerchantTradeNo = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 20),
//                MerchantTradeDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
//                PaymentType = "aio",
//                TotalAmount =Int32.Parse(TotalAmount),
//                TradeDesc = "無",
//                ItemName = ItemName
//            };
//            //Add Order
//           // await AddOrder(Ec);

//            //Do Payment
//            var website1 = $"http://localhost:8889";
//            var website2 = $"http://localhost:8888";   //需填入你的網址
//            //var order = new Dictionary<string, string>
//            //{
//            //    { "MerchantID",  Ec.MerchantID },
//            //    { "MerchantTradeNo", Ec.MerchantTradeNo},
//            //    { "MerchantTradeDate",  Ec.MerchantTradeDate},
//            //    { "PaymentType",  Ec.PaymentType},
//            //    { "TotalAmount",  Convert.ToString(Ec.TotalAmount)},
//            //    { "TradeDesc", Ec.TradeDesc },
//            //    { "ItemName",  Ec.ItemName},
//            //    { "ExpireDate",  "3"},
//            //    { "ReturnURL",  $"{website1}/api/ECPayPayments/PayInfo"},
//            //    { "OrderResultURL", $"{website1}/api/ECPayPayments/GetPayInfo"},
//            //    { "ChoosePayment", "ALL"},
//            //    { "PaymentInfoURL",  $"{website1}/api/ECPayPayments/AddAccountInfo"},
//            //    { "EncryptType",  "1"},
//            //    { "ClientRedirectURL",  $"{website2}/Pay/milePurchaseTest"},

//            //};
//            var order = new List <OrderDetail>
//{
//    new OrderDetail { key="MerchantID", data= Ec.MerchantID },
//    new OrderDetail { key="MerchantTradeNo", data= Ec.MerchantTradeNo},
//    new OrderDetail { key="MerchantTradeDate", data= Ec.MerchantTradeDate},
//    new OrderDetail { key="PaymentType", data= Ec.PaymentType},
//    new OrderDetail { key="TotalAmount", data= Convert.ToString(Ec.TotalAmount)},
//    new OrderDetail { key="TradeDesc", data= Ec.TradeDesc },
//    new OrderDetail { key="ItemName", data= Ec.ItemName},
//    new OrderDetail { key="ExpireDate", data= "3"},
//    new OrderDetail { key="ReturnURL", data= $"{website1}/api/ECPayPayments/PayInfo"},
//    new OrderDetail { key="OrderResultURL", data= $"{website1}/api/ECPayPayments/GetPayInfo"},
//    new OrderDetail { key="ChoosePayment", data= "ALL"},
//    new OrderDetail { key="PaymentInfoURL", data= $"{website1}/api/ECPayPayments/AddAccountInfo"},
//    new OrderDetail { key="EncryptType", data= "1"},
//    new OrderDetail { key="ClientRedirectURL", data= $"{website2}/Pay/milePurchaseTest"},
//};

//            var mac = new OrderDetail { key = "CheckMacValue", data = GetCheckMacdata(order) };
//            order.Add(mac);


//            return order;
//        }
//        [HttpPost("GetPayInfo")]
//        public async Task<IActionResult> GetPayInfo([FromForm] IFormCollection form)
//        {
//            try
//            {
//                // 提取表單數據中的相關信息
//                var ecPayData = new ECPayCallbackData
//                {
//                    MemberID = form["MemberID"],
//                    RtnCode = int.Parse(form["RtnCode"]),
//                    RtnMsg = form["RtnMsg"],
//                    TradeNo = form["TradeNo"],
//                    TradeAmt = decimal.Parse(form["TradeAmt"]),
//                    PaymentDate = DateTime.Parse(form["PaymentDate"]),
//                    PaymentType = form["PaymentType"],
//                    PaymentTypeChargeFee = decimal.Parse(form["PaymentTypeChargeFee"]),
//                    TradeDate = DateTime.Parse(form["TradeDate"]),
//                    MerchantTradeNo = form["MerchantTradeNo"],
//                    SimulatePaid = int.Parse(form["SimulatePaid"])
//                };
//                var transferDto = new TransferDto
//                {
//                    PaymentDate = DateTime.Now,
//                    PaymentAmount = ecPayData.TradeAmt,
//                    Info = JsonConvert.SerializeObject(new
//                    {
//                        RtnCode = ecPayData.RtnCode,
//                        RtnMsg = ecPayData.RtnMsg,
//                        TradeNo= ecPayData.TradeNo,
//                        PaymentType = ecPayData.PaymentType,
//                        TradeDate = DateTime.Parse(form["TradeDate"]),
//                        MerchantTradeNo = ecPayData.MerchantTradeNo,
//                        SimulatePaid = int.Parse(form["SimulatePaid"])
//                    })
//                };
                

//                // 檢查是否已存在相同的訂單
//                var existingOrder = await _db.TransferPayments.FirstOrDefaultAsync(m => m.Info.Contains(ecPayData.MerchantTradeNo));

//                if (existingOrder != null)
//                {
//                    // 如果已存在相同的訂單，更新其信息
//                    existingOrder.Info = transferDto.Info;
                    
//                    existingOrder.PaymentAmount = transferDto.PaymentAmount;
//                }
//                else
//                {
//                    // 如果不存在相同的訂單，創建新的訂單並保存到資料庫
//                    var transferPayment = new TransferPayment
//                    {
//                        TransferMethodsId = 1, // 設置有效的 TransferMethodsId
//                        PaymentDate = DateTime.Now,
//                        PaymentAmount = ecPayData.TradeAmt,
//                        Info = transferDto.Info
//                    };
//                    _db.TransferPayments.Add(transferPayment);
//                }

//                await _db.SaveChangesAsync();

//                //return Redirect("http://127.0.0.1:8888/Pay/milePurchaseTest");

//                // 從資料庫中查詢相應的實體以獲取 paymentId
//                var searchId = await _db.TransferPayments.FirstOrDefaultAsync(p => p.Info == transferDto.Info);
//                var paymentId = searchId?.Id;


//              return Redirect($"http://127.0.0.1:8888/Pay/milePurchaseTest/{paymentId}");
//            }
//            catch (Exception ex)
//            {
//                // 如果發生異常，記錄錯誤並返回錯誤狀態碼
//                _logger.LogError($"Error occurred while processing payment callback: {ex.Message}");
//                return StatusCode(500, "Internal server error");
//            }
//        }


//        [HttpPost("/AddPayInfo/{id}")]
//        public string AddPayInfo([FromForm] IFormCollection PayInfo)
//        {
//            _logger.LogInformation($"MerchantTradeNo:{PayInfo}, 時間:{DateTime.Now}");
//            return "OK";
//        }

//        [HttpPost("AddAccountInfo")]
//        public string AddAccountInfo(AccountInfoDTO AccountInfo)
//        {
//            _logger.LogInformation($"MerchantTradeNo:{AccountInfo}, 時間:{DateTime.Now}");
//            return "OK";
//        }
//        //[HttpPost]
//        //[Route("ECPayCallback")]
//        //public async Task<IActionResult> ECPayCallback([FromBody] ECPayCallbackData ecPayData)
//        //{
//        //    // 解析ECPAY返回的数据
//        //    // 提取您需要的信息
//        //    var paymentDate = DateTime.Now;
//        //    var paymentAmount = ecPayData.TradeAmt;
//        //    var info = new
//        //    {
//        //        RtnCode = ecPayData.RtnCode,
//        //        RtnMsg = ecPayData.RtnMsg,
//        //        PaymentType = ecPayData.PaymentType,
//        //        MerchantTradeNo = ecPayData.MerchantTradeNo
//        //    };
//        //    string infoJson = JsonConvert.SerializeObject(info);

//        //    int transferMethodsId = 1; // 設置有效的 TransferMethodsId，這裡使用 1 作為示例

//        //    // 创建TransferPayment对象并存储到数据库
//        //    var transferPayment = new TransferPayment
//        //    {
//        //        TransferMethodsId = transferMethodsId,
//        //        PaymentDate = paymentDate,
//        //        PaymentAmount = paymentAmount,
//        //        Info = infoJson
//        //    };

//        //    _db.TransferPayments.Add(transferPayment);
//        //    await _db.SaveChangesAsync();

//        //    return Ok(); // 返回成功状态码
//        //}

//        [HttpPost("PayInfo")]
//        public async Task<ActionResult> PayInfo(IFormCollection col)
//        {
//            var merchantTradeNo = col["MerchantTradeNo"];

//            // 使用 LINQ 查詢來查找包含指定 MerchantTradeNo 的訂單
//            var order = _db.TransferPayments.FirstOrDefault(m => m.Info.Contains(merchantTradeNo));

//            if (order != null)
//            {
//                var ecPayCallbackData = new ECPayCallbackData
//                {
//                    RtnCode = int.Parse(col["RtnCode"]),
//                    RtnMsg = col["RtnMsg"] == "Succeeded" ? "已付款" : "未付款",
//                    PaymentDate = Convert.ToDateTime(col["PaymentDate"]),
//                    // 其他屬性的賦值
//                };
//                var infoJson = JsonConvert.SerializeObject(ecPayCallbackData);

//                // 更新訂單的相關信息
//                order.Info = infoJson;

//                // 如果 SimulatePaid 是整數類型，您也可以更新它
//                if (int.TryParse(col["SimulatePaid"], out int simulatePaid))
//                {
//                    // 只有在成功解析後才更新 SimulatePaid 屬性
//                    ecPayCallbackData.SimulatePaid = simulatePaid;
//                }

//                await _db.SaveChangesAsync();

//                return Ok("ok");
//            }
//            else
//            {
//                return NotFound(); // 找不到相應的訂單
//            }
//        }


//        [HttpPost("AccountInfo")]
//        public async Task<ActionResult> AccountInfo([FromForm] IFormCollection col)
//        {
//            var data = new Dictionary<string, string>();
//            foreach (string key in col.Keys)
//            {
//                data.Add(key, col[key]);
//            }

//            // Process account info logic here

//            return Ok("Account information processed successfully");
//        }

//        //private async Task<string> AddOrder(ECPayDTO EcPayDto)
//        //{
//        //    EcpayOrders Orders = new EcpayOrders();
//        //    Orders.MemberId = EcPayDto.MerchantID;
//        //    Orders.MerchantTradeNo = EcPayDto.MerchantTradeNo;
//        //    Orders.RtnCode = 0; //未付款
//        //    Orders.RtnMsg = "訂單成功尚未付款";
//        //    Orders.TradeNo = EcPayDto.MerchantID.ToString();
//        //    Orders.TradeAmt = EcPayDto.TotalAmount;
//        //    Orders.PaymentDate = Convert.ToDateTime(EcPayDto.MerchantTradeDate);
//        //    Orders.PaymentType = EcPayDto.PaymentType;
//        //    Orders.PaymentTypeChargeFee = "0";
//        //    Orders.TradeDate = EcPayDto.MerchantTradeDate;
//        //    Orders.SimulatePaid = 0;
//        //    _repo..TransferPayment.Info.Add(Orders);
//        //    await _repo.TransferPayment.Info.SaveChangesAsync();
//        //    return "OK";
//        //}

//        private string GetCheckMacdata(List<OrderDetail> order)
//        {
//            var param = order.OrderBy(x => x.key).Select(o => o.key + "=" + o.data).ToList();
//            string checkdata = string.Join("&", param);
//            var hashKey = "pwFHCqoQZGmho4w6";
//            var HashIV = "EkRm7iFT261dpevs";
//            checkdata = $"HashKey={hashKey}" + "&" + checkdata + $"&HashIV={HashIV}";
//            checkdata = HttpUtility.UrlEncode(checkdata).ToLower();
//            checkdata = GetSHA256(checkdata);
//            return checkdata.ToUpper();
//        }
//        //private string GetCheckMacdata(Dictionary<string, string> order)
//        //{
//        //    var param = order.Keys.OrderBy(x => x).Select(key => key + "=" + order[key]).ToList();
//        //    string checkdata = string.Join("&", param);
//        //    var hashKey = "pwFHCqoQZGmho4w6";
//        //    var HashIV = "EkRm7iFT261dpevs";
//        //    checkdata = $"HashKey={hashKey}" + "&" + checkdata + $"&HashIV={HashIV}";
//        //    checkdata = HttpUtility.UrlEncode(checkdata).ToLower();
//        //    checkdata = GetSHA256(checkdata);
//        //    return checkdata.ToUpper();
//        //}

//        private string GetSHA256(string data)
//        {
//            var result = new StringBuilder();
//            var sha256 = SHA256.Create();
//            var bts = Encoding.UTF8.GetBytes(data);
//            var hash = sha256.ComputeHash(bts);

//            for (int i = 0; i < hash.Length; i++)
//            {
//                result.Append(hash[i].ToString("X2"));
//            }

//            return result.ToString();
//        }
//    }
//}
