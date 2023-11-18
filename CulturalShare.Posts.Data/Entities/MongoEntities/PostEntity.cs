using CulturalShare.Posts.Data.Entities.Base;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CulturalShare.Posts.Data.Entities.MongoEntities;

public class PostEntity : IPostEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.Int32)]
    public int Id { get; set; }
    public string Caption { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAd { get; set; }
    public string? ImageUrl { get; set; }
    public int Likes { get; set; }
    public string? Location { get; set; }

    public int OwnerId { get; set; }
    public ICollection<CommentEntity> Comments { get; set; }
}
