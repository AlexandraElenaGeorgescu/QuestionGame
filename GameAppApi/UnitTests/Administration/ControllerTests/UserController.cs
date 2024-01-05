using Xunit;
using GameAppApi.UserAdministration.Controllers;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GameAppApi.API.PublicModels;

namespace GameAppApi.Tests.UserAdministration.Controllers
{
    public class UserControllerTests
    {
        private readonly UserController _controller;
        private readonly FakeAdminService _fakeAdminService;

        public UserControllerTests()
        {
            _fakeAdminService = new FakeAdminService();
            _controller = new UserController(_fakeAdminService);
        }

        [Fact]
        public async Task GetAllUsers_ReturnsAllUsers()
        {
            // Arrange
            var user1 = new User { Id = Guid.NewGuid(), Username = "user1" };
            var user2 = new User { Id = Guid.NewGuid(), Username = "user2" };
            _fakeAdminService.AddUser(user1);
            _fakeAdminService.AddUser(user2);

            // Act
            var result = await _controller.GetAllUsers();

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result.Result);
            var users = Assert.IsType<List<User>>(actionResult.Value);
            Assert.Equal(2, users.Count);
        }

        [Fact]
        public async Task DeleteUser_WhenUserExists_RemovesUserAndReturnsNoContent()
        {
            // Arrange
            var user = new User { Id = Guid.NewGuid(), Username = "user1" };
            _fakeAdminService.AddUser(user);

            // Act
            var result = await _controller.DeleteUser(user.Id.ToString());

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteUser_WhenUserDoesNotExist_ReturnsNotFound()
        {
            // Arrange & Act
            var result = await _controller.DeleteUser(Guid.NewGuid().ToString());

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
