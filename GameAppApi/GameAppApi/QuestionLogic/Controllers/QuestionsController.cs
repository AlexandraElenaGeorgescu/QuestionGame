using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class QuestionsController : ControllerBase
{
    private readonly QuestionService _questionService;

    public QuestionsController(QuestionService questionService)
    {
        _questionService = questionService;
    }

    // GET: api/Questions
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Question>>> GetQuestions()
    {
        return await _questionService.Get();
    }

    // POST: api/Questions
    [HttpPost]
    public async Task<ActionResult<Question>> PostQuestion(Question question)
    {
        await _questionService.Create(question);
        return CreatedAtAction(nameof(GetQuestions), new { id = question.Id }, question);
    }

    // DELETE: api/Questions/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteQuestion(string id)
    {
        var question = await _questionService.Get(id);
        if (question == null)
        {
            return NotFound();
        }

        await _questionService.Remove(question.Id.ToString());
        return NoContent();
    }

}
