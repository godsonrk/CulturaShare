using CulturalShare.Posts.Data.Entities.NpSqlEntities;

namespace CulturalShare.Posts.Data.Entities.Base;

public interface ICommentEntity : IEntity
{
    public string Username { get; set; }
    public string Text { get; set; }
    public DateTime CreatedAt { get; set; }

    public int OwnerId { get; set; }
    public PostEntity Post { get; set; }
}
