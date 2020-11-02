using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace LightestNight.System.Data.MySql
{
    public static class ExtendsServiceCollection
    {
        public static IServiceCollection AddMySqlData(this IServiceCollection services, MySqlOptionsFactory mySqlOptionsFactory)
        {
            services.TryAddSingleton<IMySqlConnection>(sp =>
            {
                var loggerFactory = sp.GetService<ILoggerFactory>();
                var logger = loggerFactory?.CreateLogger<MySqlConnection>() ?? NullLogger<MySqlConnection>.Instance;

                return new MySqlConnection(mySqlOptionsFactory, logger);
            });
            return services;
        }
    }
}