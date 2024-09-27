using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace EventClassifier;

public class EventDurationPlugin
{
    public const string CorrelationIdArgument = "correlationId";

    private readonly List<string> _correlationIds = [];

    public IReadOnlyList<string> CorrelationIds => _correlationIds;

    [KernelFunction]
    [Description("Provides a list of possible event durations.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Design",
        "CA1024:Use properties where appropriate",
        Justification = "Too smart"
    )]
    public string GetEventDurations()
    {
        return @"
Single Day
Multiple Days
";
    }
}
