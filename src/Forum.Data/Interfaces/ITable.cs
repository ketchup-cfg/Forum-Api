namespace Forum.Data.Interfaces;

public interface ITable
{
    /// <summary>
    /// Initialize the table for the application database, ensuring to also drop the table if it already exists.  
    /// </summary>
    public void Initialize();
    
    /// <summary>
    /// Drop the table from the application database.
    /// </summary>
    public void DropTable();

    /// <summary>
    /// Create the table for the application database.
    /// </summary>
    public void CreateTable();

    /// <summary>
    /// Truncate the table in the application database and remove all records.
    /// </summary>
    public void ClearTable();
}