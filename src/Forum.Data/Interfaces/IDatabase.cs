using System.Data;

namespace Forum.Data.Interfaces;

public interface IDatabase
{
    /// <summary>
    /// Generate a connection to the application database.
    /// </summary>
    /// <returns>An open connection to the application database.</returns>
    public IDbConnection Connect();
}