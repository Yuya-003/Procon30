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
            return await response.Result.Content.ReadAsStringAsync();
        }

        public static async Task<string> GetPriorInformation()
        {
            return await Task.Run(() => GetHtml("matches"));
        }

        public static async Task<Structure.FieldJson> GetMatcheInformation(int id)
        {
            var field = new Structure.FieldJson(await Task.Run(() => GetHtml("matches/" + id.ToString())));
            return field;
        }

        public static async Task<Structure.ActionJson[]> PostAction(int id, string action)
        {
            var content = new StringContent("", Encoding.UTF8, @"application/json");
            var response = client.PostAsync(url + ":" + port + "/matches/" + id.ToString() + "/action", content);
            return await response.Result.Content.ReadAsStringAsync();
        }
    }
}
