using Moq;
using Xunit;
using GameAppApi.Game.Models;
using GameAppApi.Game.Services;
using GameAppApi.Game.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

public class GameControllerTests
{
    private readonly Mock<IGameService> _mockGameService;
    private readonly GameController _controller;

    public GameControllerTests()
    {
        _mockGameService = new Mock<IGameService>();
        _controller = new GameController(_mockGameService.Object);
    }

    [Fact]
    public async Task GetScore_UserExists_ReturnsGameObj()
    {
        // Arrange
        var username = "testuser";
        var gameObj = new GameObj { /* ... */ };
        _mockGameService.Setup(service => service.Get(username)).ReturnsAsync(gameObj);

        // Act
        var result = await _controller.GetScore(username);

        // Assert
        Assert.IsType<ActionResult<GameObj>>(result);
    }

    [Fact]
    public async Task GetScore_UserDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        _mockGameService.Setup(service => service.Get(It.IsAny<string>())).ReturnsAsync((GameObj)null);

        // Act
        var result = await _controller.GetScore("nonexistentuser");

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }


    [Fact]
    public async Task SubmitScore_ValidGame_ReturnsCreatedAtRouteResult()
    {
        // Arrange
        var gameObj = new GameObj { /* ... */ };
        _mockGameService.Setup(service => service.Create(It.IsAny<GameObj>())).ReturnsAsync(gameObj);

        // Act
        var result = await _controller.SubmitScore(gameObj);

        // Assert
        Assert.IsType<CreatedAtRouteResult>(result.Result);
    }
}
