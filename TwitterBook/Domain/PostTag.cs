using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TwitterBook.Domain;

public class PostTag
{
    [Key]
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]

    public int Id { get; set; }
    public string TagName { get; set; }
    public Guid PostId { get; set; }
    [ForeignKey(nameof(PostId))]
    public Post Post { get; set; }
}