namespace TwitterBook.Contracts.V1.Response;

public class Response<T>
{
    public Response(T response)
    {
        Data = response;
    }

    public T Data { get; set; }
}