using System.Globalization;
using Refit;
using TweetBook.Sdk;
using TwitterBook.Contracts.V1.Requests;

class Program
{
    static async Task Main(string[] args)
    {
        var cachedToken = string.Empty;
        var identityApi = RestService.For<IIdentityApi>("https://localhost:7270");
        var tweetBookApi = RestService.For<ITweetBookApi>("https://localhost:7270", new RefitSettings
        {
            AuthorizationHeaderValueGetter = () => Task.FromResult(cachedToken)
        });
        var registerResponse = await identityApi.RegisterAsync(new UserRegistrationRequest
        {
            Email = "sdkacc@sdk.com",
            Password = "Blendi123."
        });
        var loginResponse = await identityApi.LoginAsync(new UserLoginRequest
        {
            Email = "sdkacc@sdk.com",
            Password = "Blendi123."
        });
        cachedToken = loginResponse.Content.Token;

        var allPosts = await tweetBookApi.GetAllAsync();
        var createPost = await tweetBookApi.CreateAsync(new CreatePostRequest
        {
            Name = "refit post",
            tagNames = new[] { "refit1", "refit2" }
        });
        var retrievedPost = await tweetBookApi.GetAsync(createPost.Content.Id);
        
        
    }
}

