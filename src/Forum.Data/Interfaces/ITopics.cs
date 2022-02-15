using Forum.Data.Models;

namespace Forum.Data.Interfaces;

public interface ITopics
{
    public Task<IEnumerable<Topic>> GetAll();
}