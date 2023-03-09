using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.EntityFrameworkCore;
using WireDev.Erp.V1.Api.Context;
using WireDev.Erp.V1.Api.Controllers;
using WireDev.Erp.V1.Models.Statistics;

namespace WireDev.Erp.V1.Api.Test
{
    [TestClass]
    public class StatsControllerTest
	{
        List<YearStats> yearsTemp = new()
        {
            new YearStats(new DateTime(2020, 1, 1)),
            new YearStats(new DateTime(2021, 1, 1)),
            new YearStats(new DateTime(2022, 1, 1)),
            new YearStats(new DateTime(2023, 1, 1)),
        };

        List<MonthStats> monthTemp = new()
        {
            new MonthStats(new DateTime(2020, 1, 1)),
            new MonthStats(new DateTime(2021, 1, 1)),
            new MonthStats(new DateTime(2022, 1, 1)),
            new MonthStats(new DateTime(2023, 1, 1)),
        };

        List<DayStats> dayTemp = new()
        {
            new DayStats(new DateTime(2020, 1, 1)),
            new DayStats(new DateTime(2021, 1, 1)),
            new DayStats(new DateTime(2022, 1, 1)),
            new DayStats(new DateTime(2023, 1, 1)),
        };

        List<TotalStats> totalTemp = new()
        {
            new TotalStats(new DateTime(2020, 1, 1)),
        };

        List<ProductStats> productTemp = new()
        {
            new ProductStats(9997),
        };


        [TestMethod("Get all total stats")]
        public void GetTotalTestMethod()
        {
            ILogger<StatsController> logger = Mock.Of<ILogger<StatsController>>();
            DbContextOptions<ApplicationDataDbContext> options =
                new DbContextOptionsBuilder<ApplicationDataDbContext>().UseInMemoryDatabase("Data.WireDevErpV1").Options;
            Mock<ApplicationDataDbContext> dbcMock = new(options);
            _ = dbcMock.Setup(x => x.TotalStats).ReturnsDbSet(totalTemp);
            StatsController sc = new(dbcMock.Object, logger);

            ObjectResult response = (ObjectResult)sc.GetTotalStats().Result;
            Assert.IsNotNull(response);
            Assert.IsTrue(response.StatusCode == StatusCodes.Status200OK, "Status code does not indicate success: " + response.StatusCode);

            Assert.IsNotNull(response.Value, "Data in response is emtpy.");
            Assert.IsInstanceOfType(response.Value, typeof(List<TotalStats>), "Data is not an instance of expected type.");
        }

        [TestMethod("Get all years")]
        public void GetYearsTestMethod()
        {
            ILogger<StatsController> logger = Mock.Of<ILogger<StatsController>>();
            DbContextOptions<ApplicationDataDbContext> options =
                new DbContextOptionsBuilder<ApplicationDataDbContext>().UseInMemoryDatabase("Data.WireDevErpV1").Options;
            Mock<ApplicationDataDbContext> dbcMock = new(options);
            _ = dbcMock.Setup(x => x.YearStats).ReturnsDbSet(yearsTemp);
            StatsController sc = new(dbcMock.Object, logger);

            ObjectResult response = (ObjectResult)sc.GetAllYearStats().Result;
            Assert.IsNotNull(response);
            Assert.IsTrue(response.StatusCode == StatusCodes.Status200OK, "Status code does not indicate success: " + response.StatusCode);

            Assert.IsNotNull(response.Value, "Data in response is emtpy.");
            Assert.IsInstanceOfType(response.Value, typeof(List<ushort>), "Data is not an instance of expected type.");
        }

        [TestMethod("Get single year")]
        public void GetYearTestMethod()
        {
            ushort id = 2021;
            ILogger<StatsController> logger = Mock.Of<ILogger<StatsController>>();
            DbContextOptions<ApplicationDataDbContext> options =
                new DbContextOptionsBuilder<ApplicationDataDbContext>().UseInMemoryDatabase("Data.WireDevErpV1").Options;
            Mock<ApplicationDataDbContext> dbcMock = new(options);
            _ = dbcMock.Setup(x => x.YearStats).ReturnsDbSet(yearsTemp);
            StatsController sc = new(dbcMock.Object, logger);

            ObjectResult response = (ObjectResult)sc.GetYearStats(id).Result;
            Assert.IsNotNull(response);
            Assert.IsTrue(response.StatusCode == StatusCodes.Status200OK, "Status code does not indicate success: " + response.StatusCode);

            Assert.IsNotNull(response.Value, "Data in response is emtpy.");
            Assert.IsInstanceOfType(response.Value, typeof(YearStats), "Data is not an instance of expected type.");
            Assert.IsTrue(((YearStats)response.Value).GetDate().Year == id, "Wrong data was provided.");
        }

        [TestMethod("Get all months")]
        public void GetMonthsTestMethod()
        {
            ILogger<StatsController> logger = Mock.Of<ILogger<StatsController>>();
            DbContextOptions<ApplicationDataDbContext> options =
                new DbContextOptionsBuilder<ApplicationDataDbContext>().UseInMemoryDatabase("Data.WireDevErpV1").Options;
            Mock<ApplicationDataDbContext> dbcMock = new(options);
            _ = dbcMock.Setup(x => x.MonthStats).ReturnsDbSet(monthTemp);
            StatsController sc = new(dbcMock.Object, logger);

            ObjectResult response = (ObjectResult)sc.GetAllMonthStats().Result;
            Assert.IsNotNull(response);
            Assert.IsTrue(response.StatusCode == StatusCodes.Status200OK, "Status code does not indicate success: " + response.StatusCode);

            Assert.IsNotNull(response.Value, "Data in response is emtpy.");
            Assert.IsInstanceOfType(response.Value, typeof(List<DateTime>), "Data is not an instance of expected type.");
        }

        [TestMethod("Get single month")]
        public void GetMonthTestMethod()
        {
            DateTime time = new DateTime(2020, 1, 1);
            ILogger<StatsController> logger = Mock.Of<ILogger<StatsController>>();
            DbContextOptions<ApplicationDataDbContext> options =
                new DbContextOptionsBuilder<ApplicationDataDbContext>().UseInMemoryDatabase("Data.WireDevErpV1").Options;
            Mock<ApplicationDataDbContext> dbcMock = new(options);
            _ = dbcMock.Setup(x => x.MonthStats).ReturnsDbSet(monthTemp);
            StatsController sc = new(dbcMock.Object, logger);

            ObjectResult response = (ObjectResult)sc.GetMonthStats(((ushort)time.Year), ((ushort)time.Month)).Result;
            Assert.IsNotNull(response);
            Assert.IsTrue(response.StatusCode == StatusCodes.Status200OK, "Status code does not indicate success: " + response.StatusCode);

            Assert.IsNotNull(response.Value, "Data in response is emtpy.");
            Assert.IsInstanceOfType(response.Value, typeof(MonthStats), "Data is not an instance of expected type.");
            Assert.IsTrue(((MonthStats)response.Value).GetDate() == time, "Wrong data was provided.");
        }

        [TestMethod("Get all days")]
        public void GetDaysTestMethod()
        {
            ILogger<StatsController> logger = Mock.Of<ILogger<StatsController>>();
            DbContextOptions<ApplicationDataDbContext> options =
                new DbContextOptionsBuilder<ApplicationDataDbContext>().UseInMemoryDatabase("Data.WireDevErpV1").Options;
            Mock<ApplicationDataDbContext> dbcMock = new(options);
            _ = dbcMock.Setup(x => x.DayStats).ReturnsDbSet(dayTemp);
            StatsController sc = new(dbcMock.Object, logger);

            ObjectResult response = (ObjectResult)sc.GetAllDayStats().Result;
            Assert.IsNotNull(response);
            Assert.IsTrue(response.StatusCode == StatusCodes.Status200OK, "Status code does not indicate success: " + response.StatusCode);

            Assert.IsNotNull(response.Value, "Data in response is emtpy.");
            Assert.IsInstanceOfType(response.Value, typeof(List<DateTime>), "Data is not an instance of expected type.");
        }

        [TestMethod("Get single day")]
        public void GetDayTestMethod()
        {
            DateTime time = new DateTime(2020, 1, 1);
            ILogger<StatsController> logger = Mock.Of<ILogger<StatsController>>();
            DbContextOptions<ApplicationDataDbContext> options =
                new DbContextOptionsBuilder<ApplicationDataDbContext>().UseInMemoryDatabase("Data.WireDevErpV1").Options;
            Mock<ApplicationDataDbContext> dbcMock = new(options);
            _ = dbcMock.Setup(x => x.DayStats).ReturnsDbSet(dayTemp);
            StatsController sc = new(dbcMock.Object, logger);

            ObjectResult response = (ObjectResult)sc.GetDayStats(((ushort)time.Year), ((ushort)time.Month), ((ushort)time.Day)).Result;
            Assert.IsNotNull(response);
            Assert.IsTrue(response.StatusCode == StatusCodes.Status200OK, "Status code does not indicate success: " + response.StatusCode);

            Assert.IsNotNull(response.Value, "Data in response is emtpy.");
            Assert.IsInstanceOfType(response.Value, typeof(DayStats), "Data is not an instance of expected type.");
            Assert.IsTrue(((DayStats)response.Value).GetDate() == time, "Wrong data was provided.");
        }

        [TestMethod("Get product stats")]
        public void GetProductStatsTestMethod()
        {
            uint id = 9997;
            ILogger<StatsController> logger = Mock.Of<ILogger<StatsController>>();
            DbContextOptions<ApplicationDataDbContext> options =
                new DbContextOptionsBuilder<ApplicationDataDbContext>().UseInMemoryDatabase("Data.WireDevErpV1").Options;
            Mock<ApplicationDataDbContext> dbcMock = new(options);
            _ = dbcMock.Setup(x => x.ProductStats).ReturnsDbSet(productTemp);
            StatsController sc = new(dbcMock.Object, logger);

            ObjectResult response = (ObjectResult)sc.GetProductStats(id).Result;
            Assert.IsNotNull(response);
            Assert.IsTrue(response.StatusCode == StatusCodes.Status200OK, "Status code does not indicate success: " + response.StatusCode);

            Assert.IsNotNull(response.Value, "Data in response is emtpy.");
            Assert.IsInstanceOfType(response.Value, typeof(ProductStats), "Data is not an instance of expected type.");
            Assert.IsTrue(((ProductStats)response.Value).ProductId == id, "Wrong data was provided.");
        }
    }
}