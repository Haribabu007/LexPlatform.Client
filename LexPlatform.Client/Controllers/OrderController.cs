using Microsoft.AspNetCore.Mvc;
using LexPlatform.Client.Models;
using System.Threading.Tasks;

namespace LexPlatform.Client.Controllers
{
    [ApiController]
    public class OrderController : ControllerBase
    {
        readonly IPlatformApi _platformApi;

        public OrderController(IPlatformApi platformApi)
        {
            _platformApi = platformApi;
        }

        [HttpGet]
        [Route("order")]
        public async Task Order()
        {
            var request = new PlaceOrderRequest
            {
                Market = "ltcbtc",
                Side = OrderSide.Buy,
                OrderType = OrderType.Limit,
                Price = 0.01511161m,
                Volume = 1,
                Timestamp = new TimeStamp()
            };
            var res = await _platformApi.PlaceOrder(request);
        }
    }
}
