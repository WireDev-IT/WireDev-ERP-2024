using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using WireDev.Erp.V1.Api.Controllers;
using WireDev.Erp.V1.Models.Authentication;

namespace WireDev.Erp.V1.Api.Test
{
    [TestClass]
    public class AuthenticateControllerTest
    {
        private readonly List<IdentityUser> users = new();

        [TestMethod("Login")]
        public void LoginTestMethod()
        {
            Mock<UserManager<IdentityUser>> uManager = new(new Mock<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);
            Mock<RoleManager<IdentityRole>> rManager = new(new Mock<IRoleStore<IdentityRole>>(), null, null, null, null, null, null, null, null);
            Mock<IConfiguration> config = new();
            _ = uManager.Setup(userManager => userManager.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new IdentityUser { UserName = "huhn", PasswordHash = "huhn".GetHashCode().ToString() });
            _ = uManager.Setup(userManager => userManager.IsInRoleAsync(It.IsAny<IdentityUser>(), "Admin")).ReturnsAsync(true);

            AuthenticateController ac = new(uManager.Object, rManager.Object, config.Object);
            LoginModel loginModel = new() { Username = "huhn", Password = "huhn" };

            ObjectResult response = (ObjectResult)ac.Login(loginModel).Result;
            Assert.IsNotNull(response);
            Assert.IsTrue(response.StatusCode == StatusCodes.Status200OK, "Status code does not indicate success: " + response.StatusCode);

            Assert.IsNotNull(response.Value, "Data in response is emtpy.");
            Assert.IsInstanceOfType(response.Value, typeof(JwtSecurityToken), "Data is not an instance of expected type.");
        }
    }
}