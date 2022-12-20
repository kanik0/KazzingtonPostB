using KazzingtonPostB.Shared;
using Base64UrlEncoding;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

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
        public ProxyPage Get(String encodedUrl, bool amp)
        {
            string decodedUrl = Base64UrlEncoding.Base64UrlEncoder.Decode(encodedUrl);

            string responseBody = "";
            string responseBodyAMP = "";
            HttpResponseMessage response;
            HttpResponseMessage responseAMP;

            if (amp)
            {
                using (HttpClient clientAMP = new HttpClient())
                {
                    clientAMP.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/108.0.0.0 Safari/537.36 Edg/108.0.1462.46");

                    response = clientAMP.GetAsync(decodedUrl).Result;
                    responseAMP = clientAMP.GetAsync(decodedUrl + "/amp/").Result;

                    if (response.IsSuccessStatusCode && responseAMP.IsSuccessStatusCode)
                    {
                        var responseContent = response.Content;
                        responseBody = responseContent.ReadAsStringAsync().Result;

                        var responseContentAMP = responseAMP.Content;
                        responseBodyAMP = responseContentAMP.ReadAsStringAsync().Result;
                    }
                };
            }
            else
            {
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
            }

            return new ProxyPage
            {
                HTML = responseBody,
                HTMLAMP = responseBodyAMP,
                StatusCode = response.StatusCode
            };
        }
    }
}