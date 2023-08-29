using TwitterBook.Contracts.V1.Requests.Queries;

namespace TwitterBook.Services;

public interface IUriService
{
    Uri GetPostsUri(string postId);
    Uri GetAllPostsUri(PaginationQuery paginationQuery = null);
}