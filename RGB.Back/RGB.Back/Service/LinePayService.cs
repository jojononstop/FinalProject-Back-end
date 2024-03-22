using Newtonsoft.Json;
using RGB.Back.DTOs;
using RGB.Back.Providers;
using System.Text;
using System.Threading.Channels;

namespace RGB.Back.Service
{
    public class LinePayService
    {
        public LinePayService()
        {
            client = new HttpClient();
        }

        private readonly string channelId = "1657306405";
        private readonly string channelSecretKey = "720c8af4d271ce2fef3535b8821d9e8e";


        private readonly string linePayBaseApiUrl = "https://sandbox-api-pay.line.me";

        private static HttpClient client;
        //private readonly JsonProvider _jsonProvider;

        // 送出建立交易請求至 Line Pay Server
        public async Task<PaymentResponseDto> SendPaymentRequest(PaymentRequestDto dto)
        {
            var json = JsonConvert.SerializeObject(dto);

            Console.WriteLine(json);

            // 產生 GUID Nonce
            var nonce = Guid.NewGuid().ToString();
            // 要放入 signature 中的 requestUrl
            var requestUrl = "/v3/payments/request";

            //使用 channelSecretKey & requestUrl & jsonBody & nonce 做簽章
            var signature = SignatureProvider.HMACSHA256(channelSecretKey, channelSecretKey + requestUrl + json + nonce);

            var request = new HttpRequestMessage(HttpMethod.Post, linePayBaseApiUrl + requestUrl)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            // 帶入 Headers
            client.DefaultRequestHeaders.Add("X-LINE-ChannelId", channelId);
            client.DefaultRequestHeaders.Add("X-LINE-Authorization-Nonce", nonce);
            client.DefaultRequestHeaders.Add("X-LINE-Authorization", signature);

            var response = await client.SendAsync(request);

            string jsonResponse = await response.Content.ReadAsStringAsync();

            // 印出JSON字符串
            //Console.WriteLine(jsonResponse);

            var linePayResponse = JsonConvert.DeserializeObject<PaymentResponseDto>(await response.Content.ReadAsStringAsync());

            //Console.WriteLine(nonce);
            //Console.WriteLine(signature);

            return linePayResponse;
        }
        // 取得 transactionId 後進行確認交易
        public async Task<PaymentConfirmResponseDto> ConfirmPayment(string transactionId, string orderId, PaymentConfirmDto dto) //加上 OrderId 去找資料
        {
            var json = JsonConvert.SerializeObject(dto);

            var nonce = Guid.NewGuid().ToString();
            var requestUrl = string.Format("/v3/payments/{0}/confirm", transactionId);
            var signature = SignatureProvider.HMACSHA256(channelSecretKey, channelSecretKey + requestUrl + json + nonce);

            var request = new HttpRequestMessage(HttpMethod.Post, String.Format(linePayBaseApiUrl + requestUrl, transactionId))
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            client.DefaultRequestHeaders.Add("X-LINE-ChannelId", channelId);
            client.DefaultRequestHeaders.Add("X-LINE-Authorization-Nonce", nonce);
            client.DefaultRequestHeaders.Add("X-LINE-Authorization", signature);

            var response = await client.SendAsync(request);
            var responseDto = JsonConvert.DeserializeObject<PaymentConfirmResponseDto>(await response.Content.ReadAsStringAsync());
            return responseDto;
        }
        public async void TransactionCancel(string transactionId)
        {
            //使用者取消交易則會到這裏。
            Console.WriteLine($"訂單 {transactionId} 已取消");
        }
    }

}
