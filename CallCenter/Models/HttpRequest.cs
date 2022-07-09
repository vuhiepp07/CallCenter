using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CallCenter.Models
{
    internal class HttpRequest
    {
        HttpClient _httpClient = null;
        public HttpClient httpClient => _httpClient ?? (new HttpClient());

        // Post Json Data
        public string PostAsyncJson(string url, string json)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                request.Content = httpContent;
                var response = httpClient.Send(request);
                var rcontent = response.Content.ReadAsStringAsync();
                return rcontent.Result;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
        }

        public string GetUserDriverAsync(string url)
        {
            // Thiết lập các Header nếu cần
            //httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml+json");
            try
            {
                var msg = new HttpRequestMessage(HttpMethod.Get, url);
                //msg.Headers.Add("User-Agent", "C# Program");
                var res = httpClient.Send(msg);

                var content = res.Content.ReadAsStringAsync();
                return content.Result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }
    }
}
