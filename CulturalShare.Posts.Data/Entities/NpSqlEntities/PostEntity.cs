using CulturalShare.Posts.Data.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CulturalShare.Posts.Data.Entities.NpSqlEntities;

[Table("posts")]
public class PostEntity : IPostEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
