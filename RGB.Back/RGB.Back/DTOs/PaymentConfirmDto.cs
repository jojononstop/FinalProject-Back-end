using Newtonsoft.Json;

namespace RGB.Back.DTOs
{
    public class PaymentConfirmDto
    {
        [JsonProperty("amount")]
        public int Amount { get; set; }
        [JsonProperty("currency")]
        public string Currency { get; set; }
    }
}
