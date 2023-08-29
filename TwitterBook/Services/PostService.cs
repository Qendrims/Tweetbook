using Microsoft.EntityFrameworkCore;
using TwitterBook.Data;
using TwitterBook.Domain;

namespace TwitterBook.Services;

public class PostService : IPostService
{
    private readonly DataContext _dataContext;

    public PostService(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<List<Post>> GetPostsAsync(PaginationFilter paginationFilter = null)
    {
        if (paginationFilter == null)
        {
        return await _dataContext.Posts.Include(x => x.PostTags).ToListAsync();
        }

        var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
        return await _dataContext.Posts.Include(x => x.PostTags).Skip(skip)
            .Take(paginationFilter.PageSize)
            .ToListAsync();
    }

    public async Task<Post> GetPostByIdAsync(Guid postId)
    {
        return await _dataContext.Posts.Include(x => x.PostTags).SingleOrDefaultAsync(x => x.Id == postId);
    }

    public async Task<bool> CreatePostAsync(Post post)
    {
        foreach (var postPostTag in post.PostTags)
        {
            postPostTag.TagName = postPostTag.TagName.ToLower();
        }

        await AddNewTags(post);

        await _dataContext.Posts.AddAsync(post);
        var created = await _dataContext.SaveChangesAsync();
        return created > 0;
    }

    private async Task<bool> AddNewTags(Post post)
    {
        if (post != null)
        {
            foreach (var tag in post.PostTags)
            {
                tag.Post = post;
                await _dataContext.PostTags.AddAsync(tag);
            }

            // var added = await _dataContext.SaveChangesAsync();
            // if (added > 0)
            return true;
        }

        return false;
        // return false;
    }

    public async Task<bool> UserOwnsPostAsync(Guid postId, string userId)
    {
        var post = await _dataContext.Posts.AsNoTracking().SingleOrDefaultAsync(x => x.Id == postId);
        if (post == null)
        {
            return false;
        }

        if (post.UserId != userId)
        {
            return false;
        }

        return true;
    }


    public async Task<bool> UpdatePostAsync(Post postToUpdate)
    {
        _dataContext.Posts.Update(postToUpdate);
        var updated = await _dataContext.SaveChangesAsync();
        return updated > 0;
    }

    public async Task<bool> DeletePostAsync(Guid postId)
    {
        var post = await GetPostByIdAsync(postId);
        _dataContext.Posts.Remove(post);
        var deleted = await _dataContext.SaveChangesAsync();
        return deleted > 0;
    }

    public async Task<List<Tags>> GetAllTagsAsync()
    {
        return await _dataContext.Tags.ToListAsync();
    }

    public async Task<Tags> GetTagById(string tagId)
    {
        return await _dataContext.Tags.SingleOrDefaultAsync(x => x.Id == tagId);
    }

    public async Task<bool> CreateTagsAsync(Tags tag)
    {
        await _dataContext.Tags.AddAsync(tag);

        var created = await _dataContext.SaveChangesAsync();
        if (created > 0)
            return true;
        return false;
    }

    public async Task<bool> UpdateTagAsync(Tags tagToUpdate)
    {
        _dataContext.Tags.Update(tagToUpdate);
        var updated = await _dataContext.SaveChangesAsync();
        return updated > 0;
    }

    public async Task<bool> DeleteTagAsync(string tagId)
    {
        var tag = await _dataContext.Tags.SingleOrDefaultAsync(x => x.Id == tagId);
        _dataContext.Tags.Remove(tag);
        var deleted = await _dataContext.SaveChangesAsync();
        return deleted > 0;
    }
}