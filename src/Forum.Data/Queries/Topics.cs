using System.Dynamic;
using Forum.Data.Models;

namespace Forum.Data.Queries;

public class Topics
{
    public static List<Topic> GetAll()
    {
        return new List<Topic> {
            new()
            {
                Id = 1,
                Name = "Test"
            }
        };
    }
    
}