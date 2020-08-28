using System;
using AutoFixture;
using Shouldly;
using Xunit;

namespace LightestNight.System.Data.MySql.Tests
{
    public class MySqlConnectionTests
    {
        private readonly IMySqlConnection _sut;

        public MySqlConnectionTests()
        {
            var fixture = new Fixture();
            var options = fixture.Build<MySqlOptions>()
                .Without(o => o.Server)
                .Without(o => o.Port)
                .Without(o => o.UserId)
                .Without(o => o.Password)
                .Without(o => o.Database)
                .Do(o =>
                {
                    o.Server = Environment.GetEnvironmentVariable("MYSQL_SERVER") ?? "localhost";
                    o.Port = Convert.ToUInt32(Environment.GetEnvironmentVariable("MYSQL_PORT") ?? "3306");
                    o.UserId = Environment.GetEnvironmentVariable("MYSQL_USERID") ?? "mysql";
                    o.Password = Environment.GetEnvironmentVariable("MYSQL_PASSWORD") ?? "mysql";
                    o.Database = Environment.GetEnvironmentVariable("MYSQL_DATABASE") ?? "mysql";
                })
                .Create();

            _sut = new MySqlConnection(options);
        }

        [Fact, Trait("Category", "Unit")]
        public void ShouldBuildNewConnection()
        {
            // Act
            var result = _sut.Build();
            
            // Assert
            result.ConnectionString.ShouldNotBeNullOrEmpty();
            result.ConnectionString.ShouldNotBeNullOrWhiteSpace();
        }

        [Fact, Trait("Category", "Integration")]
        public void ShouldConnectSuccessfully()
        {
            // Act
            using var result = _sut.Build();
            
            // Assert
            Should.NotThrow(() => result.Open());
        }
    }
}