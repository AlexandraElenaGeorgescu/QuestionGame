using GameAppApi.API.DatabaseSettings;
using GameAppApi.Game.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

public class QuestionService: IQuestionService
{
    private readonly IMongoCollection<Question> _questions;

    public QuestionService(IMongoDBSettings settings)
    {
        var client = new MongoClient(settings.ConnectionString);
        var database = client.GetDatabase(settings.DatabaseName);
        _questions = database.GetCollection<Question>("Questions"); 
    }

    public async Task<List<Question>> Get()
    {
        return await _questions.Find(question => true).ToListAsync();
    }

    public async Task<Question> Get(string id)
    {
        return await _questions.Find<Question>(question => question.Id.ToString() == id).FirstOrDefaultAsync();
    }

    public async Task<Question> Create(Question question)
    {
        await _questions.InsertOneAsync(question);
        return question;
    }

    public async Task Update(string id, Question questionIn)
    {
        await _questions.ReplaceOneAsync(question => question.Id.ToString() == id, questionIn);
    }

    public async Task Remove(string id)
    {
        await _questions.DeleteOneAsync(question => question.Id.ToString() == id);
    }
}
