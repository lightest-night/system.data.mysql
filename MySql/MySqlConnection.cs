﻿using System;
using MySqlConnector;

namespace LightestNight.System.Data.MySql
{
    public class MySqlConnection : IMySqlConnection
    {
        private readonly MySqlOptions _options;

        public MySqlConnection(MySqlOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        /// <inheritdoc cref="IMySqlConnection.Build" />
        public MySqlConnector.MySqlConnection Build()
        {
            var builder = new MySqlConnectionStringBuilder
            {
                Server = _options.Server,
				Port = _options.Port,
				UserID = _options.UserId,
				Password = _options.Password,
				Database = _options.Database,
				LoadBalance = _options.LoadBalance,
				ConnectionProtocol = _options.ConnectionProtocol,
				PipeName = _options.PipeName,
				SslMode = _options.SslMode,
				CertificateFile = _options.CertificateFile,
				CertificatePassword = _options.CertificatePassword,
				SslCert = _options.SslCert,
				SslKey = _options.SslKey,
				SslCa = _options.SslCa,
				CertificateStoreLocation = _options.CertificateStoreLocation,
				CertificateThumbprint = _options.CertificateThumbprint,
				Pooling = _options.Pooling,
				ConnectionLifeTime = _options.ConnectionLifeTime,
				ConnectionReset = _options.ConnectionReset,
				ConnectionIdlePingTime = _options.ConnectionIdlePingTime,
				ConnectionIdleTimeout = _options.ConnectionIdleTimeout,
				MinimumPoolSize = _options.MinimumPoolSize,
				MaximumPoolSize = _options.MaximumPoolSize,
				AllowLoadLocalInfile = _options.AllowLoadLocalInfile,
				AllowPublicKeyRetrieval = _options.AllowPublicKeyRetrieval,
				AllowUserVariables = _options.AllowUserVariables,
				AllowZeroDateTime = _options.AllowZeroDateTime,
				ApplicationName = _options.ApplicationName,
				AutoEnlist = _options.AutoEnlist,
				CharacterSet = _options.CharacterSet,
				ConnectionTimeout = _options.ConnectionTimeout,
				ConvertZeroDateTime = _options.ConvertZeroDateTime,
				DateTimeKind = _options.DateTimeKind,
				DefaultCommandTimeout = _options.DefaultCommandTimeout,
				ForceSynchronous = _options.ForceSynchronous,
				GuidFormat = _options.GuidFormat,
				IgnoreCommandTransaction = _options.IgnoreCommandTransaction,
				IgnorePrepare = _options.IgnorePrepare,
				InteractiveSession = _options.InteractiveSession,
				Keepalive = _options.Keepalive,
				NoBackslashEscapes = _options.NoBackslashEscapes,
				OldGuids = _options.OldGuids,
				PersistSecurityInfo = _options.PersistSecurityInfo,
				ServerRsaPublicKeyFile = _options.ServerRsaPublicKeyFile,
				ServerSPN = _options.ServerSpn,
				TreatTinyAsBoolean = _options.TreatTinyAsBoolean,
				UseAffectedRows = _options.UseAffectedRows,
				UseCompression = _options.UseCompression,
				UseXaTransactions = _options.UseXaTransactions
            };
            
            return new MySqlConnector.MySqlConnection(builder.ConnectionString);
        }
    }
}