namespace LightestNight.System.Data.MySql
{
    public interface IMySqlConnection
    {
        /// <summary>
        /// Builds a <see cref="MySqlConnector.MySqlConnection" /> based on the known options
        /// </summary>
        /// <param name="retries">How many times to retry getting the connection</param>
        /// <returns>A ready to use, but closed, <see cref="MySqlConnector.MySqlConnection" /></returns>
        MySqlConnector.MySqlConnection GetConnection(int retries = 3);
    }
}