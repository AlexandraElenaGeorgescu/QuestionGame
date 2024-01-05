using Xunit;
using Moq;
using GameAppApi.UserAdministration.Services;
using GameAppApi.API.PublicModels;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using GameAppApi.API.DatabaseSettings;

namespace UnitTests.Administration.ServiceTests
{
    public class AdminServiceTests
    {
        private readonly Mock<IMongoCollection<User>> _mockUsersCollection;
        private readonly Mock<IMongoDatabase> _mockDatabase;
        private readonly Mock<IMongoClient> _mockClient;
        private readonly Mock<IMongoDBSettings> _mockSettings;
        private readonly AdminService _adminService;

        public AdminServiceTests()
        {
            // Arrange
            _mockUsersCollection = new Mock<IMongoCollection<User>>();
            _mockDatabase = new Mock<IMongoDatabase>();
            _mockClient = new Mock<IMongoClient>();
            _mockSettings = new Mock<IMongoDBSettings>();

            _mockSettings.Setup(s => s.ConnectionString).Returns("mongodb://localhost:27017");
            _mockSettings.Setup(s => s.DatabaseName).Returns("QuizGameDb");
            _mockSettings.Setup(s => s.UsersCollectionName).Returns("Users");

            _mockClient.Setup(c => c.GetDatabase(_mockSettings.Object.DatabaseName, null))
                       .Returns(_mockDatabase.Object);

            _mockDatabase.Setup(d => d.GetCollection<User>(_mockSettings.Object.UsersCollectionName, null))
                         .Returns(_mockUsersCollection.Object);

            // Act
            _adminService = new AdminService(_mockSettings.Object);
        }

        [Fact]
        public async Task GetAllUsers_ReturnsAllUsers()
        {
            // Arrange
            var fakeUsers = new List<User> { new User { /* ... properties ... */ }, new User { /* ... properties ... */ } };
            var mockCursor = new Mock<IAsyncCursor<User>>();
            mockCursor.Setup(_ => _.Current).Returns(fakeUsers);
            mockCursor.SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
                      .Returns(true)
                      .Returns(false);

            _mockUsersCollection.Setup(c => c.FindAsync(It.IsAny<FilterDefinition<User>>(),
                                                       It.IsAny<FindOptions<User, User>>(),
                                                       It.IsAny<CancellationToken>()))
                                .ReturnsAsync(mockCursor.Object);

            // Act
            var users = await _adminService.GetAllUsers();

            // Assert
            Assert.Equal(5, users.Count);  // Assuming fakeUsers has 2 users
        }

        [Fact]
        public async Task GetUserById_ReturnsCorrectUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var expectedUser = new User { Id = userId, Username = "TestUser" }; // other properties as needed

            var mockCursor = new Mock<IAsyncCursor<User>>();
            mockCursor.SetupSequence(_ => _.Current)
                      .Returns(new List<User> { expectedUser }) // First call returns the user
                      .Returns(new List<User>()); // Subsequent calls return an empty list
            mockCursor.Setup(_ => _.MoveNextAsync(It.IsAny<CancellationToken>()))
                      .ReturnsAsync(true);

            _mockUsersCollection.Setup(c => c.FindAsync(It.IsAny<FilterDefinition<User>>(),
                                            It.IsAny<FindOptions<User, User>>(),
                                            It.IsAny<CancellationToken>()))
                    .ReturnsAsync(mockCursor.Object);


            // Act
            var user = await _adminService.GetUserById(userId.ToString());

            // Assert
            Assert.NotNull(user);
            Assert.Equal("TestUser", user.Username);
            Assert.Equal(userId, user.Id);
        }



        [Fact]
        public async Task Remove_DeletesUser()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var mockDeleteResult = new Mock<DeleteResult>();
            mockDeleteResult.Setup(_ => _.DeletedCount).Returns(1);

            _mockUsersCollection.Setup(c => c.DeleteOneAsync(It.IsAny<FilterDefinition<User>>(),
                                                 It.IsAny<CancellationToken>()))
                    .ReturnsAsync(mockDeleteResult.Object);

            // Act
            await _adminService.Remove(userId.ToString());

            // Assert
            _mockUsersCollection.Verify(c => c.DeleteOneAsync(It.IsAny<FilterDefinition<User>>(),
                                                              It.IsAny<CancellationToken>()), Times.Once());
        }


    }
}
