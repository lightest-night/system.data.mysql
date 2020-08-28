namespace LightestNight.System.Data.MySql
{
    public interface IMySqlConnection
    {
        /// <summary>
        /// Builds a <see cref="MySqlConnector.MySqlConnection" /> based on the known options
        /// </summary>
        /// <returns>A ready to use, but closed, <see cref="MySqlConnector.MySqlConnection" /></returns>
        MySqlConnector.MySqlConnection Build();
    }
}