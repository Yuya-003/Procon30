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

        public static void SetToken(string token)
        {
            client.DefaultRequestHeaders.Add("Authorization", "procon30_example_token");
        }

        public static async Task<string> GetHtml(string url)
        {
            client.DefaultRequestHeaders.Add("Authorization", "procon30_example_token");
            var response = client.GetAsync(url);
            var content = await response.Result.Content.ReadAsStringAsync();
            return content;
        }
    }
}
