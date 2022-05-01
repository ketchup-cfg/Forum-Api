using HeavyMetalMachine.Core.Models;

namespace HeavyMetalMachine.Core.Abstractions;

public interface IPostsService
{
    /// <summary>
    /// Check to see if a post exists that matches the given ID.
    /// </summary>
    /// <param name="id">The unique ID for the post.</param>
    /// <returns>True if a post is found matching the provided ID, false if no post if found.</returns>
    public Task<bool> PostExists(int id);

    /// <summary>
    /// Find and return a collection of all existing posts.
    /// </summary>
    /// <param name="limit">How many posts to return.</param>
    /// <param name="page">Which set of posts to return for the given limit.</param> 
    /// <returns>The collection of existing posts.</returns>
    public Task<IEnumerable<Post>> GetAllPosts(int limit = 30, int page = 1);

    /// <summary>
    /// Find and return a single post using the provided topic ID.
    /// </summary>
    /// <param name="id">The identifier for the post to retrieve.</param>
    /// <returns>The post associated with the provided ID, if found. Otherwise, a null value if not found.</returns>
    public Task<Post?> GetPost(int id);

    /// <summary>
    /// Create a new post and return the newly created post.
    /// </summary>
    /// <param name="post">The values for the new post to be created.</param>
    /// <returns>The newly created post.</returns>
    public Task<Post> AddPost(Post post);

    /// <summary>
    /// Update the information for an existing post.
    /// </summary>
    /// <param name="id">The identifier for the post to update.</param>
    /// <param name="post">The values to use to replace the existing post's data with.</param>
    /// <returns>The number of posts updated.</returns>
    public Task<int> UpdatePost(int id, Post post);

    /// <summary>
    /// Delete a post that matches the provided ID.
    /// </summary>
    /// <param name="id">The identifier for the post to delete.</param>
    /// <returns>The number of posts deleted.</returns>
    public Task<int> RemovePost(int id);
}