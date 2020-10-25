using System;
using AutoFixture;
using MySqlConnector;
using Shouldly;
using Xunit;

namespace LightestNight.System.Data.MySql.Tests
{
    public class MySqlConnectionTests
    {
        private readonly Fixture _fixture;
        private readonly IMySqlConnection _sut;

        public MySqlConnectionTests()
        {
            _fixture = new Fixture();
            var options = _fixture.Build<MySqlOptions>()
                .Without(o => o.Server)
                .Without(o => o.Port)
                .Without(o => o.UserId)
                .Without(o => o.Password)
                .Without(o => o.Database)
                .Without(o => o.Pooling)
                .Without(o => o.MinimumPoolSize)
                .Without(o => o.MaximumPoolSize)
                .Do(o =>
                {
                    o.Server = Environment.GetEnvironmentVariable("MYSQL_SERVER") ?? "localhost";
                    o.Port = Convert.ToUInt32(Environment.GetEnvironmentVariable("MYSQL_PORT") ?? "3306");
                    o.UserId = Environment.GetEnvironmentVariable("MYSQL_USERID") ?? "mysql";
                    o.Password = Environment.GetEnvironmentVariable("MYSQL_PASSWORD") ?? "mysql";
                    o.Database = Environment.GetEnvironmentVariable("MYSQL_DATABASE") ?? "mysql";
                    o.Pooling = false;
                    o.MinimumPoolSize = 1;
                    o.MaximumPoolSize = 1;
                })
                .Create();

            _sut = new MySqlConnection(() => options);
        }

        [Fact, Trait("Category", "Unit")]
        public void ShouldBuildNewConnection()
        {
            // Act
            var result = _sut.GetConnection();
            
            // Assert
            result.ConnectionString.ShouldNotBeNullOrEmpty();
            result.ConnectionString.ShouldNotBeNullOrWhiteSpace();
        }

        [Fact, Trait("Category", "Integration")]
        public void ShouldConnectSuccessfully()
        {
            // Act
            using var result = _sut.GetConnection();
            
            // Assert
            Should.NotThrow(() => result.Open());
        }
        
        [Fact, Trait("Category", "Unit")]
        public void ShouldErrorWhenConnectionFails()
        {
            // Arrange
            var options = _fixture.Create<MySqlOptions>();
            var sut = new MySqlConnection(() => options);
            
            // Act
            Should.Throw<MySqlException>(() => sut.GetConnection());
        }
    }
}