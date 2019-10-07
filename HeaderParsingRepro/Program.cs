using System;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace HeaderParsingRepro
{
    public class Program
    {
        public static async Task Main()
        {
            using var http = new HttpClient();

            var endpoint = new Uri("http://localhost:5000");

            //warmup
            await Run(endpoint, http);

            await RunBenchmark(endpoint, http);

            Console.WriteLine("Done.");
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static Task RunBenchmark(Uri endpoint, HttpClient http) => Run(endpoint, http);

        private static async Task Run(Uri endpoint, HttpClient http)
        {
            for (int i = 0; i < 100; i++)
            {
                var request = new HttpRequestMessage(HttpMethod.Get, endpoint);
                
                request.Headers.TryAddWithoutValidation("Authorization", "ANYTHING SOMEKEY");
                request.Headers.TryAddWithoutValidation("Referer", "http://someuri.com");
                request.Headers.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.90 Safari/537.36");
                request.Headers.TryAddWithoutValidation("Host", "www.somehost.com");

                using var response = await http.SendAsync(request);
                
                response.EnsureSuccessStatusCode();
            }
        }
    }
}
