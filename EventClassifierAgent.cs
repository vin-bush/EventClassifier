using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace EventClassifier;

public class EventClassifierAgent
{
    private const string _instructions = """
You are an intelligent Events Classification Officer.
You are provided with an event overview and some additional data, your task is to extract key facts from this information.

Respond in JSON format with the following JSON schema:
{
    "eventType": "the event type",
    "eventSize": "the event size",
    "eventDuration": "the event duration",
    "eventComplexity": "the event complexity",
}

Respond with null if there is insufficient information to deduce any of the above attributes.
""";

#pragma warning disable SKEXP0010 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
    private readonly PromptExecutionSettings _promptExecutionSettings =
        new OpenAIPromptExecutionSettings()
        {
            MaxTokens = 500,
            Temperature = 0.1,
            ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions,
            ResponseFormat = "json",
            ChatSystemPrompt =
                @"
Assistant is a large language model. 
This assistant uses plugins to complete it's task.",
        };
#pragma warning restore SKEXP0010 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

    private readonly Kernel _kernel;
    private readonly IChatCompletionService _chatCompletionService;

    public EventClassifierAgent(IChatCompletionService chatCompletionService)
    {
        IKernelBuilder kernelBuilder = Kernel.CreateBuilder();
        kernelBuilder
            .Plugins.Add(KernelPluginFactory.CreateFromType<EventComplexityPlugin>())
            .Add(KernelPluginFactory.CreateFromType<EventDurationPlugin>())
            .Add(KernelPluginFactory.CreateFromType<EventSizePlugin>())
            .Add(KernelPluginFactory.CreateFromType<EventTypePlugin>());

        _kernel = kernelBuilder.Build();
        _chatCompletionService = chatCompletionService;
    }

    public async Task RunAsync(string prompt)
    {
        ChatHistory chat = [];

        chat.AddSystemMessage(_instructions);
        chat.AddUserMessage(prompt);

        ChatMessageContent response = await _chatCompletionService.GetChatMessageContentAsync(
            chat,
            executionSettings: _promptExecutionSettings,
            kernel: _kernel
        );
        chat.Add(response);
        Console.WriteLine($"# {response.Role}: '{response.Content}'");
    }
}
