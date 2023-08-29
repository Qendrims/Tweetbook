namespace TwitterBook.Contracts.V1.Requests.Queries;

public class PaginationQuery
{
    public PaginationQuery()
    {
        PageNumber = 1;
        PageSize = 6;
    }

    public PaginationQuery(int pageNumber,int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}