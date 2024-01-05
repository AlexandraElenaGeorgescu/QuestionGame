using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

public class Question
{
    public Guid Id { get; set; }
    public string Text { get; set; }
    public string CorrectAnswer { get; set; }
}
