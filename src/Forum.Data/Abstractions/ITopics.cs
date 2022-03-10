using Forum.Data.Models;

namespace Forum.Data.Abstractions;

public interface ITopics
{
    /// <summary>
    /// Find and return a collection of all existing topics.
    /// </summary>
    /// <returns>The collection of existing topics.</returns>
    public Task<IEnumerable<Topic>> GetAll();
    
    /// <summary>
    /// Find and return a single topic using the provided topic ID.
    /// </summary>
    /// <param name="id">The identifier for the topic to retrieve.</param>
    /// <returns>The topic associated with the provided ID, if found. Otherwise, a null value if not found.</returns>
    public Task<Topic?> GetTopicById(int id);
    
    /// <summary>
    /// Find and return a single topic using the provided topic name.
    /// </summary>
    /// <param name="name">The unique name for the topic to retrieve.</param>
    /// <returns>The topic associated with the provided name, if found. Otherwise, a null value if not found.</returns>
    public Task<Topic?> GetTopicByName(string name);

    /// <summary>
    /// Create a new topic and return the ID for the new topic.
    /// </summary>
    /// <param name="topic">The values for the new topic to be created.</param>
    /// <returns>The ID for the new topic record.</returns>
    public Task<int> CreateTopic(Topic topic);

    /// <summary>
    /// Update the information for an existing topic.
    /// </summary>
    /// <param name="topic">The values to use to replace the existing topic's data with.</param>
    /// <returns>The number of topics updated successfully.</returns>
    public Task<int> UpdateTopic(Topic topic);

    /// <summary>
    /// Delete a topic definition that matches the provided ID.
    /// </summary>
    /// <param name="id">The identifier for the topic to delete.</param>
    /// <returns>The number of topics that were deleted that match the provided ID.</returns>
    public Task<int> DeleteTopic(int id);
    
    /// <summary>
    /// Remove all existing topic entries.
    /// </summary>
    /// <returns>The number of topics removed.</returns>
    public Task<int> RemoveAll();
}