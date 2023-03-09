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
    public List<Product> productsTemp { get; } = new()
    {
        new(9999) { Name = "Default_Product", Prices = new() { Guid.NewGuid() } },
        new(9998) { Name = "Default_Product1", Prices = new() { Guid.NewGuid() } },
        new(9997) { Name = "Default_Product2", Prices = new() { Guid.NewGuid() } }
    };

    [TestMethod("Get all products")]
    public void GetAllProductsTestMethod()
    {
        ILogger<ProductController> logger = Mock.Of<ILogger<ProductController>>();
        DbContextOptions<ApplicationDataDbContext> options =
            new DbContextOptionsBuilder<ApplicationDataDbContext>().UseInMemoryDatabase("Data.WireDevErpV1").Options;
        Mock<ApplicationDataDbContext> dbcMock = new(options);
        _ = dbcMock.Setup(x => x.Products).ReturnsDbSet(productsTemp);
        ProductController pc = new(dbcMock.Object, logger);

        ObjectResult response = (ObjectResult)pc.GetProducts().Result;
        Assert.IsNotNull(response);
        Assert.IsTrue(response.StatusCode == StatusCodes.Status200OK, "Status code does not indicate success: " + response.StatusCode);

        Assert.IsNotNull(response.Value, "Data in response is emtpy.");
        Assert.IsInstanceOfType(response.Value, typeof(List<uint>), "Data is not an instance of expected type.");
    }

    [TestMethod("Get single product")]
    public void GetProductTestMethod()
    {
        uint id = 9997;
        ILogger<ProductController> logger = Mock.Of<ILogger<ProductController>>();
        DbContextOptions<ApplicationDataDbContext> options =
            new DbContextOptionsBuilder<ApplicationDataDbContext>().UseInMemoryDatabase("Data.WireDevErpV1").Options;
        Mock<ApplicationDataDbContext> dbcMock = new(options);
        _ = dbcMock.Setup(x => x.Products).ReturnsDbSet(productsTemp);
        ProductController pc = new(dbcMock.Object, logger);

        ObjectResult response = (ObjectResult)pc.GetProduct(id).Result;
        Assert.IsNotNull(response);
        Assert.IsTrue(response.StatusCode == StatusCodes.Status200OK, "Status code does not indicate success: " + response.StatusCode);

        Assert.IsNotNull(response.Value, "Data in response is emtpy.");
        Assert.IsInstanceOfType(response.Value, typeof(Product), "Data is not an instance of expected type.");
        Assert.IsTrue(((Product)response.Value).Uuid == id, "Product is not as expected.");
    }

    [TestMethod("Create product")]
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
        Assert.IsTrue(response.StatusCode == StatusCodes.Status201Created, "Status code does not indicate success: " + response.StatusCode);

        Assert.IsNotNull(response.Value, "Data in response is emtpy.");
        Assert.IsInstanceOfType(response.Value, typeof(Product), "Data is not an instance of expected type.");
    }

    [TestMethod("Modify product")]
    public void ModifyProductTestMethod()
    {
        Product p = productsTemp[0];
        p.Name = "new name";
        p.Add(17);

        ILogger<ProductController> logger = Mock.Of<ILogger<ProductController>>();
        DbContextOptions<ApplicationDataDbContext> options =
            new DbContextOptionsBuilder<ApplicationDataDbContext>().UseInMemoryDatabase("Data.WireDevErpV1").Options;
        Mock<ApplicationDataDbContext> dbcMock = new(options);
        _ = dbcMock.Setup(x => x.Products).ReturnsDbSet(productsTemp);
        ProductController pc = new(dbcMock.Object, logger);

        ObjectResult response = (ObjectResult)pc.ModifyProduct(p.Uuid, p).Result;
        Assert.IsNotNull(response);
        Assert.IsTrue(response.StatusCode == StatusCodes.Status200OK, "Status code does not indicate success: " + response.StatusCode);

        Assert.IsNotNull(response.Value, "Data in response is emtpy.");
        Assert.IsInstanceOfType(response.Value, typeof(Product), "Data is not an instance of expected type.");

        Product? p2 = response.Value as Product;
        Assert.IsTrue(p.Name == p2.Name, "Not all properties are changed correctly.");
        Assert.IsTrue(p.Availible == p2.Availible);
        Assert.IsTrue(p.Uuid == p2.Uuid, "Product ids changed.");
    }

    [TestMethod("Delete product")]
    public void DeleteProductTestMethod()
    {
        uint id = productsTemp[1].Uuid;
        ILogger<ProductController> logger = Mock.Of<ILogger<ProductController>>();
        DbContextOptions<ApplicationDataDbContext> options =
            new DbContextOptionsBuilder<ApplicationDataDbContext>().UseInMemoryDatabase("Data.WireDevErpV1").Options;
        Mock<ApplicationDataDbContext> dbcMock = new(options);
        _ = dbcMock.Setup(x => x.Products).ReturnsDbSet(productsTemp);
        ProductController pc = new(dbcMock.Object, logger);

        ObjectResult response = (ObjectResult)pc.DeleteProduct(id).Result;
        Assert.IsNotNull(response);
        Assert.IsTrue(response.StatusCode == StatusCodes.Status200OK, "Status code does not indicate success: " + response.StatusCode);

        Assert.IsNull(dbcMock.Object.Products.Find(id), "Object is still in database.");
    }

    [TestMethod("Fail to change id of product")]
    public void ChangeProductIdTestMethod()
    {
        uint id = productsTemp[0].Uuid;

        ILogger<ProductController> logger = Mock.Of<ILogger<ProductController>>();
        DbContextOptions<ApplicationDataDbContext> options =
            new DbContextOptionsBuilder<ApplicationDataDbContext>().UseInMemoryDatabase("Data.WireDevErpV1").Options;
        Mock<ApplicationDataDbContext> dbcMock = new(options);
        _ = dbcMock.Setup(x => x.Products).ReturnsDbSet(productsTemp);
        ProductController pc = new(dbcMock.Object, logger);

        ObjectResult response = (ObjectResult)pc.ModifyProduct(id, productsTemp[1]).Result;
        Assert.IsNotNull(response);
        Assert.IsTrue(response.StatusCode == StatusCodes.Status200OK, "Status code does not indicate success: " + response.StatusCode);

        Assert.IsNotNull(response.Value, "Data in response is emtpy.");
        Assert.IsInstanceOfType(response.Value, typeof(Product), "Data is not an instance of expected type.");

        Product? p = response.Value as Product;
        Assert.IsTrue(p.Uuid == id, "Product ids changed.");
    }
}