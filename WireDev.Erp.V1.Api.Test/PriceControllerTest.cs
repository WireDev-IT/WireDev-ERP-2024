using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.EntityFrameworkCore;
using WireDev.Erp.V1.Api.Context;
using WireDev.Erp.V1.Api.Controllers;
using WireDev.Erp.V1.Models.Storage;

namespace WireDev.Erp.V1.Api.Test;

[TestClass]
public class PriceControllerTest
{
    public List<Price> pricesTemp { get; } = new()
    {
        new(Guid.NewGuid()) { Description = "Price1", RetailValue = 50, SellValue = 100 },
        new(Guid.NewGuid()) { Description = "Price2", RetailValue = 13, SellValue = 46 },
        new(Guid.NewGuid()) { Description = "Price3", RetailValue = 69, SellValue = 187 },
    };

    [TestMethod("Get all price")]
    public void GetAllPricesTestMethod()
    {
        ILogger<PriceController> logger = Mock.Of<ILogger<PriceController>>();
        DbContextOptions<ApplicationDataDbContext> options =
            new DbContextOptionsBuilder<ApplicationDataDbContext>().UseInMemoryDatabase("Data.WireDevErpV1").Options;
        Mock<ApplicationDataDbContext> dbcMock = new(options);
        _ = dbcMock.Setup(x => x.Prices).ReturnsDbSet(pricesTemp);
        PriceController pc = new(dbcMock.Object, logger);

        ObjectResult response = (ObjectResult)pc.GetPrices().Result;
        Assert.IsNotNull(response);
        Assert.IsTrue(response.StatusCode == StatusCodes.Status200OK, "Status code does not indicate success: " + response.StatusCode);


        Assert.IsNotNull(response.Value, "Data in response is emtpy.");
        Assert.IsInstanceOfType(response.Value, typeof(List<Guid>), "Data is not an instance of expected type.");
    }

    [TestMethod("Get single price")]
    public void GetPriceTestMethod()
    {
        Guid id = pricesTemp[1].Uuid;
        ILogger<PriceController> logger = Mock.Of<ILogger<PriceController>>();
        DbContextOptions<ApplicationDataDbContext> options =
            new DbContextOptionsBuilder<ApplicationDataDbContext>().UseInMemoryDatabase("Data.WireDevErpV1").Options;
        Mock<ApplicationDataDbContext> dbcMock = new(options);
        _ = dbcMock.Setup(x => x.Prices).ReturnsDbSet(pricesTemp);
        PriceController pc = new(dbcMock.Object, logger);

        ObjectResult response = (ObjectResult)pc.GetPrice(id).Result;
        Assert.IsNotNull(response);
        Assert.IsTrue(response.StatusCode == StatusCodes.Status200OK, "Status code does not indicate success: " + response.StatusCode);

        Assert.IsNotNull(response.Value, "Data in response is emtpy.");
        Assert.IsInstanceOfType(response.Value, typeof(Price), "Data is not an instance of expected type.");
        Assert.IsTrue(((Price)response.Value).Uuid == id, "Price is not as expected.");
    }

    [TestMethod("Create price")]
    public void CreatePriceTestMethod()
    {
        ILogger<PriceController> logger = Mock.Of<ILogger<PriceController>>();
        DbContextOptions<ApplicationDataDbContext> options =
            new DbContextOptionsBuilder<ApplicationDataDbContext>().UseInMemoryDatabase("Data.WireDevErpV1").Options;
        Mock<ApplicationDataDbContext> dbcMock = new(options);
        _ = dbcMock.Setup(x => x.Prices).ReturnsDbSet(new List<Price>());
        PriceController pc = new(dbcMock.Object, logger);
        Price p = new(Guid.NewGuid()) { Description = "Default_Price", RetailValue = 1, SellValue = 2 };

        ObjectResult response = (ObjectResult)pc.AddPrice(p).Result;
        Assert.IsNotNull(response);
        Assert.IsTrue(response.StatusCode == StatusCodes.Status201Created, "Status code does not indicate success: " + response.StatusCode);

        Assert.IsNotNull(response.Value, "Data in response is emtpy.");
        Assert.IsInstanceOfType(response.Value, typeof(Price), "Data is not an instance of expected type.");
    }

    [TestMethod("Modify price")]
    public void ModifyPriceTestMethod()
    {
        Price p = pricesTemp[0];
        p.Description = "new name";
        p.RetailValue = 0.5m;

        ILogger<PriceController> logger = Mock.Of<ILogger<PriceController>>();
        DbContextOptions<ApplicationDataDbContext> options =
            new DbContextOptionsBuilder<ApplicationDataDbContext>().UseInMemoryDatabase("Data.WireDevErpV1").Options;
        Mock<ApplicationDataDbContext> dbcMock = new(options);
        _ = dbcMock.Setup(x => x.Prices).ReturnsDbSet(pricesTemp);
        PriceController pc = new(dbcMock.Object, logger);

        ObjectResult response = (ObjectResult)pc.ModifyPrice(p.Uuid, p).Result;
        Assert.IsNotNull(response);
        Assert.IsTrue(response.StatusCode == StatusCodes.Status200OK, "Status code does not indicate success: " + response.StatusCode);

        Assert.IsNotNull(response.Value, "Data in response is emtpy.");
        Assert.IsInstanceOfType(response.Value, typeof(Price), "Data is not an instance of expected type.");

        Price? p2 = response.Value as Price;
        Assert.IsTrue(p.Description == p2.Description, "Not all properties are changed correctly.");
        Assert.IsTrue(p.RetailValue == p2.RetailValue);
        Assert.IsTrue(p.Uuid == p2.Uuid, "Price ids changed.");
    }

    [TestMethod("Delete price")]
    public void DeletePriceTestMethod()
    {
        Guid id = pricesTemp[1].Uuid;
        ILogger<PriceController> logger = Mock.Of<ILogger<PriceController>>();
        DbContextOptions<ApplicationDataDbContext> options =
            new DbContextOptionsBuilder<ApplicationDataDbContext>().UseInMemoryDatabase("Data.WireDevErpV1").Options;
        Mock<ApplicationDataDbContext> dbcMock = new(options);
        _ = dbcMock.Setup(x => x.Prices).ReturnsDbSet(pricesTemp);
        PriceController pc = new(dbcMock.Object, logger);

        ObjectResult response = (ObjectResult)pc.DeletePrice(id).Result;
        Assert.IsNotNull(response);
        Assert.IsTrue(response.StatusCode == StatusCodes.Status200OK, "Status code does not indicate success: " + response.StatusCode);

        Assert.IsNull(dbcMock.Object.Prices.Find(id), "Object is still in database.");
    }

    [TestMethod("Fail to change id of price")]
    public void ChangePriceIdTestMethod()
    {
        Guid id = pricesTemp[0].Uuid;

        ILogger<PriceController> logger = Mock.Of<ILogger<PriceController>>();
        DbContextOptions<ApplicationDataDbContext> options =
            new DbContextOptionsBuilder<ApplicationDataDbContext>().UseInMemoryDatabase("Data.WireDevErpV1").Options;
        Mock<ApplicationDataDbContext> dbcMock = new(options);
        _ = dbcMock.Setup(x => x.Prices).ReturnsDbSet(pricesTemp);
        PriceController pc = new(dbcMock.Object, logger);

        ObjectResult response = (ObjectResult)pc.ModifyPrice(id, pricesTemp[1]).Result;
        Assert.IsNotNull(response);
        Assert.IsTrue(response.StatusCode == StatusCodes.Status200OK, "Status code does not indicate success: " + response.StatusCode);

        Assert.IsNotNull(response.Value, "Data in response is emtpy.");
        Assert.IsInstanceOfType(response.Value, typeof(Price), "Data is not an instance of expected type.");

        Price? p = response.Value as Price;
        Assert.IsTrue(p.Uuid == id, "Price ids changed.");
    }
}