using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AccessApiOne
{
    class Program
    {
        static void Main(string[] args)
        {
            Test().Wait();
            Console.ReadKey();
        }

        static async Task Test()
        {
            var discoveryResponse = await DiscoveryClient.GetAsync("http://localhost:12345");
            if (discoveryResponse.IsError)
            {
                Console.WriteLine($"访问IdentityServer服务出错：{discoveryResponse.Error}");
                Console.ReadKey();
                return;
            }

            // 客户端模式
            // 使用IdentityServer服务端配置的客户端Id和Secret，通过TokenEndpoint地址向IdentityServer服务端获取访问one-api资源需要的AccessToken
            TokenClient tokenClient = new TokenClient(discoveryResponse.TokenEndpoint, "client1", "secret1");
            TokenResponse tokenResponse = await tokenClient.RequestClientCredentialsAsync("one-api");
            if (tokenResponse.IsError)
            {
                Console.WriteLine($"获取AccessToken出错：{tokenResponse.Error}");
                Console.ReadKey();
                return;
            }

            Console.WriteLine(tokenResponse.Json);
            HttpClient httpClient = new HttpClient();
            // 使用获取到的AccessToken访问user-api资源
            httpClient.SetBearerToken(tokenResponse.AccessToken);
            var httpResonse = await httpClient.GetAsync("https://localhost:5001/one/values");
            if (httpResonse.IsSuccessStatusCode)
            {
                var content = await httpResonse.Content.ReadAsStringAsync();
                Console.WriteLine(JArray.Parse(content));
            }
            else
            {
                Console.WriteLine($"访问OneApi接口失败：{httpResonse.StatusCode}");
            }
        }
    }
}
