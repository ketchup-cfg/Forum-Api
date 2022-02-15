using Forum.Data.Models;

namespace Forum.Data.Interfaces;

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
}