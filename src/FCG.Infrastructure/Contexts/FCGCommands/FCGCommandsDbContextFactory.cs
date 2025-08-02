using FCG.Infrastructure._Common.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace FCG.Infrastructure.Contexts.FCGCommands;
public class FCGCommandsDbContextFactory : IDesignTimeDbContextFactory<FCGCommandsDbContext>
{
    private readonly IConfiguration _configuration;

    public FCGCommandsDbContextFactory()
    {
        _configuration = new ConfigurationBuilder()
            .SetBasePath(Project.GetDirectory("FCG.API"))
            .AddJsonFile($"appsettings.json")
            .Build();
    }

    public FCGCommandsDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("FCGCommands");

        if (string.IsNullOrEmpty(connectionString))
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Project.GetDirectory("FCG.API"))
                .AddJsonFile("appsettings.json", optional: true)
                .Build();

            connectionString = configuration.GetConnectionString("FCGCommands");
        }

        var builder = new DbContextOptionsBuilder<FCGCommandsDbContext>();

        builder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

        return new FCGCommandsDbContext(builder.Options);
    }
}
