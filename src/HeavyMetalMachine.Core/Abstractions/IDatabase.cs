using System.Data;

namespace HeavyMetalMachine.Core.Abstractions;

public interface IDatabase
{
    /// <summary>
    /// Generate a connection to the application database.
    /// </summary>
    /// <returns>An open connection to the application database.</returns>
    public IDbConnection Connect();

    /// <summary>
    /// Initialize the application database.
    /// </summary>
    public void Initialize();
}