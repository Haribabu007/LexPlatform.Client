using System.Collections.Generic;
using System.Net.Http;

namespace LexPlatform.Client.Models
{
    public class PlaceOrderRequest: RequestBase
    {
        public string Market { get; set; }

        public OrderSide Side { get; set; }

        public OrderType OrderType { get; set; }

        public decimal Price { get; set; }

        public decimal Volume { get; set; }


        public override Dictionary<string, string> BuildProperties()
        {
            var qp = new Dictionary<string, string>
            {
                { "market", Market.ToLower() },
                { "price", Price.ToString() },
                { "side",  Side.ToString().ToLower() },
                { "volume", Volume.ToString() },
                { "ord_type", OrderType.ToString().ToLower() }
            };

            return qp;
        }

        public override string Url()
        {
            return "/api/v2/order";
        }

        public PlaceOrderRequest()
        {
            Method = HttpMethod.Post;
        }
    }
}
