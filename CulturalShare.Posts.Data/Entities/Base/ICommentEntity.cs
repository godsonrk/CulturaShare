using CulturalShare.Posts.Data.Entities.NpSqlEntities;

namespace CulturalShare.Posts.Data.Entities.Base;

public interface ICommentEntity
{
    public string Id { get; set; }
    public string Username { get; set; }
    public string Text { get; set; }
    public DateTime Timestamp { get; set; }

    public int Owner_Id { get; set; }
    public PostEntity Post { get; set; }
}
