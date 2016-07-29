using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RetryStrategies
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the url of a site.");
            var url = Console.ReadLine();
            PrintSiteHtml(url);
            Console.WriteLine("Press any key to exit.");
            Console.Read();
        }

        public static void PrintSiteHtml(string url)
        {
            HttpClient httpClient = new HttpClient();
            string html = string.Empty;

            RetryStrategy.TightLoop(5, () =>
            {
                HttpResponseMessage response = httpClient.GetAsync(url).Result;
                if (!response.IsSuccessStatusCode) throw new ApplicationException("Could not connect");
                html = response.Content.ReadAsStringAsync().Result;
            });

            RetryStrategy.ConstantTimeInterval(10, 2, () =>
            {
                HttpResponseMessage response = httpClient.GetAsync(url).Result;
                if (!response.IsSuccessStatusCode) throw new ApplicationException("Could not connect");
                html = response.Content.ReadAsStringAsync().Result;
            });

            RetryStrategy.RandomInterval(10, 1, 5, () =>
            {
                HttpResponseMessage response = httpClient.GetAsync(url).Result;
                if (!response.IsSuccessStatusCode) throw new ApplicationException("Could not connect");
                html = response.Content.ReadAsStringAsync().Result;
            });

            RetryStrategy.ExponentialBackOff(10, 1, 10, () =>
            {
                HttpResponseMessage response = httpClient.GetAsync(url).Result;
                if (!response.IsSuccessStatusCode) throw new ApplicationException("Could not connect");
                html = response.Content.ReadAsStringAsync().Result;
            });

            Console.WriteLine(html);
        }
    }
}
