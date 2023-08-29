using System.Diagnostics;
using System.Net;
using FluentAssertions;
using TwitterBook.Contracts.V1;
using TwitterBook.Contracts.V1.Response;
using TwitterBook.Domain;

namespace TwitterBook.IntegrationTests;

public class PostTests : IntegrationTest
{

    [Fact]
    public async Task NormalAllPostsReturn()
    {
        await AuthenticateAsync();

        Stopwatch.StartNew();
        TestClient.GetAsync();
    }
    
    
    
    [Fact]
    public async Task GetAll_WithoutAnyPostsReturnsEmpty()
    {
        //Arrange
        await AuthenticateAsync();
        //Act
        var response = await TestClient.GetAsync(ApiRoutes.Posts.GetAll);
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadAsAsync<List<Post>>()).Should().BeNull();
    }

    [Fact]
    public async Task GetAllReturnsPosts()
    {
        //Arrange
        await AuthenticateAsync();
        //Act
        var response = await TestClient.GetAsync(ApiRoutes.Posts.GetAll);
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var posts = (await response.Content.ReadAsAsync<PagedResponse<PostResponse>>()).Data.ToList();
        // (await response.Content.ReadAsAsync<PagedResponse<PostResponse>>()).Data.ToList().Should().NotBeEmpty();
        posts.Should().NotBeEmpty();
    }

    [Theory]
    [InlineData("0d0951b5-133b-4bcc-90b0-5a9fe0321ea7")]
    public async Task ShouldReturn404PostNotFound(string value)
    {
        await AuthenticateAsync();

        var response = await TestClient.GetAsync($"{ApiRoutes.Posts.Get}/{value}");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);   

    }
}