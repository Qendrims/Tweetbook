using Refit;
using TwitterBook.Contracts.V1;
using TwitterBook.Contracts.V1.Requests;
using TwitterBook.Contracts.V1.Response;

namespace TweetBook.Sdk;
[Headers("Authorization: Bearer")]
public interface ITweetBookApi
{
   
   [Get("/api/v1/posts")]
   Task<ApiResponse<List<PostResponse>>> GetAllAsync();
   
   [Get("/api/v1/posts/{postId}")]
   Task<ApiResponse<List<PostResponse>>> GetAsync(Guid postId);

   [Get("/api/v1/posts")]
   Task<ApiResponse<PostResponse>> CreateAsync([Body] CreatePostRequest createPostRequest);
   [Put("/api/v1/posts/{postId}")]
   Task<ApiResponse<PostResponse>> UpdateAsync(Guid postId,[Body] UpdatePostRequest updatePostRequest);
   [Delete("/api/v1/posts/{postId}")]
   Task<ApiResponse<string>> DeleteAsync(Guid postId);
}