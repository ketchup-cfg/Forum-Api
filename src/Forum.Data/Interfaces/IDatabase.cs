using System.Data;
using Dapper;

namespace Forum.Data.Interfaces;

public interface IDatabase
{
    /// <summary>
    /// Generate a connection to the application database.
    /// </summary>
    /// <returns>An open connection to the application database.</returns>
    public IDbConnection Connect();

    /// <summary>
    /// Initialize the database by creating tables needed for the application database, ensuring to also drop these
    /// tables if they already exist.  
    /// </summary>
    public void Initialize();
}