namespace HeavyMetalMachine.Core.Models;

/// <summary>
/// A forum post.
/// </summary>
public class Post
{
    /// <summary>
    /// The unique identifier for the post.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The title of the post.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// The optional content for the post.
    /// </summary>
    public string? Content { get; set; }
}