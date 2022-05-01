using Microsoft.AspNetCore.Mvc;
using HeavyMetalMachine.Core.Abstractions;
using WallOfNoise.Api.Controllers.Base;
using HeavyMetalMachine.Core.Models;

namespace WallOfNoise.Api.Controllers;

public class PostsController : SomeBaseController
{
    private readonly IPostsService _posts;
    private readonly ILogger<PostsController> _log;

    public PostsController(IPostsService posts, ILogger<PostsController> log)
    {
        _posts = posts;
        _log = log;
    }

    /// <summary>
    /// Find and return a collection of all existing posts.
    /// </summary>
    /// <returns>A collection containing data for all existing posts.</returns>
    [HttpGet]
    public async Task<IActionResult> GetAllPosts()
    {
        _log.LogInformation("Searching for posts");
        
        var posts = await _posts.GetAllPosts();

        _log.LogInformation("Found {PostCount} topics", posts.Count());
        
        return Ok(posts);
    }

    /// <summary>
    /// Find and return information for a single post matching the specified post ID. 
    /// </summary>
    /// <param name="id">The unique ID for a single post.</param>
    /// <returns>The data for the post that was found using the provided ID.</returns>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetPostById([FromRoute] int id)
    {
        _log.LogInformation("Searching for post with ID {PostId}", id);
        
        var post = await _posts.GetPost(id);

        if (post is not null) return Ok(post);
        
        _log.LogWarning("Could not locate post with ID {PostId}", id);
        
        return NotFound();
    }

    /// <summary>
    /// Create a new post.
    /// </summary>
    /// <param name="post">The new post to be created.</param>
    /// <returns>The newly created post, including its new ID.</returns>
    [HttpPost]
    public async Task<IActionResult> CreateTopic([FromBody] Post post)
    {
        _log.LogInformation("Creating new post with title: {PostTitle}", post.Title);
        
        var newPost = await _posts.AddPost(post);
        
        _log.LogInformation("New post successfully created with ID of {PostId}", newPost.Id);

        return CreatedAtAction(nameof(GetPostById), new {id = newPost.Id}, newPost);
    }

    /// <summary>
    /// Update an existing post matching the specified post ID. 
    /// </summary>
    /// <param name="id">The unique ID for the post to be updated.</param>
    /// <param name="post">The post values to replace the existing post's values with.</param>
    [HttpPut("{id:int}")]
    [Consumes("application/json")]
    public async Task<IActionResult> UpdatePost([FromRoute] int id, [FromBody] Post post)
    {
        _log.LogInformation("Updating post with ID of {PostId}", id);
        
        var numberOfPostsUpdated = await _posts.UpdatePost(id, post);
        _log.LogInformation("Updated {UpdateCount} posts with an ID of {PostId}", numberOfPostsUpdated, id);
        
        return numberOfPostsUpdated == 0 ? NotFound() : NoContent();
    }

    /// <summary>
    /// Delete the post that matches the ID passed into the URI route.
    /// </summary>
    /// <param name="id">The unique ID for the post to be deleted.</param>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeletePost([FromRoute] int id)
    {
        _log.LogInformation("Removing post with ID of {PostId}", id);
        
        var numberOfPostsRemoved = await _posts.RemovePost(id);
    
        _log.LogInformation("Removed {DeleteCount} posts with an ID of {PostId}", numberOfPostsRemoved, id);
        
        return numberOfPostsRemoved == 0 ? NotFound() : NoContent();
    }
}