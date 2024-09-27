using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace EventClassifier;

public class EventTypePlugin
{
    public const string CorrelationIdArgument = "correlationId";

    private readonly List<string> _correlationIds = [];

    public IReadOnlyList<string> CorrelationIds => _correlationIds;

    [KernelFunction]
    [Description("Provides a list of possible event types.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Design",
        "CA1024:Use properties where appropriate",
        Justification = "Too smart"
    )]
    public string GetEventTypes()
    {
        return @"
Conference
Trade Show
Corporate
Cultural
Public
";
    }
}
