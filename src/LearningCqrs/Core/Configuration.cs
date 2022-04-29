namespace LearningCqrs.Core;

public static class Configuration
{
    public static string GetConnectionString(string configurationName)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        return configuration.GetConnectionString(configurationName);
    }

    public static string Get(string configurationName)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        return configuration.GetValue<string>(configurationName);
    }
}