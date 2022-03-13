using System.ComponentModel.DataAnnotations;

namespace Somewhere.Data.Models;

/// <summary>
/// A forum topic.
/// </summary>
public class Topic
{
    /// <summary>
    /// The unique identifier for the topic.
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// The unique human-readable name of the topic.
    /// </summary>
    [Required]
    [MinLength(1)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The optional detailed description of the topic.
    /// </summary>
    public string? Description { get; set; }
}