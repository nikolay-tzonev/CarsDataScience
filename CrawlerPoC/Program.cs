using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using SoftCircuits.HtmlMonkey;

namespace CrawlerPoC
{
    class Program
    {
        
        private static string[] sitemaps = new[]
        {
            "https://www.auto.bg/sitemap/adverts.xml",
            "https://www.auto.bg/sitemap/adverts-part1.xml",
            "https://www.auto.bg/sitemap/adverts-part2.xml",
            "https://www.auto.bg/sitemap/adverts-part3.xml",
        };

        static void Main(string[] args)
        {
            using (HttpClient client = new HttpClient())
            {
                Task.WaitAll(sitemaps.Select(sm => CrawlSitemap(sm, client)).ToArray());
            }
        }

        private static async Task CrawlSitemap(string mapUrl, HttpClient client)
        {
            var sitemap = HtmlDocument.FromHtml(await client.GetStringAsync(mapUrl));
            var addUrls = sitemap.Find("loc").Select(x => x.Text);

            using (StreamWriter writter = new StreamWriter(Path.GetFileNameWithoutExtension(mapUrl) + ".csv"))
            {
                foreach (var url in addUrls)
                {
                    var record = await CrawlAdd(url, client);

                    await writter.WriteLineAsync(string.Join(",", record));
                }
            }
        }

        private static async Task<string[]> CrawlAdd(string addUrl, HttpClient client)
        {
            try
            {
                var page = await client.GetStringAsync(addUrl);
                var addDoc = HtmlDocument.FromHtml(page);

                return new[]
                {
                    addDoc.Find("#carPage > ul.carPageName > li.name > div > h1").First().Text, // title
                    addDoc.Find("#leftColumn > div.carData > table.dowble > tbody > tr:nth-child(2) > td:nth-child(2)").First().Text, // price
                    addDoc.Find("#leftColumn > div.carData > table.dowble > tbody > tr:nth-child(2) > td.secColumn").First().Text, // condition
                    addDoc.Find("#leftColumn > div.carData > table.dowble > tbody > tr:nth-child(4) > td:nth-child(2) > a").First().Text, //engine
                    addDoc.Find("#leftColumn > div.carData > table.dowble > tbody > tr:nth-child(3) > td.secColumn").First().Text, // manufactured
                    addDoc.Find("#leftColumn > div.carData > table.dowble > tbody > tr:nth-child(4) > td.secColumn").First().Text, // HP
                    addDoc.Find("#leftColumn > div.carData > table.dowble > tbody > tr:nth-child(5) > td:nth-child(2)").First().Text, // transmision
                    addDoc.Find("#leftColumn > div.carData > table.dowble > tbody > tr:nth-child(6) > td:nth-child(2)").First().Text // mileage
                };
            }
            catch (Exception ex)
            {
                return new[] { $"Error: {ex.Message}" };
            }
        }

    }
}
