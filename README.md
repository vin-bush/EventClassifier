This is a sample using a language model with semantic kernel to provide classification based on this diagram by R. Jacobs

![Event Classification](https://github.com/vin-bush/EventClassifier/blob/main/eventClassification.png?raw=true "Event Classification")

To run with Ollama (default) - note the functionality doesn't work correctly with Ollama as function calling is model dependant
1. Install Ollama from here https://ollama.com/download
2. Enter 'ollama run llama3.2:1b' into a commandline (downloads and installs the target model)
3. If this works, it can be run using visual studio or dotnet commandline tools

To run with OpenAI
1. Sign up for a free OpenAI Platform account here: https://platform.openai.com/docs/overview
2. Create an api key here: https://platform.openai.com/settings/organization/api-keys
3. Add this api key to the project appsettings
4. Ensure the appsettings llm provider is set to OpenAI
5. Run using visual studio or dotnet commandline tools
