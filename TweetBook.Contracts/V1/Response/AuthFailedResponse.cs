namespace TwitterBook.Contracts.V1.Response;

public class AuthFailedResponse
{
    public IEnumerable<String> Errors { get; set; }
}