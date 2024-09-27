using System.Reflection;
using EventClassifier;
using Microsoft.Extensions.Configuration;

IConfigurationRoot configRoot = new ConfigurationBuilder()
    .AddJsonFile("appsettings.Development.json", true)
    .AddEnvironmentVariables()
    .AddUserSecrets(Assembly.GetExecutingAssembly())
    .Build();

Configuration.Initialize(configRoot);

EventClassifierAgent agent = new();

while (true)
{
    Console.WriteLine("Enter event data:");

    string? eventData = Console.ReadLine();

    if (string.IsNullOrEmpty(eventData))
    {
        break;
    }

    await agent.RunAsync(eventData);
}
