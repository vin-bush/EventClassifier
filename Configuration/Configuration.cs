using System.Runtime.CompilerServices;
using EventClassifier.Ollama;
using EventClassifier.OpenAI;
using Microsoft.Extensions.Configuration;

namespace EventClassifier;

public sealed class Configuration
{
    private readonly IConfigurationRoot _configRoot;
    private static Configuration? _instance;

    private Configuration(IConfigurationRoot configRoot)
    {
        _configRoot = configRoot;
    }

    public static void Initialize(IConfigurationRoot configRoot)
    {
        _instance = new Configuration(configRoot);
    }

    public static SettingsConfig Settings => LoadSection<SettingsConfig>();
    public static OpenAIConfig OpenAI => LoadSection<OpenAIConfig>();
    public static OllamaConfig Ollama => LoadSection<OllamaConfig>();

    private static T LoadSection<T>([CallerMemberName] string? caller = null)
    {
        if (_instance is null)
        {
            throw new InvalidOperationException(
                "Configuration must be initialized with a call to Initialize(IConfigurationRoot) before accessing configuration values."
            );
        }

        if (string.IsNullOrEmpty(caller))
        {
            throw new ArgumentNullException(nameof(caller));
        }

        return _instance._configRoot.GetSection(caller).Get<T>()
            ?? throw new ConfigurationNotFoundException(section: caller);
    }
}
