// See https://aka.ms/new-console-template for more information
// Console.WriteLine("Hello, World!");

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net.Http;
using System.Threading.Tasks;
using Farmacias_script_to_cs;
using HtmlAgilityPack;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Seleccione el sitio web para scraping:");
        Console.WriteLine("1. Mercado Libre");
        Console.WriteLine("2. Farmacias Similares");
        Console.WriteLine("3. Farmacias del Ahorro");

        // Aquí puedes agregar más opciones para otros sitios web.

        string choice = Console.ReadLine();

        if (choice == "1")
        {
            var scraper = new MercadoLibreScraper();
            string url = "https://listado.mercadolibre.com.mx/deportes-fitness/tenis-paddle-squash/equipamiento/bandas-cabeza/nuevo/bandas-deportivas-para-cabello";

            Console.WriteLine("Iniciando scraping para Mercado Libre...");
            var products = await scraper.ScrapeProducts(url);

            string filePath = "C:\\Users\\Daniel\\Documents\\Conversion_python_cs\\Results\\productos_mercadolibre.csv";
            scraper.ExportToCsv(products, filePath);

            Console.WriteLine($"Archivo CSV creado: {filePath}");
        }

        if (choice == "2")
        {
            var scraper = new FarmaciasSimilaresScraper();
            string product = "crema"; // Aquí se puede cambiar el producto.
            int maxPages = 52; // Límite de páginas a procesar.

            Console.WriteLine("Iniciando scraping para Farmacias Similares...");
            var products = await scraper.ScrapeProducts(product, maxPages);

            string filePath = "C:\\Users\\Daniel\\Documents\\Conversion_python_cs\\Results\\farmacias_similares_products.csv";
            scraper.ExportToCsv(products, filePath);

            Console.WriteLine($"Archivo CSV creado en: {filePath}");
        }

        if (choice == "3")
        {
            var scraper = new FarmaciasAhorroScraper();
            string product = "condones"; // Aquí se puede cambiar el producto.
            int maxPages = 52; // Límite de páginas a procesar.

            Console.WriteLine("Iniciando scraping para Farmacias del Ahorro...");
            var products = await scraper.ScrapeProducts(product, maxPages);

            string filePath = "C:\\Users\\Daniel\\Documents\\Conversion_python_cs\\Results\\farmacias_ahorro_products.csv";
            scraper.ExportToCsv(products, filePath);

            Console.WriteLine($"Archivo CSV creado en: {filePath}");
        }

        else
        {
            Console.WriteLine("Opción no válida.");
        }
    }


    // Conexión a la base de datos y exportación de datos
    //string connectionString = "Server=TU_SERVIDOR;Database=TU_BASE_DE_DATOS;User Id=TU_USUARIO;Password=TU_CONTRASEÑA;";
    //InsertDataIntoDatabase(products, connectionString);

    // Exportar los datos a un archivo CSV
    //string filePath = "C:\\Users\\Daniel\\Documents\\Conversion_python_cs\\Results\\productos.csv";
    //    ExportToCsv(products, filePath);
    //    Console.WriteLine($"Archivo CSV creado en: {filePath}");
    //}

    // Método para exportar datos a un archivo CSV
    //static void ExportToCsv(List<Dictionary<string, string>> products, string filePath)
    //{
    //    using (StreamWriter writer = new StreamWriter(filePath))
    //    {
    //        // Escribe el encabezado del CSV
    //        writer.WriteLine("Description,Normal Price,Discount Price,URL");

    //        // Escribe cada producto en el archivo
    //        foreach (var product in products)
    //        {
    //            writer.WriteLine($"\"{product["Description"]}\",\"{product["Normal Price"]}\",\"{product["Discount Price"]}\",\"{product["URL"]}\"");
    //        }
    //    }
    //}


    //static void InsertDataIntoDatabase(List<Dictionary<string, string>> products, string connectionString)
    //{
    //    using (SqlConnection connection = new SqlConnection(connectionString))
    //    {
    //        connection.Open();

    //        foreach (var product in products)
    //        {
    //            using (SqlCommand command = new SqlCommand(
    //                "INSERT INTO Productos (Descripcion, PrecioNormal, PrecioDescuento, URL) VALUES (@Descripcion, @PrecioNormal, @PrecioDescuento, @URL)", connection))
    //            {
    //                command.Parameters.AddWithValue("@Descripcion", product["Description"]);
    //                command.Parameters.AddWithValue("@PrecioNormal", string.IsNullOrEmpty(product["Normal Price"]) ? (object)DBNull.Value : Convert.ToDecimal(product["Normal Price"]));
    //                command.Parameters.AddWithValue("@PrecioDescuento", string.IsNullOrEmpty(product["Discount Price"]) ? (object)DBNull.Value : Convert.ToDecimal(product["Discount Price"]));
    //                command.Parameters.AddWithValue("@URL", product["URL"]);

    //                command.ExecuteNonQuery();
    //            }
    //        }
    //    }

    //    Console.WriteLine("Datos exportados a la base de datos.");
    //}
}

