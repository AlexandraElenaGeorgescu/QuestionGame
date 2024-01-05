public interface IQuestionService
{
    Task<List<Question>> Get();
    Task<Question> Get(string id);
    Task<Question> Create(Question question);
    Task Update(string id, Question questionIn);
    Task Remove(string id);
    public Task<Question> GetQuestionByIndex(int index);
    public Task<long> CountQuestions();

}
