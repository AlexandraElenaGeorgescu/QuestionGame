using Moq;
using Xunit;
using GameAppApi.Authentification.Models;
using GameAppApi.Authentification.PublicServices;
using GameAppApi.Authentification.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using GameAppApi.API.PublicModels;

public class AuthControllerTests
{
    [Fact]
    public async Task Register_UserDoesNotExist_ReturnsOk()
    {
        // Arrange
        var mockService = new Mock<IUserService>();
        mockService.Setup(s => s.GetUserByUsername(It.IsAny<string>())).Returns<User>(null);
        mockService.Setup(s => s.CreateUser(It.IsAny<User>())).ReturnsAsync(new User { /* ... */ });

        var controller = new AuthController(mockService.Object);
        var newUser = new User { /* ... */ };

        // Act
        var result = await controller.Register(newUser);

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Register_UserExists_ReturnsBadRequest()
    {
        // Arrange
        var existingUser = new User { /* ... */ };
        var mockService = new Mock<IUserService>();
        mockService.Setup(s => s.GetUserByUsername(It.IsAny<string>())).Returns(existingUser);

        var controller = new AuthController(mockService.Object);
        var newUser = new User { /* ... */ };

        // Act
        var result = await controller.Register(newUser);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void Login_ValidCredentials_ReturnsOk()
    {
        // Arrange
        var user = new User { Username = "test", Password = "password", Role = Role.Player };
        var mockService = new Mock<IUserService>();
        mockService.Setup(s => s.GetUserByUsername("test")).Returns(user);

        var controller = new AuthController(mockService.Object);
        var loginUser = new User { Username = "test", Password = "password" };

        // Act
        var result = controller.Login(loginUser);

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void Login_InvalidCredentials_ReturnsUnauthorized()
    {
        // Arrange
        var mockService = new Mock<IUserService>();
        mockService.Setup(s => s.GetUserByUsername(It.IsAny<string>())).Returns<User>(null);

        var controller = new AuthController(mockService.Object);
        var loginUser = new User { Username = "test", Password = "wrongpassword" };

        // Act
        var result = controller.Login(loginUser);

        // Assert
        Assert.IsType<UnauthorizedObjectResult>(result);
    }
}
