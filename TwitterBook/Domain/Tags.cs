using System.ComponentModel.DataAnnotations;

namespace TwitterBook.Domain;

public class Tags
{
    [Key]
    public string Id { get; set; }
    public string TagName { get; set; }
    public DateTime CreatedOn { get; set; }= DateTime.UtcNow;
    public string CreatorId { get; set; }
}