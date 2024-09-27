using System.Text.Json;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
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

    public async Task RunAsync(string UserPrompt)
    {
        // Create a kernel with OpenAI chat completion
#pragma warning disable SKEXP0110 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
#pragma warning disable SKEXP0010 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
        ChatCompletionAgent agent =
            new()
            {
                Name = "EventClassifier",
                Instructions = _instructions,
                Kernel = Kernel
                    .CreateBuilder()
                    .AddOpenAIChatCompletion(
                        modelId: Configuration.OpenAI.ChatModelId,
                        apiKey: Configuration.OpenAI.ApiKey
                    )
                    .Build(),
                Arguments = new KernelArguments(
                    new OpenAIPromptExecutionSettings
                    {
                        MaxTokens = 500,
                        Temperature = 0.5,
                        ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions,
                        ResponseFormat = "json_object",
                    }
                ),
            };

        // Initialize plugins and add to the agent's Kernel (same as direct Kernel usage).
        agent.Kernel.Plugins.Add(KernelPluginFactory.CreateFromType<EventComplexityPlugin>());
        agent.Kernel.Plugins.Add(KernelPluginFactory.CreateFromType<EventDurationPlugin>());
        agent.Kernel.Plugins.Add(KernelPluginFactory.CreateFromType<EventSizePlugin>());
        agent.Kernel.Plugins.Add(KernelPluginFactory.CreateFromType<EventTypePlugin>());

        // Create the chat history to capture the agent interaction.
        ChatHistory chat = [];

        // Respond to user input, invoking functions where appropriate.
        await InvokeAgentAsync(UserPrompt);

        // Local function to invoke agent and display the conversation messages.
        async Task InvokeAgentAsync(string input)
        {
            chat.Add(new ChatMessageContent(AuthorRole.User, input));

            await foreach (ChatMessageContent content in agent.InvokeAsync(chat))
            {
                chat.Add(content);
                Console.WriteLine($"# {content.Role}: '{content.Content}'");
            }
        }

#pragma warning restore SKEXP0110 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
#pragma warning restore SKEXP0010 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
    }
}
