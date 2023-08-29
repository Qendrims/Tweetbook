using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TwitterBook.Authorization;
using TwitterBook.Contracts.V1;
using TwitterBook.Contracts.V1.Requests;
using TwitterBook.Contracts.V1.Response;
using TwitterBook.Extensions;
using TwitterBook.Services;

namespace TwitterBook.Controllers.V1;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Poster")]
public class Tags : Controller
{
    private readonly IPostService _postService;
    private readonly IMapper _mapper;
    public Tags(IPostService postService, IMapper mapper)
    {
        _postService = postService;
        _mapper = mapper;
    }
    /// <summary>
    ///  Returns all the tags in the system.
    /// </summary>
    /// <response code="200">
    /// Returns all the tags in the system.
    /// </response>
    [HttpGet(ApiRoutes.Tags.GetAll)]
    public async Task<IActionResult> GetAllAsync()
    {
        var tags = await _postService.GetAllTagsAsync();
        // var tagsResponse = tags.Select(x => new TagResponse
        // {
        //     TagName = x.TagName
        // }).ToList();
        var tagsResponse = _mapper.Map<List<TagResponse>>(tags);
        return Ok(new Response<List<TagResponse>>(_mapper.Map<List<TagResponse>>(tags)));
    }
    /// <summary>
    /// Gets a specific tag.
    /// </summary>
    /// <param name="tagId"></param>
    /// <returns></returns>
    [HttpGet(ApiRoutes.Tags.Get)]
    public async Task<IActionResult> GetTag(string tagId)
    {
        var tag = await _postService.GetTagById(tagId);
        if (tag == null)
            return NotFound();
        return Ok(new Response<TagResponse>(_mapper.Map<TagResponse>(tag)));
    }
    /// <summary>
    ///  Creates a tag in the system.
    /// </summary>
    /// <remarks>
    /// Sample *Request*:
    ///         POST /api/v1/tags
    ///         {
    ///             "name":"some name"
    ///         }
    /// </remarks>
    /// <response code="200"> Creates a tag in the system.</response>
    /// <response code="400"> Unable to create the tag due to validation error.</response>
    [HttpPost(ApiRoutes.Tags.Create)]
    public async Task<IActionResult> Create([FromBody] CreateTagRequest request)
    {
        var userId = HttpContext.GetUserId();
        Domain.Tags newTag = new()
        {
            Id = Guid.NewGuid().ToString(),
            TagName = request.TagName,
            CreatorId = userId
        };
        var created = await _postService.CreateTagsAsync(newTag);
        if (!created)
            return BadRequest();

        var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
        var locationUri = baseUrl + "/" + ApiRoutes.Tags.Get.Replace("{tagId}", newTag.Id);
        var response = _mapper.Map<TagResponse>(newTag);
        return Created(locationUri, new Response<TagResponse>(_mapper.Map<TagResponse>(newTag)));
    }

    [HttpPut(ApiRoutes.Tags.Update)]    
    public async Task<IActionResult> UpdateTagAsync([FromBody] UpdateTagRequest request)
    {
        var tag = await _postService.GetTagById(request.Id);
        tag.TagName = request.tagName;
        
        var updated = await _postService.UpdateTagAsync(tag);
        
        if (updated)
        {
            return Ok(new Response<TagResponse>(_mapper.Map<TagResponse>(tag)));
        }

        return NotFound();
    }

    [HttpDelete(ApiRoutes.Tags.Delete)]
    public async Task<IActionResult> DeleteTagAsync([FromRoute] string tagId)
    {
        var tag = await _postService.GetTagById(tagId);
        if (HttpContext.GetUserId() != tag.CreatorId)
        {
            return BadRequest();
        }

        var deleted = await _postService.DeleteTagAsync(tagId);
        if (deleted)
        {
            return NoContent();
        }

        return NotFound();
    }
}