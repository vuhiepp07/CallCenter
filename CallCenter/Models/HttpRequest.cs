using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CallCenter.Models
{
    internal class HttpRequest
    {
        HttpClient _httpClient = null;
        public HttpClient httpClient => _httpClient ?? (new HttpClient());

        public string PutRequestWithAccessToken(string url,string accessToken)
        {
            try
            {
                var msg = new HttpRequestMessage(HttpMethod.Put, url);
                msg.Headers.Add("token", accessToken);
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

        //public string PutRequest(string url)
        //{
        //    try
        //    {
        //        var msg = new HttpRequestMessage(HttpMethod.Put, url);
        //        //msg.Headers.Add("User-Agent", "C# Program");
        //        var res = httpClient.Send(msg);

        //        var content = res.Content.ReadAsStringAsync();
        //        return content.Result;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        throw ex;
        //    }
        //}

        public string PutAsyncJsonWithAccessToken(string url, string json, string accessToken)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Put, url);
                HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                request.Content = httpContent;
                request.Headers.Add("token", accessToken);
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

        //public string PutAsyncJson(string url, string json)
        //{
        //    try
        //    {
        //        var request = new HttpRequestMessage(HttpMethod.Put, url);
        //        HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
        //        request.Content = httpContent;
        //        var response = httpClient.Send(request);
        //        var rcontent = response.Content.ReadAsStringAsync();
        //        return rcontent.Result;

        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //        throw e;
        //    }
        //}


        public string PostAsyncJsonWithAccessToken(string url, string json, string accessToken)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                request.Content = httpContent;
                request.Headers.Add("token", accessToken);
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

        public string PostLoginJson(string url, string json)
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

        public string GetDataFromUrlAsync(string url)
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

        public string GetDataFromUrlAsyncWithAccessTokenAndJson(string url, string json, string accessToken)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                request.Content = httpContent;
                request.Headers.Add("token", accessToken);
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

        public string GetDataFromUrlAsyncWithAccessToken(string url, string accessToken)
        {
            //httpClient.DefaultRequestHeaders.Add("token", accessToken);
            try
            {
                var msg = new HttpRequestMessage(HttpMethod.Get, url);
                msg.Headers.Add("token", accessToken);
                var res = httpClient.Send(msg);
                var content = res.Content.ReadAsStringAsync();
                MessageBox.Show(msg.ToString());
                return content.Result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }

        public string DeleteDataByUrlWithAccessToken(string url, string accessToken)
        {
            // Thiết lập các Header nếu cần
            //httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml+json");
            try
            {
                var msg = new HttpRequestMessage(HttpMethod.Delete, url);
                msg.Headers.Add("token", accessToken);
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

        //public string DeleteDataByUrl(string url)
        //{
        //    // Thiết lập các Header nếu cần
        //    //httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml+json");
        //    try
        //    {
        //        var msg = new HttpRequestMessage(HttpMethod.Delete, url);
        //        //msg.Headers.Add("User-Agent", "C# Program");
        //        var res = httpClient.Send(msg);

        //        var content = res.Content.ReadAsStringAsync();
        //        return content.Result;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        throw ex;
        //    }
        //}
    }
}
