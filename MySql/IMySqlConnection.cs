using System;
using System.Data;

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

        /// <summary>
        /// Validates that the connection to the db is currently valid
        /// </summary>
        /// <param name="connection">The <see cref="IDbConnection" /> to test</param>
        /// <param name="exception">Any exceptions that are thrown when validating</param>
        /// <returns>Boolean denoting whether the connection is valid</returns>
        bool ValidateConnection(IDbConnection connection, out Exception? exception);
    }
}