namespace EventClassifier.OpenAI;

public class OpenAIConfig
{
    public const string SectionName = "OpenAI";
    public string ModelId { get; set; } = string.Empty;
    public string EmbeddingModelId { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
}
