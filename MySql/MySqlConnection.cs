using System;
using System.Data;
using System.Linq;
using MySqlConnector;
using Polly;
using Polly.Contrib.WaitAndRetry;

namespace LightestNight.System.Data.MySql
{
	public class MySqlConnection : IMySqlConnection
    {
	    private readonly MySqlOptionsFactory _optionsFactory;

	    private MySqlConnector.MySqlConnection? _connection;
	    
        public MySqlConnection(MySqlOptionsFactory optionsFactory)
        {
            _optionsFactory = optionsFactory ?? throw new ArgumentNullException(nameof(optionsFactory));
        }
        
        public MySqlConnector.MySqlConnection GetConnection(int retries = 3)
        {
	        if (_connection != null && ValidateConnection(_connection, out _))
		        return _connection;

	        var maxDelay = TimeSpan.FromSeconds(10);
	        var delay = Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromMilliseconds(500), retries)
		        .Select(s => TimeSpan.FromTicks(Math.Min(s.Ticks, maxDelay.Ticks)));
	        var retryPolicy = Policy
		        .Handle<MySqlException>()
		        .WaitAndRetry(delay);
		       
	        retryPolicy.Execute(() =>
	        {
		        var connection = Build();
		        ValidateConnection(connection, out var mySqlException);
		        if (mySqlException != null)
			        throw mySqlException;

		        _connection = connection;
	        });

	        return _connection ??
	               throw new InvalidOperationException("An error occurred building an instance of MySqlConnection");
        }
        
        public bool ValidateConnection(IDbConnection connection, out MySqlException? exception)
        {
	        try
	        {
		        connection.Open();
	        }
	        catch (MySqlException ex)
	        {
		        exception = ex;
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