using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using HtmlAgilityPack;

namespace Farmacias_script_to_cs
{
    public class FarmaciasSimilaresScraper
    {
        private readonly string userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/89.0.4389.82 Safari/537.36";

        public async Task<List<Dictionary<string, string>>> ScrapeProducts(string product, int maxPages = 52)
        {
            List<Dictionary<string, string>> products = new List<Dictionary<string, string>>();
            int page = 1;

            while (true)
            {
                if (page > maxPages) break;

                string url = $"https://www.farmaciasdesimilares.com/{product}?map=ft&page={page}";
                Console.WriteLine($"Scraping URL: {url}");

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("User-Agent", userAgent);
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"Error: {response.StatusCode}");
                        break;
                    }

                    string htmlContent = await response.Content.ReadAsStringAsync();
                    var htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(htmlContent);

                    var items = htmlDoc.DocumentNode.SelectNodes("//div[contains(@class, 'vtex-search-result-3-x-galleryItem')]");

                    if (items == null || items.Count == 0)
                    {
                        Console.WriteLine("No more items found, exiting loop.");
                        break;
                    }

                    Console.WriteLine($"Found {items.Count} items on page {page}");

                    foreach (var item in items)
                    {
                        string description = item.SelectSingleNode(".//span[contains(@class, 'vtex-product-summary-2-x-brandName')]")?.InnerText.Trim() ?? "Sin descripción";
                        string currentPrice = item.SelectSingleNode(".//span[contains(@class, 'vtex-product-summary-2-x-currencyContainer')]")?.InnerText.Trim().Replace("$", "") ?? "0";

                        products.Add(new Dictionary<string, string>
                    {
                        { "Description", description },
                        { "Current Price", currentPrice }
                    });
                    }
                }

                page++;
                await Task.Delay(2000); // Pausa de 2 segundos para evitar ser bloqueado.
            }

            return products;
        }

        public void ExportToCsv(List<Dictionary<string, string>> products, string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("Description,Current Price");

                foreach (var product in products)
                {
                    writer.WriteLine($"\"{product["Description"]}\",\"{product["Current Price"]}\"");
                }
            }
        }
    }

}
