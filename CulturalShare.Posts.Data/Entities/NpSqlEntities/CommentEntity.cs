using CulturalShare.Posts.Data.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CulturalShare.Posts.Data.Entities.NpSqlEntities;

public class CommentEntity : ICommentEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; }
    public string Username { get; set; }
    public string Text { get; set; }
    public DateTime Timestamp { get; set; }

    public int Owner_Id { get; set; }
    public PostEntity Post { get; set; }
}
