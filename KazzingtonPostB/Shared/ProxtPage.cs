using System.Net;

namespace KazzingtonPostB.Shared
{
    public class ProxyPage
    {
        public HttpStatusCode StatusCode { get; set; }
        public string? HTML { get; set; }
    }
}