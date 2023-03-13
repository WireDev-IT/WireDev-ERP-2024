using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.EntityFrameworkCore;
using WireDev.Erp.V1.Api.Context;
using WireDev.Erp.V1.Api.Controllers;
using WireDev.Erp.V1.Models.Statistics;
using WireDev.Erp.V1.Models.Storage;

namespace WireDev.Erp.V1.Api.Test
{
    [TestClass]
    public class PurchaseControllerTest
    {
        public static List<Product> productsTemp { get; } = new()
    {
        new(9999) { Name = "Default_Product", Prices = new() { Guid.NewGuid() } },
        new(9998) { Name = "Default_Product1", Prices = new() { Guid.NewGuid() } },
        new(9997) { Name = "Default_Product2", Prices = new() { Guid.NewGuid() } }
    };

        public static List<Purchase> sampleData
        {
            get
            {
                List<Price> pricesTemp = new()
                {
                    new(Guid.NewGuid()) { Description = "Price1", RetailValue = 50, SellValue = 100 },
                    new(Guid.NewGuid()) { Description = "Price2", RetailValue = 13, SellValue = 46 }
                };

                List<Purchase> purchaseTemp = new()
                {
                    new() { Type = Models.Enums.TransactionType.Sell },
                    new() { Type = Models.Enums.TransactionType.Refund },
                    new() { Type = Models.Enums.TransactionType.Purchase }
                };

                purchaseTemp[0].TryAddItem(9999, pricesTemp[0], 2);
                purchaseTemp[0].TryAddItem(9998, pricesTemp[1], 36);

                purchaseTemp[1].TryAddItem(9999, pricesTemp[0], 1);
                purchaseTemp[1].TryAddItem(9998, pricesTemp[1], 6);

                purchaseTemp[2].TryAddItem(9999, pricesTemp[0], 10);
                purchaseTemp[2].TryAddItem(9998, pricesTemp[1], 45);

                return purchaseTemp;
            }
        }

        [TestMethod("Get all purchases")]
        public void GetAllPurchasesTestMethod()
        {
            ILogger<PurchaseController> logger = Mock.Of<ILogger<PurchaseController>>();
            DbContextOptions<ApplicationDataDbContext> options =
                new DbContextOptionsBuilder<ApplicationDataDbContext>().UseInMemoryDatabase("Data.WireDevErpV1").Options;
            Mock<ApplicationDataDbContext> dbcMock = new(options);
            _ = dbcMock.Setup(x => x.Purchases).ReturnsDbSet(sampleData);
            PurchaseController pc = new(dbcMock.Object, logger);

            ObjectResult response = (ObjectResult)pc.GetPurchases().Result;
            Assert.IsNotNull(response);
            Assert.IsTrue(response.StatusCode == StatusCodes.Status200OK, "Status code does not indicate success: " + response.StatusCode);

            Assert.IsNotNull(response.Value, "Data in response is emtpy.");
            Assert.IsInstanceOfType(response.Value, typeof(List<Guid>), "Data is not an instance of expected type.");
        }

        [TestMethod("Get single purchase")]
        public void GetPurchaseTestMethod()
        {
            Guid id = sampleData[0].Uuid;
            ILogger<PurchaseController> logger = Mock.Of<ILogger<PurchaseController>>();
            DbContextOptions<ApplicationDataDbContext> options =
                new DbContextOptionsBuilder<ApplicationDataDbContext>().UseInMemoryDatabase("Data.WireDevErpV1").Options;
            Mock<ApplicationDataDbContext> dbcMock = new(options);
            _ = dbcMock.Setup(x => x.Purchases).ReturnsDbSet(sampleData);
            PurchaseController pc = new(dbcMock.Object, logger);

            ObjectResult response = (ObjectResult)pc.GetPurchase(id).Result;
            Assert.IsNotNull(response);
            Assert.IsTrue(response.StatusCode == StatusCodes.Status200OK, "Status code does not indicate success: " + response.StatusCode);

            Assert.IsNotNull(response.Value, "Data in response is emtpy.");
            Assert.IsInstanceOfType(response.Value, typeof(Purchase), "Data is not an instance of expected type.");
            Assert.IsTrue(((Purchase)response.Value).Uuid == id, "Product is not as expected.");
        }

        //[TestMethod("Sell purchase")]
        //public void SellPurchaseTestMethod()
        //{
        //    ILogger<PurchaseController> logger = Mock.Of<ILogger<PurchaseController>>();
        //    DbContextOptions<ApplicationDataDbContext> options =
        //        new DbContextOptionsBuilder<ApplicationDataDbContext>().UseInMemoryDatabase("Data.WireDevErpV1").Options;
        //    Mock<ApplicationDataDbContext> dbcMock = new(options);
        //    _ = dbcMock.Setup(x => x.Database).Returns(new DatabaseFacade(dbcMock.Object));
        //    _ = dbcMock.Setup(x => x.Products).ReturnsDbSet(productsTemp);
        //    _ = dbcMock.Setup(x => x.Purchases).ReturnsDbSet(new List<Purchase>());
        //    _ = dbcMock.Setup(x => x.TotalStats).ReturnsDbSet(new List<TotalStats>());
        //    _ = dbcMock.Setup(x => x.YearStats).ReturnsDbSet(new List<YearStats>());
        //    _ = dbcMock.Setup(x => x.MonthStats).ReturnsDbSet(new List<MonthStats>());
        //    _ = dbcMock.Setup(x => x.DayStats).ReturnsDbSet(new List<DayStats>());
        //    PurchaseController pc = new(dbcMock.Object, logger);
        //    Purchase p = sampleData[0];

        //    ObjectResult response = (ObjectResult)pc.SellPurchase(p).Result;
        //    Assert.IsNotNull(response);
        //    Assert.IsTrue(response.StatusCode == StatusCodes.Status200OK, "Status code does not indicate success: " + response.StatusCode);

        //    Assert.IsNotNull(response.Value, "Data in response is emtpy.");
        //    Assert.IsInstanceOfType(response.Value, typeof(Product), "Data is not an instance of expected type.");
        //}
    }
}