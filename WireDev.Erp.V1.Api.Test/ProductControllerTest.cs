using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.EntityFrameworkCore;
using WireDev.Erp.V1.Api.Context;
using WireDev.Erp.V1.Api.Controllers;
using WireDev.Erp.V1.Models.Authentication;
using WireDev.Erp.V1.Models.Storage;

namespace WireDev.Erp.V1.Api.Test;

[TestClass]
public class ProductControllerTest
{
    [TestMethod("Get All Products")]
    public void GetAllProductsTestMethod()
    {
        ILogger<ProductController> logger = Mock.Of<ILogger<ProductController>>();
        DbContextOptions<ApplicationDataDbContext> options =
            new DbContextOptionsBuilder<ApplicationDataDbContext>().UseInMemoryDatabase("Data.WireDevErpV1").Options;
        Mock<ApplicationDataDbContext> dbcMock = new(options);
        List<Product> entities = new()
        {
            new(9999) { Name = "Default_Product", Prices = new() { Guid.NewGuid() } },
            new(9998) { Name = "Default_Product1", Prices = new() { Guid.NewGuid() } },
            new(9997) { Name = "Default_Product2", Prices = new() { Guid.NewGuid() } }
        };
        _ = dbcMock.Setup(x => x.Products).ReturnsDbSet(entities);
        ProductController pc = new(dbcMock.Object, logger);

        ObjectResult response = (ObjectResult)pc.GetProducts().Result;
        Assert.IsNotNull(response);
        Assert.IsTrue(response.StatusCode == StatusCodes.Status200OK, "Status code does not indicate success.");

        Response? r = (Response?)response.Value;
        Assert.IsNotNull(r.Data, "Data in response is emtpy.");
        Assert.IsInstanceOfType(r.Data, typeof(List<uint>), "Data is not an instance of expected type.");
    }

    [TestMethod("Create Product")]
    public void CreateProductTestMethod()
    {
        ILogger<ProductController> logger = Mock.Of<ILogger<ProductController>>();
        DbContextOptions<ApplicationDataDbContext> options =
            new DbContextOptionsBuilder<ApplicationDataDbContext>().UseInMemoryDatabase("Data.WireDevErpV1").Options;
        Mock<ApplicationDataDbContext> dbcMock = new(options);
        _ = dbcMock.Setup(x => x.Products).ReturnsDbSet(new List<Product>());
        ProductController pc = new(dbcMock.Object, logger);
        Product p = new(9990) { Name = "Default_Product", Prices = new() { Guid.NewGuid() } };

        ObjectResult response = (ObjectResult)pc.AddProduct(p).Result;
        Assert.IsNotNull(response);
        Assert.IsTrue(response.StatusCode == StatusCodes.Status201Created, "Status code does not indicate success.");

        Response? r = (Response?)response.Value;
        Assert.IsNotNull(r.Data, "Data in response is emtpy.");
        Assert.IsInstanceOfType(r.Data, typeof(Product), "Data is not an instance of expected type.");
    }

    [TestMethod("Delete Product")]
    public void DeleteProductTestMethod()
    {
        ILogger<ProductController> logger = Mock.Of<ILogger<ProductController>>();
        DbContextOptions<ApplicationDataDbContext> options =
            new DbContextOptionsBuilder<ApplicationDataDbContext>().UseInMemoryDatabase("Data.WireDevErpV1").Options;
        Mock<ApplicationDataDbContext> dbcMock = new(options);
        _ = dbcMock.Setup(x => x.Products).ReturnsDbSet(new List<Product>()
        {
            new(9999) { Name = "Default_Product", Prices = new() { Guid.NewGuid() } },
            new(9998) { Name = "Default_Product1", Prices = new() { Guid.NewGuid() } },
            new(9997) { Name = "Default_Product2", Prices = new() { Guid.NewGuid() } }
        });
        ProductController pc = new(dbcMock.Object, logger);

        ObjectResult response = (ObjectResult)pc.DeleteProduct(9998).Result;
        Assert.IsNotNull(response);
        Assert.IsTrue(response.StatusCode == StatusCodes.Status200OK, "Status code does not indicate success.");
    }
}