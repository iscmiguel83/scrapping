using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Farmacias_script_to_cs
{
    public class MercadoLibreScraper
    {
        public async Task<List<Dictionary<string, string>>> ScrapeProducts(string urlBase)
        {
            int pageNumber = 1;
            List<Dictionary<string, string>> products = new List<Dictionary<string, string>>();

            while (true)
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/89.0.4389.82 Safari/537.36");

                string pageUrl = $"{urlBase}_Desde_{(pageNumber - 1) * 10}_NoIndex_True";
                HttpResponseMessage response = await client.GetAsync(pageUrl);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Error en la página {pageNumber}");
                    break;
                }

                string htmlContent = await response.Content.ReadAsStringAsync();
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(htmlContent);

                var allData = htmlDoc.DocumentNode.SelectNodes("//div[@class='poly-card__content']");

                if (allData == null || allData.Count == 0)
                {
                    Console.WriteLine($"Total de páginas procesadas: {pageNumber}");
                    break;
                }

                foreach (var data in allData)
                {
                    string description = data.SelectSingleNode(".//h2[@class='poly-box poly-component__title']")?.InnerText ?? "Sin descripción";
                    string normalPrice = data.SelectSingleNode(".//s[@class='andes-money-amount andes-money-amount--previous andes-money-amount--cents-dot']")?.InnerText
                        ?.Replace("$", "").Replace(",", ".") ?? null;
                    string discountPrice = data.SelectSingleNode(".//div[@class='poly-price__current']//span[@class='andes-money-amount andes-money-amount--cents-superscript']")?.InnerText
                        ?.Replace("$", "").Replace(",", ".") ?? null;
                    string link = data.SelectSingleNode(".//a")?.GetAttributeValue("href", null);

                    products.Add(new Dictionary<string, string>
                {
                    { "Description", description },
                    { "Normal Price", normalPrice ?? discountPrice },
                    { "Discount Price", discountPrice ?? normalPrice },
                    { "URL", link ?? "Sin enlace" }
                });
                }

                pageNumber++;
                await Task.Delay(2000);
            }

            return products;
        }

        public void ExportToCsv(List<Dictionary<string, string>> products, string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("Description,Normal Price,Discount Price,URL");

                foreach (var product in products)
                {
                    writer.WriteLine($"\"{product["Description"]}\",\"{product["Normal Price"]}\",\"{product["Discount Price"]}\",\"{product["URL"]}\"");
                }
            }
        }
    }

}
