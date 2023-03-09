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
public class GroupControllerTest
{
    public List<Group> groupsTemp { get; } = new()
    {
        new(999) { Name = "Default_Group" },
        new(998) { Name = "Default_Group1" },
        new(997) { Name = "Default_Group2" }
    };

    [TestMethod("Get all groups")]
    public void GetAllGroupsTestMethod()
    {
        ILogger<GroupController> logger = Mock.Of<ILogger<GroupController>>();
        DbContextOptions<ApplicationDataDbContext> options =
            new DbContextOptionsBuilder<ApplicationDataDbContext>().UseInMemoryDatabase("Data.WireDevErpV1").Options;
        Mock<ApplicationDataDbContext> dbcMock = new(options);
        _ = dbcMock.Setup(x => x.Groups).ReturnsDbSet(groupsTemp);
        GroupController pc = new(dbcMock.Object, logger);

        ObjectResult response = (ObjectResult)pc.GetGroups().Result;
        Assert.IsNotNull(response);
        Assert.IsTrue(response.StatusCode == StatusCodes.Status200OK, "Status code does not indicate success: " + response.StatusCode);

        Assert.IsNotNull(response.Value, "Data in response is emtpy.");
        Assert.IsInstanceOfType(response.Value, typeof(List<int>), "Data is not an instance of expected type.");
    }

    [TestMethod("Get single group")]
    public void GetGroupTestMethod()
    {
        int id = 997;
        ILogger<GroupController> logger = Mock.Of<ILogger<GroupController>>();
        DbContextOptions<ApplicationDataDbContext> options =
            new DbContextOptionsBuilder<ApplicationDataDbContext>().UseInMemoryDatabase("Data.WireDevErpV1").Options;
        Mock<ApplicationDataDbContext> dbcMock = new(options);
        _ = dbcMock.Setup(x => x.Groups).ReturnsDbSet(groupsTemp);
        GroupController pc = new(dbcMock.Object, logger);

        ObjectResult response = (ObjectResult)pc.GetGroup(id).Result;
        Assert.IsNotNull(response);
        Assert.IsTrue(response.StatusCode == StatusCodes.Status200OK, "Status code does not indicate success: " + response.StatusCode);

        Assert.IsNotNull(response.Value, "Data in response is emtpy.");
        Assert.IsInstanceOfType(response.Value, typeof(Group), "Data is not an instance of expected type.");
        Assert.IsTrue(((Group)response.Value).Uuid == id, "Group is not as expected.");
    }

    [TestMethod("Create group")]
    public void CreateGroupTestMethod()
    {
        ILogger<GroupController> logger = Mock.Of<ILogger<GroupController>>();
        DbContextOptions<ApplicationDataDbContext> options =
            new DbContextOptionsBuilder<ApplicationDataDbContext>().UseInMemoryDatabase("Data.WireDevErpV1").Options;
        Mock<ApplicationDataDbContext> dbcMock = new(options);
        _ = dbcMock.Setup(x => x.Groups).ReturnsDbSet(new List<Group>());
        GroupController pc = new(dbcMock.Object, logger);
        Group p = new(990) { Name = "Default_Group", };

        ObjectResult response = (ObjectResult)pc.AddGroup(p).Result;
        Assert.IsNotNull(response);
        Assert.IsTrue(response.StatusCode == StatusCodes.Status201Created, "Status code does not indicate success: " + response.StatusCode);

        Assert.IsNotNull(response.Value, "Data in response is emtpy.");
        Assert.IsInstanceOfType(response.Value, typeof(Group), "Data is not an instance of expected type.");
    }

    [TestMethod("Modify group")]
    public void ModifyGroupTestMethod()
    {
        Group g = groupsTemp[0];
        g.Name = "new name";

        ILogger<GroupController> logger = Mock.Of<ILogger<GroupController>>();
        DbContextOptions<ApplicationDataDbContext> options =
            new DbContextOptionsBuilder<ApplicationDataDbContext>().UseInMemoryDatabase("Data.WireDevErpV1").Options;
        Mock<ApplicationDataDbContext> dbcMock = new(options);
        _ = dbcMock.Setup(x => x.Groups).ReturnsDbSet(groupsTemp);
        GroupController pc = new(dbcMock.Object, logger);

        ObjectResult response = (ObjectResult)pc.ModifyGroup(g.Uuid, g).Result;
        Assert.IsNotNull(response);
        Assert.IsTrue(response.StatusCode == StatusCodes.Status200OK, "Status code does not indicate success: " + response.StatusCode);

        Assert.IsNotNull(response.Value, "Data in response is emtpy.");
        Assert.IsInstanceOfType(response.Value, typeof(Group), "Data is not an instance of expected type.");

        Group? p2 = response.Value as Group;
        Assert.IsTrue(g.Name == p2.Name, "Not all properties are changed correctly.");
        Assert.IsTrue(g.Uuid == p2.Uuid, "Group ids changed.");
    }

    [TestMethod("Delete group")]
    public void DeleteGroupTestMethod()
    {
        int id = groupsTemp[1].Uuid;
        ILogger<GroupController> logger = Mock.Of<ILogger<GroupController>>();
        DbContextOptions<ApplicationDataDbContext> options =
            new DbContextOptionsBuilder<ApplicationDataDbContext>().UseInMemoryDatabase("Data.WireDevErpV1").Options;
        Mock<ApplicationDataDbContext> dbcMock = new(options);
        _ = dbcMock.Setup(x => x.Groups).ReturnsDbSet(groupsTemp);
        GroupController pc = new(dbcMock.Object, logger);

        ObjectResult response = (ObjectResult)pc.DeleteGroup(id).Result;
        Assert.IsNotNull(response);
        Assert.IsTrue(response.StatusCode == StatusCodes.Status200OK, "Status code does not indicate success: " + response.StatusCode);

        Assert.IsNull(dbcMock.Object.Groups.Find(id), "Object is still in database.");
    }

    [TestMethod("Fail to change id of group")]
    public void ChangeGroupIdTestMethod()
    {
        int id = groupsTemp[0].Uuid;

        ILogger<GroupController> logger = Mock.Of<ILogger<GroupController>>();
        DbContextOptions<ApplicationDataDbContext> options =
            new DbContextOptionsBuilder<ApplicationDataDbContext>().UseInMemoryDatabase("Data.WireDevErpV1").Options;
        Mock<ApplicationDataDbContext> dbcMock = new(options);
        _ = dbcMock.Setup(x => x.Groups).ReturnsDbSet(groupsTemp);
        GroupController pc = new(dbcMock.Object, logger);

        ObjectResult response = (ObjectResult)pc.ModifyGroup(id, groupsTemp[1]).Result;
        Assert.IsNotNull(response);
        Assert.IsTrue(response.StatusCode == StatusCodes.Status200OK, "Status code does not indicate success: " + response.StatusCode);

        Assert.IsNotNull(response.Value, "Data in response is emtpy.");
        Assert.IsInstanceOfType(response.Value, typeof(Group), "Data is not an instance of expected type.");

        Group? p = response.Value as Group;
        Assert.IsTrue(p.Uuid == id, "Group ids changed.");
    }
}