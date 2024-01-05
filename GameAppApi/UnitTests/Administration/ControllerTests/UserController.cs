using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameAppApi.UserAdministration.Controllers;
using GameAppApi.UserAdministration.Services;
using GameAppApi.API.PublicModels;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;

namespace GameAppApi.Tests.UserAdministration.Controllers
{
    public class UserControllerTests
    {
        private readonly Mock<AdminService> _mockAdminService;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            // Mock the dependencies
            _mockAdminService = new Mock<AdminService>();
            // Instantiate the controller with the mocked service
            _controller = new UserController(_mockAdminService.Object);
        }

        [Fact]
        public async Task GetAllUsers_ReturnsAllUsers()
        {
            // Arrange
            var fakeUsers = new List<User>
            {
                new User { Id = Guid.NewGuid(), Username = "user1", Role = "Player" },
                new User { Id = Guid.NewGuid(), Username = "user2", Role = "Moderator" }
            };
            _mockAdminService.Setup(service => service.GetAllUsers()).ReturnsAsync(fakeUsers);

            // Act
            var result = await _controller.GetAllUsers();

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<User>>(actionResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task DeleteUser_WhenUserExists_RemovesUserAndReturnsNoContent()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var fakeUser = new User { Id = userId, Username = "user1", Role = "Player" };
            _mockAdminService.Setup(service => service.GetUserById(userId.ToString())).ReturnsAsync(fakeUser);
            _mockAdminService.Setup(service => service.Remove(userId.ToString())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteUser(userId.ToString());

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mockAdminService.Verify(service => service.Remove(userId.ToString()), Times.Once());
        }

        [Fact]
        public async Task DeleteUser_WhenUserDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            _mockAdminService.Setup(service => service.GetUserById(It.IsAny<string>())).ReturnsAsync((User)null);

            // Act
            var result = await _controller.DeleteUser("nonexistent-id");

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
