using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace LightestNight.System.Data.MySql
{
    public static class ExtendsServiceCollection
    {
        public static IServiceCollection AddMySqlData(this IServiceCollection services,
            Action<MySqlOptions>? options = null)
        {
            var mysqlOptions = new MySqlOptions();
            options?.Invoke(mysqlOptions);

            services.TryAddSingleton<IMySqlConnection>(_ => new MySqlConnection(mysqlOptions));

            return services;
        }
    }
}