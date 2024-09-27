using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace EventClassifier;

public class EventComplexityPlugin
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
    public string GetEventComplexity()
    {
        return @"
Simple: 1 attraction, no additional activities
Moderate: multiple attractions in single area, some additional activities and amenities
Complex: multiple attractions across multiple areas, additional activities and amenities
";
    }
}
