using Newtonsoft.Json;

namespace LexPlatform.Client.Models
{
    public class PlaceOrderResponse
    {
        [JsonProperty(PropertyName = "error")]
        public OrderError error { get; set; }
    }

    public class OrderError
    {
        [JsonProperty(PropertyName = "code")]
        public int Code { get; set; }
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
    }
}
