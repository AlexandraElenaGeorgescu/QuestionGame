using GameAppApi.API.DatabaseSettings;
using GameAppApi.Game.Models;
using GameAppApi.Game.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace GameAppApi.Tests
{
    public class GameServiceTests
    {
        private Mock<IMongoDBSettings> _settingsMock;
        private Mock<IQuestionService> _questionServiceMock;
        private Mock<IMongoCollection<GameObj>> _gamesCollectionMock;
        private GameService _gameService;

        public GameServiceTests()
        {
            _settingsMock = new Mock<IMongoDBSettings>();
            _questionServiceMock = new Mock<IQuestionService>();
            _gamesCollectionMock = new Mock<IMongoCollection<GameObj>>();

            var clientMock = new Mock<IMongoClient>();
            clientMock.Setup(c => c.GetDatabase(It.IsAny<string>())).Returns(null);

            _settingsMock.SetupGet(s => s.ConnectionString).Returns("connectionString");
            _settingsMock.SetupGet(s => s.DatabaseName).Returns("databaseName");
            _settingsMock.SetupGet(s => s.GamesCollectionName).Returns("gamesCollectionName");

            _gameService = new GameService(_settingsMock.Object, _questionServiceMock.Object);
        }

        [Fact]
        public async Task EnsureGameExists_ShouldCreateNewGame_WhenGameDoesNotExist()
        {
            // Arrange
            string username = "newUser";
            GameObj expectedGame = new GameObj { Username = username, Score = 0, CurrentQuestionIndex = 0 };
            _gamesCollectionMock
                .Setup(c => c.Find<GameObj>(It.IsAny<FilterDefinition<GameObj>>(), It.IsAny<FindOptions<GameObj, GameObj>>()))
                .ReturnsAsync((GameObj)null);
            _gamesCollectionMock
                .Setup(c => c.InsertOneAsync(expectedGame, null, default))
                .Returns(Task.CompletedTask);
            _questionServiceMock
                .Setup(q => q.GetQuestionByIndex(It.IsAny<int>()))
                .ReturnsAsync(new Question());

            // Act
            var result = await _gameService.EnsureGameExists(username);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(username, result.Username);
            Assert.Equal(0, result.Score);
            Assert.Equal(0, result.CurrentQuestionIndex);
        }


        [Fact]
        public async Task StartNewGame_ShouldResetExistingGame_WhenGameExists()
        {
            string username = "existingUser";
            GameObj existingGame = new GameObj { Username = username, Score = 2, CurrentQuestionIndex = 3 };
            GameObj expectedGame = new GameObj { Username = username, Score = 0, CurrentQuestionIndex = 0 };

            _gamesCollectionMock.Setup(c => c.Find<GameObj>(It.IsAny<Func<FilterDefinition<GameObj>, FindOptions<GameObj, GameObj>>>()))
                .ReturnsAsync(existingGame);
            _gamesCollectionMock.Setup(c => c.ReplaceOneAsync(It.IsAny<FilterDefinition<GameObj>>(), It.IsAny<GameObj>(), null, default)).Returns(Task.CompletedTask);
            _questionServiceMock.Setup(q => q.GetQuestionByIndex(It.IsAny<int>())).ReturnsAsync(new Question());

            var result = await _gameService.StartNewGame(username);

            Assert.NotNull(result);
            Assert.Equal(username, result.Username);
            Assert.Equal(0, result.Score);
            Assert.Equal(0, result.CurrentQuestionIndex);
        }

        [Fact]
        public async Task GetNextQuestionForUser_ShouldReturnNextQuestion_WhenQuestionsAreAvailable()
        {
            string username = "existingUser";
            GameObj existingGame = new GameObj { Username = username, Score = 2, CurrentQuestionIndex = 3 };
            int totalQuestions = 5;
            Question nextQuestion = new Question { Text = "Next question" };

            _gamesCollectionMock.Setup(c => c.Find<GameObj>(It.IsAny<Func<FilterDefinition<GameObj>, FindOptions<GameObj, GameObj>>>()))
                .ReturnsAsync(existingGame);
            _questionServiceMock.Setup(q => q.CountQuestions()).ReturnsAsync(totalQuestions);
            _questionServiceMock.Setup(q => q.GetQuestionByIndex(existingGame.CurrentQuestionIndex)).ReturnsAsync(nextQuestion);

            var result = await _gameService.GetNextQuestionForUser(username) as JsonResult;

            Assert.NotNull(result);
            Assert.Equal(nextQuestion, result.Value);
        }
    }
}
