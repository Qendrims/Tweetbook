namespace TwitterBook.Contracts.V1.Requests
{
    public class CreatePostRequest
    {
        public string Name { get; set; }
        public IEnumerable<string> tagNames { get; set; }
    }
}
