using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace LightestNight.System.Data.MySql
{
    public static class ExtendsServiceCollection
    {
        public static IServiceCollection AddMySqlData(this IServiceCollection services, MySqlOptionsFactory mySqlOptionsFactory)
        {
            services.TryAddSingleton<IMySqlConnection>(_ => new MySqlConnection(mySqlOptionsFactory));
            return services;
        }
    }
}