using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Controls;
using WireDev.Erp.V1.Models.Storage;

namespace WireDev.Erp.V1.Client.Windows.Pages
{
    /// <summary>
    /// Interaktionslogik für Storage.xaml
    /// </summary>
    public partial class ProductsPage : Page
    {
        public ProductsPage()
        {
            InitializeComponent();

            // Update port # in the following line.
            client.BaseAddress = new Uri("http://localhost:64195/api");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private static readonly HttpClient client = new();

        private static async Task<Uri> CreateProductAsync(Product product)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(
                "api/products", product);
            _ = response.EnsureSuccessStatusCode();

            // return URI of the created resource.
            return response.Headers.Location;
        }

        private static async Task<Product> GetProductAsync(uint id)
        {
            Product? product = null;
            HttpResponseMessage response = await client.GetAsync("Products/" + id);
            if (response.IsSuccessStatusCode)
            {
                product = await response.Content.ReadFromJsonAsync<Product>();
            }
            return product;
        }

        private static async Task<Product> UpdateProductAsync(Product? product)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync(
                $"api/products/{product.Uuid}", product);
            _ = response.EnsureSuccessStatusCode();

            // Deserialize the updated product from the response body.
            product = await response.Content.ReadFromJsonAsync<Product>();
            return product;
        }

        private static async Task<HttpStatusCode> DeleteProductAsync(uint id)
        {
            HttpResponseMessage response = await client.DeleteAsync(
                $"api/products/{id}");
            return response.StatusCode;
        }
    }
}
