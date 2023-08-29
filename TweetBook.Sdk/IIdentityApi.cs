using Refit;
using TwitterBook.Contracts.V1.Requests;
using TwitterBook.Contracts.V1.Response;

namespace TweetBook.Sdk;

public interface IIdentityApi
{
    [Post("/api/v1/identity/register")]
    Task<ApiResponse<AuthSuccesResponse>> RegisterAsync([Body] UserRegistrationRequest registrationRequest);

    [Post("/api/v1/identity/login")]
    Task<ApiResponse<AuthSuccesResponse>> LoginAsync([Body] UserLoginRequest loginRequest);

    [Post("/api/v1/identity/refresh")]
    Task<ApiResponse<AuthSuccesResponse>> RefreshAsync([Body] RefreshTokenRequest refreshRequest);
}