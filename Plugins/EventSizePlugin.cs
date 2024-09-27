using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace EventClassifier;

public class EventSizePlugin
{
    public const string CorrelationIdArgument = "correlationId";

    private readonly List<string> _correlationIds = [];

    public IReadOnlyList<string> CorrelationIds => _correlationIds;

    [KernelFunction]
    [Description("Provides a list of possible event sizes.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Design",
        "CA1024:Use properties where appropriate",
        Justification = "Too smart"
    )]
    public string GetEventSizes()
    {
        return @"
Small: 1-99 attendees
Medium: 100-999 attendees
Large: 1,000-99,999 attendees
Mega: 100,000+ attendees
";
    }
}
