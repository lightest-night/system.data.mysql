using System;
using System.Data;
using System.Linq;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using Polly;
using Polly.Contrib.WaitAndRetry;

namespace LightestNight.System.Data.MySql
{
	public class MySqlConnection : IMySqlConnection
    {
	    private readonly MySqlOptionsFactory _optionsFactory;
	    private readonly ILogger<MySqlConnection> _logger;

	    public MySqlConnection(MySqlOptionsFactory optionsFactory, ILogger<MySqlConnection> logger)
        {
            _optionsFactory = optionsFactory ?? throw new ArgumentNullException(nameof(optionsFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public MySqlConnector.MySqlConnection GetConnection(int retries = 3)
        {
	        var maxDelay = TimeSpan.FromSeconds(10);
	        var delay = Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromMilliseconds(500), retries)
		        .Select(s => TimeSpan.FromTicks(Math.Min(s.Ticks, maxDelay.Ticks)));
	        var retryPolicy = Policy
		        .Handle<MySqlException>()
		        .WaitAndRetry(delay);
		       
	        return retryPolicy.Execute(() =>
	        {
		        var connection = Build();
		        ValidateConnection(connection, out var exception);
		        
		        if (exception == null) 
			        return connection;
		        
		        throw exception;
	        });
        }
        
        public bool ValidateConnection(IDbConnection connection, out Exception? exception)
        {
	        try
	        {
		        if ((connection.State == ConnectionState.Closed ||
		             connection.State == ConnectionState.Broken) &&
		            connection.State != ConnectionState.Connecting)
			        connection.Open();

		        if (connection.State == ConnectionState.Broken)
			        throw new InvalidOperationException("MySql Connection is broken");
	        }
	        catch (MySqlException ex)
	        {
		        exception = ex;
		        _logger.LogError(ex, "A MySql exception occurred validating a connection to the MySql Database");
		        return false;
	        }
	        catch (Exception ex)
	        {
		        exception = null;
		        _logger.LogDebug(ex, "An exception occurred validating a connection to the MySql Database. Validation Failed");
		        return false;
	        }
	        finally
	        {
		        if (connection.State == ConnectionState.Open)
			        connection.Close();
	        }

	        exception = null;
	        return true;
        }

        private MySqlConnector.MySqlConnection Build()
        {
	        _logger.LogDebug("Building a new MySql Connection");
	        var options = _optionsFactory();
	        
            var builder = new MySqlConnectionStringBuilder
            {
                Server = options.Server,
				Port = options.Port,
				UserID = options.UserId,
				Password = options.Password,
				Database = options.Database,
				LoadBalance = options.LoadBalance,
				ConnectionProtocol = options.ConnectionProtocol,
				PipeName = options.PipeName,
				SslMode = options.SslMode,
				CertificateFile = options.CertificateFile,
				CertificatePassword = options.CertificatePassword,
				SslCert = options.SslCert,
				SslKey = options.SslKey,
				SslCa = options.SslCa,
				CertificateStoreLocation = options.CertificateStoreLocation,
				CertificateThumbprint = options.CertificateThumbprint,
				Pooling = options.Pooling,
				ConnectionLifeTime = options.ConnectionLifeTime,
				ConnectionReset = options.ConnectionReset,
				ConnectionIdlePingTime = options.ConnectionIdlePingTime,
				ConnectionIdleTimeout = options.ConnectionIdleTimeout,
				MinimumPoolSize = options.MinimumPoolSize,
				MaximumPoolSize = options.MaximumPoolSize,
				AllowLoadLocalInfile = options.AllowLoadLocalInfile,
				AllowPublicKeyRetrieval = options.AllowPublicKeyRetrieval,
				AllowUserVariables = options.AllowUserVariables,
				AllowZeroDateTime = options.AllowZeroDateTime,
				ApplicationName = options.ApplicationName,
				AutoEnlist = options.AutoEnlist,
				CharacterSet = options.CharacterSet,
				ConnectionTimeout = options.ConnectionTimeout,
				ConvertZeroDateTime = options.ConvertZeroDateTime,
				DateTimeKind = options.DateTimeKind,
				DefaultCommandTimeout = options.DefaultCommandTimeout,
				ForceSynchronous = options.ForceSynchronous,
				GuidFormat = options.GuidFormat,
				IgnoreCommandTransaction = options.IgnoreCommandTransaction,
				IgnorePrepare = options.IgnorePrepare,
				InteractiveSession = options.InteractiveSession,
				Keepalive = options.Keepalive,
				NoBackslashEscapes = options.NoBackslashEscapes,
				OldGuids = options.OldGuids,
				PersistSecurityInfo = options.PersistSecurityInfo,
				ServerRsaPublicKeyFile = options.ServerRsaPublicKeyFile,
				ServerSPN = options.ServerSpn,
				TreatTinyAsBoolean = options.TreatTinyAsBoolean,
				UseAffectedRows = options.UseAffectedRows,
				UseCompression = options.UseCompression,
				UseXaTransactions = options.UseXaTransactions
            };
            
            return new MySqlConnector.MySqlConnection(builder.ConnectionString);
        }
    }
}