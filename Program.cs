using System.Reflection;
using EventClassifier;
using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using OllamaSharp;

IConfigurationRoot configRoot = new ConfigurationBuilder()
    .AddJsonFile("appsettings.Development.json", true)
    .AddEnvironmentVariables()
    .AddUserSecrets(Assembly.GetExecutingAssembly())
    .Build();

Configuration.Initialize(configRoot);

IChatCompletionService chatCompletionService;
if (Configuration.Settings.LlmProvider == "Ollama")
{
#pragma warning disable SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
    chatCompletionService = new OllamaApiClient(
        defaultModel: Configuration.Ollama.ModelId,
        uriString: Configuration.Ollama.Endpoint
    ).AsChatCompletionService();
#pragma warning restore SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
}
else if (Configuration.Settings.LlmProvider == "OpenAI")
{
    chatCompletionService = new OpenAIChatCompletionService(
        modelId: Configuration.OpenAI.ModelId,
        apiKey: Configuration.OpenAI.ApiKey
    );
}
else
{
    throw new InvalidOperationException("Invalid LLM provider selected");
}

EventClassifierAgent agent = new(chatCompletionService);

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
