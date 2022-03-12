using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Crawler
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            string websiteUrl;
            try
            {
                if (args.Length < 1)
                {
                    throw new ArgumentNullException();
                }
                else
                {
                    websiteUrl = args[0];
                }
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("ArgumentNullException");
                return;
            }

            try
            {
                Regex reg = new Regex(@"(http|https)://(([www\.])?|([\da-z-\.]+))\.([a-z\.]{2,3})$");

                if (reg.IsMatch(websiteUrl))
                {
                    websiteUrl = args[0];
                }
                else
                {
                    throw new ArgumentException();
                }
            }
            catch (ArgumentException)
            {
                Console.WriteLine("ArgumentException");
                return;
            }

            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(websiteUrl);
            string content;
            if (response.IsSuccessStatusCode)
            {
                content = await response.Content.ReadAsStringAsync();
            }
            else
            {
                Console.WriteLine("Błąd w czasie pobierania strony");
                httpClient.Dispose();
                return;
            }

            Regex regex = new Regex(@"(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|""(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*"")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])");
            MatchCollection matchCollection = regex.Matches(content);

            if (matchCollection.Count == 0)
            {
                Console.WriteLine("Nie znaleziono adresów email");
            }
            else
            {
                var distinctCollection = matchCollection.OfType<Match>().Select(x => x.Value).Distinct();
                foreach (var match in distinctCollection)
                {
                    Console.WriteLine(match);
                }
            }
            httpClient.Dispose();
        }
    }
}
