using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace UI
{
    class DataTransfer
    {
        private static HttpClient client = new HttpClient();
        private static string url = "http://localhost";
        private static string port = "80";

        public static void SetToken(string token)
        {
            client.DefaultRequestHeaders.Add("Authorization", token);
            //client.DefaultRequestHeaders.Add("Authorization", "procon30_example_token");
        }

        private static async Task<string> GetHtml(string url)
        {
            var response = client.GetAsync(DataTransfer.url + ":" + port + "/" + url);
            var content = await response.Result.Content.ReadAsStringAsync();
            return content;
        }

        public static async Task<string> GetPriorInformation()
        {
            string result = await Task.Run(() => GetHtml("matches"));
            return result;
        }
    }
}
