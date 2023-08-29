namespace TwitterBook.Contracts.V1.Requests;

public class UpdatePostRequest
{
    public Guid PostId { get; set; }
    public string Name { get; set; }
}