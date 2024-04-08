using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Infrastructure.Contexts;

public class DataContextFactory : IDesignTimeDbContextFactory<ApiContext>
{
    public ApiContext CreateDbContext(string[] args)
    {
        var configurationPath = "/Users/lucaswictor/RiderProjects/Silicon_WebApi/WebApi/appsettings.json";
        var configDirectory = Path.GetDirectoryName(configurationPath);
        var configFile = Path.GetFileName(configurationPath);
        
        if (configDirectory == null || configFile == null)
        {
            throw new InvalidOperationException("Could not find the configuration file path.");
        }

        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(configDirectory)
            .AddJsonFile(configFile, optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var builder = new DbContextOptionsBuilder<ApiContext>();
        var connectionString = configuration.GetConnectionString("PostgresConnection");
        builder.UseNpgsql(connectionString);

        return new ApiContext(builder.Options);
    }
}