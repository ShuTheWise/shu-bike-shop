using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace shu_bike_shop
{
    public class Logger : ILogger
    {
        private readonly HttpClient client = new();
        private readonly IConfiguration configuration;

        public Logger(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public Task Log(string content)
        {
            try
            {
                string url = configuration["LOG_URL"];
                var data = new StringContent(content);
                return client.PostAsync(url, data);
            }
            catch
            {

            }

            return Task.FromResult(false);
        }
    }
}
