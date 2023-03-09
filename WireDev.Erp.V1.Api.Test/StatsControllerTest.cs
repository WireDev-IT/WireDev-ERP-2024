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
    }
}