using System.Net.Http.Headers;
using System.Net.Http.Json;
using WireDev.Erp.V1.Models;
using WireDev.Erp.V1.Models.Authentication;
using WireDev.Erp.V1.Models.Storage;

namespace WireDev.Erp.V1.Api.Test;

[TestClass]
public class ProductControllerTest
{
    //[TestInitialize]
    //public void InitTest()
    //{
    //    ApiConnection.SetClient(new Uri("https://127.0.0.1:7216/"), new MediaTypeWithQualityHeaderValue("application/json"));
    //}

    [TestMethod]
    public async void GetAllProductsTestMethod()
    {
        using HttpResponseMessage response = await ApiConnection.Client.GetAsync($"api/Products/all");
        Assert.IsNotNull(response);
        Assert.IsTrue(response.IsSuccessStatusCode, "Status code does not indicate success.");

        Response? r = await response.Content.ReadFromJsonAsync<Response>();
        Assert.IsNotNull(r.Data, "Data in response is emtpy.");
        Assert.IsInstanceOfType(r.Data, typeof(List<Product>), "Data is no instance of expected type.");
    }

    [TestMethod]
    public async void CreateProductTestMethod()
    {
        Product p = new() { Name = "test" };
        using HttpResponseMessage response = await ApiConnection.Client.PostAsJsonAsync("api/Products/add", p);
        Assert.IsNotNull(response);
        Assert.IsTrue(response.IsSuccessStatusCode, "Status code does not indicate success.");

        Response? r = await response.Content.ReadFromJsonAsync<Response>();
        Assert.IsNotNull(r.Data, "Data in response is emtpy.");
        Assert.IsInstanceOfType(r.Data, typeof(Product), "Data is no instance of expected type.");
        Assert.AreEqual(p, r.Data);
    }
}