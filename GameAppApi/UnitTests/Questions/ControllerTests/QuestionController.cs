using Moq;
using Xunit;
using GameAppApi.Game.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

public class QuestionsControllerTests
{
    private readonly Mock<IQuestionService> _mockQuestionService;
    private readonly QuestionsController _controller;

    public QuestionsControllerTests()
    {
        _mockQuestionService = new Mock<IQuestionService>();
        _controller = new QuestionsController(_mockQuestionService.Object);
    }

    [Fact]
    public async Task GetQuestions_ReturnsListOfQuestions()
    {
        // Arrange
        var questions = new List<Question> { new Question { /* ... */ }, new Question { /* ... */ } };
        _mockQuestionService.Setup(service => service.Get()).ReturnsAsync(questions);

        // Act
        var result = await _controller.GetQuestions();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(questions, okResult.Value);
    }

    [Fact]
    public async Task PostQuestion_ValidQuestion_ReturnsCreatedAtActionResult()
    {
        // Arrange
        var question = new Question { /* ... */ };
        _mockQuestionService.Setup(service => service.Create(It.IsAny<Question>())).ReturnsAsync(question);

        // Act
        var result = await _controller.PostQuestion(question);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(question, createdAtActionResult.Value);
    }

    [Fact]
    public async Task DeleteQuestion_ExistingQuestion_ReturnsNoContentResult()
    {
        // Arrange
        var questionId = "valid-guid";
        var question = new Question { /* ... */ };
        _mockQuestionService.Setup(service => service.Get(questionId)).ReturnsAsync(question);
        _mockQuestionService.Setup(service => service.Remove(questionId)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteQuestion(questionId);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteQuestion_NonExistingQuestion_ReturnsNotFoundResult()
    {
        // Arrange
        _mockQuestionService.Setup(service => service.Get(It.IsAny<string>())).ReturnsAsync((Question)null);

        // Act
        var result = await _controller.DeleteQuestion("non-existing-guid");

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
