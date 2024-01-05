using GameAppApi.API.DatabaseSettings;
using GameAppApi.Game.Models;
using MongoDB.Bson;
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
        if (Guid.TryParse(id, out Guid guidId))
        {
            var filter = Builders<Question>.Filter.Eq(q => q.Id, guidId);
            return await _questions.Find(filter).FirstOrDefaultAsync();
        }
        else
        {
            throw new ArgumentException("Invalid GUID format.");
        }
    }


    public async Task<Question> Create(Question question)
    {
        await _questions.InsertOneAsync(question);
        return question;
    }

    public async Task Update(string id, Question questionIn)
    {
        if (Guid.TryParse(id, out Guid guidId))
        {
            var filter = Builders<Question>.Filter.Eq(q => q.Id, guidId);
            await _questions.ReplaceOneAsync(filter, questionIn);
        }
        else
        {
            throw new ArgumentException("Invalid GUID format.");
        }
    }


    public async Task Remove(string id)
    {
        if (Guid.TryParse(id, out Guid guidId))
        {
            // Convert the Guid to MongoDB Bson binary data
            var filter = Builders<Question>.Filter.Eq(q => q.Id, guidId);
            await _questions.DeleteOneAsync(filter);
        }
        else
        {
            throw new ArgumentException("Invalid GUID format.");
        }
    }
    public async Task<long> CountQuestions()
    {
        return await _questions.CountDocumentsAsync(new BsonDocument());
    }

    public async Task<Question> GetQuestionByIndex(int index)
    {
        return await _questions.Find(new BsonDocument()).Skip(index).FirstOrDefaultAsync();
    }

}
