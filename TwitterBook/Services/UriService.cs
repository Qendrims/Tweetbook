using Microsoft.AspNetCore.WebUtilities;
using TwitterBook.Contracts.V1;
using TwitterBook.Contracts.V1.Requests.Queries;

namespace TwitterBook.Services;

public class UriService : IUriService
{
    private readonly string _baseUri;
    public UriService(string baseUri)
    {
        _baseUri = baseUri;
    }
    public Uri GetPostsUri(string postId)
    {
        return new Uri(_baseUri + ApiRoutes.Posts.Get.Replace("{postId}",postId));
    }

    public Uri GetAllPostsUri(PaginationQuery paginationQuery = null)
    {
        var uri = new Uri(_baseUri);
        if (paginationQuery == null)
        {
            return uri;
        }
        var modifiedUri = QueryHelpers.AddQueryString
            (_baseUri,"pageNumber",paginationQuery.PageNumber.ToString());
        modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "pageSize", paginationQuery.PageSize.ToString());
        return new Uri(modifiedUri);
    }
}