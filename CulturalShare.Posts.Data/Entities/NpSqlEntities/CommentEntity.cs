using CulturalShare.Posts.Data.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CulturalShare.Posts.Data.Entities.NpSqlEntities;

[Table("comments")]
public class CommentEntity : ICommentEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Username { get; set; }
    public string Text { get; set; }
    public DateTime CreatedAt { get; set; }

    public int OwnerId { get; set; }
    public PostEntity Post { get; set; }
}
