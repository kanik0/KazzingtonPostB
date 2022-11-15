using KazzingtonPostB.Shared;
using Base64UrlEncoding;
using Microsoft.AspNetCore.Mvc;

namespace KazzingtonPostB.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GetPageController : ControllerBase
    {
        private readonly ILogger<GetPageController> _logger;

        public GetPageController(ILogger<GetPageController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ProxyPage Get(String encodedUrl)
        {
            string decodedUrl = Base64UrlEncoder.Decode(encodedUrl);

            string responseBody = "";
            HttpResponseMessage response;


            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (compatible; Googlebot/2.1; +http://www.google.com/bot.html)");
                client.DefaultRequestHeaders.Add("X-Forwarded-For", "66.249.66.1");
                client.DefaultRequestHeaders.Add("Referer", "https://www.google.com/");

                response = client.GetAsync(decodedUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content;
                    responseBody = responseContent.ReadAsStringAsync().Result;
                }

            };

            return new ProxyPage
            {
                HTML = responseBody,
                StatusCode = response.StatusCode
            };
        }
    }
}