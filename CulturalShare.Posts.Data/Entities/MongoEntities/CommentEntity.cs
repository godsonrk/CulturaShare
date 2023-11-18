using CulturalShare.Posts.Data.Entities.Base;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace CulturalShare.Posts.Data.Entities.MongoEntities;

[Table("comments")]
public class CommentEntity : ICommentEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.Int32)]
    public int Id { get; set; }
    public string Username { get; set; }
    public string Text { get; set; }
    public DateTime CreatedAt { get; set; }

    public int OwnerId { get; set; }
    public PostEntity Post { get; set; }
}
