using Somewhere.Data.Models;

namespace Somewhere.Services.Abstractions;

public interface ITopicsService
{
    /// <summary>
    /// Check to see if a topic exists that matches the given ID.
    /// </summary>
    /// <param name="id">The unique ID for the topic.</param>
    /// <returns>True if a topic is found matching the provided ID, false if no topic if found.</returns>
    public Task<bool> TopicExists(int id);
    
    /// <summary>
    /// Check to see if a topic exists that matches the given name.
    /// </summary>
    /// <param name="name">The unique name for the topic.</param>
    /// <returns>True if a topic is found matching the provided name, false if no topic if found.</returns>
    public Task<bool> TopicExists(string name);

    /// <summary>
    /// Check to see if the name for the new topic is unique or if another topic already exists that matches the given
    /// name.
    /// </summary>
    /// <param name="name">The desired name for the new topic.</param>
    /// <returns>
    /// True if no other topics are found with the desired name, false if the name is already taken by a different
    /// topic.
    /// </returns>
    public Task<bool> NameIsUnique(string name);
    
    /// <summary>
    /// Check to see if the new name for the existing topic is unique or if another topic already exists that matches
    /// the given name.
    /// </summary>
    /// <param name="id">The current ID for the topic being updated.</param>
    /// <param name="newName">The new desired name for the topic.</param>
    /// <returns>
    /// True if no other topics are found with the desired new name, false if the name is already taken by a different
    /// topic.
    /// </returns>
    public Task<bool> NewNameIsUnique(int id, string newName);
    
    /// <summary>
    /// Check to see if the new ID for the existing topic is unique or if another topic already exists that matches
    /// the given ID.
    /// </summary>
    /// <param name="id">The current ID for the topic being updated.</param>
    /// <param name="newId">The new desired ID for the topic.</param>
    /// <returns>
    /// True if no other topics are found with the desired new ID, false if the ID is already taken by a different
    /// topic.
    /// </returns>
    public Task<bool> NewIdIsUnique(int id, int newId);
    
    /// <summary>
    /// Find and return a collection of all existing topics.
    /// </summary>
    /// <param name="limit">How many topics to return.</param>
    /// <param name="page">Which set of topics to return for the given limit.</param> 
    /// <returns>The collection of existing topics.</returns>
    public Task<IEnumerable<Topic>> GetAllTopics(int limit = 30, int page = 1);
    
    /// <summary>
    /// Find and return a single topic using the provided topic ID.
    /// </summary>
    /// <param name="id">The identifier for the topic to retrieve.</param>
    /// <returns>The topic associated with the provided ID, if found. Otherwise, a null value if not found.</returns>
    public Task<Topic?> GetTopic(int id);
    
    /// <summary>
    /// Find and return a single topic using the provided topic name.
    /// </summary>
    /// <param name="name">The unique name for the topic to retrieve.</param>
    /// <returns>The topic associated with the provided name, if found. Otherwise, a null value if not found.</returns>
    public Task<Topic?> GetTopic(string name);
    
    /// <summary>
    /// Create a new topic and return the newly created topic.
    /// </summary>
    /// <param name="topic">The values for the new topic to be created.</param>
    /// <returns>The newly created topic.</returns>
    public Task<Topic> AddTopic(Topic topic);

    /// <summary>
    /// Update the information for an existing topic.
    /// </summary>
    /// <param name="id">The identifier for the topic to update.</param>
    /// <param name="topic">The values to use to replace the existing topic's data with.</param>
    /// <returns>The number of topics updated.</returns>
    public Task<int> UpdateTopic(int id, Topic topic);
    
    /// <summary>
    /// Delete a topic definition that matches the provided ID.
    /// </summary>
    /// <param name="id">The identifier for the topic to delete.</param>
    /// <returns>The number of topics deleted.</returns>
    public Task<int> RemoveTopic(int id);
}