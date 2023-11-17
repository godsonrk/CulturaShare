using CulturalShare.Posts.Data.Entities.NpSqlEntities;

namespace CulturalShare.Posts.Data.Entities.Base;

public interface IPostEntity : IEntity
{
    public string Caption { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAd { get; set; }
    public string? ImageUrl { get; set; }
    public int Likes { get; set; }
    public string? Location { get; set; }

    public int OwnerId { get; set; }
    public ICollection<CommentEntity> Comments { get; set; }
}
