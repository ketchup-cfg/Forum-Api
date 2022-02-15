using System.Data;

namespace Forum.Data.Interfaces;

public interface IDatabase
{
    public IDbConnection Connect();
}