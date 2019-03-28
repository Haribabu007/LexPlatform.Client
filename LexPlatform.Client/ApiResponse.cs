using System.Net;
using System.Net.Http.Headers;

namespace LexPlatform.Client
{
    public class ApiResponse
    {
        public HttpHeaders Headers { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public bool IsSuccessStatusCode => StatusCode >= HttpStatusCode.OK && StatusCode <= (HttpStatusCode)299;

        public ApiResponse()
        {
        }

        public ApiResponse(HttpHeaders headers)
        {
            Headers = headers;
        }
    }

    public class ApiResponse<TResult> : ApiResponse
    {
        public TResult Result { get; set; }

        public ApiResponse(TResult result)
        {
            Result = result;
        }

        public ApiResponse(TResult result, HttpHeaders headers)
            : base(headers)
        {
            Result = result;
        }

        public ApiResponse(HttpHeaders headers)
        {
            Headers = headers;
        }
    }
}
