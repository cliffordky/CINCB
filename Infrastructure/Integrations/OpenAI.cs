using Core.Models;
using Core.Models.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OpenAI;
using OpenAI.Assistants;
using OpenAI.Files;
using System.ClientModel;
using System.Text;

namespace Infrastructure.Integrations
{
    public class OpenAI : Core.Integrations.IOpenAI
    {
        private readonly ILogger<OpenAI> _logger;
        private readonly IOptions<OpenAISettings> _aiSettings;

        public OpenAI(ILogger<OpenAI> logger, IOptions<OpenAISettings> aiSettings)
        {
            _logger = logger;
            _aiSettings = aiSettings;
        }

        public async Task<Ardalis.Result.Result<string>> GenerateCronExpression(string prompt)
        {
            string cronExpression = string.Empty;

#pragma warning disable OPENAI001
            OpenAIClient openAIClient = new(_aiSettings.Value.OpenAIKey);
            OpenAIFileClient fileClient = openAIClient.GetOpenAIFileClient();
            AssistantClient assistantClient = openAIClient.GetAssistantClient();
            StringBuilder instructions = new StringBuilder();
            instructions.AppendFormat($"You are an assistant that is able to build a cron expression based on a user input. The ASSISTANT should return nothing but the cron expression.  This must be a valid cron expression only.  If the USER input uses the word schedule (or similar), please assume that they mean cron expression.");

            // Now, we'll create a client intended to help with that data
            AssistantCreationOptions assistantOptions = new()
            {
                Name = $"Cron Builder",
                Instructions = instructions.ToString(),
            };

            Assistant assistant = assistantClient.CreateAssistant("gpt-4o", assistantOptions);
            // Now we'll create a thread with a user query about the data already associated with the assistant, then run it
            ThreadCreationOptions threadOptions = new()
            {
                InitialMessages = { prompt }
            };

            ThreadRun threadRun = assistantClient.CreateThreadAndRun(assistant.Id, threadOptions);

            // Check back to see when the run is done
            do
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));
                threadRun = assistantClient.GetRun(threadRun.ThreadId, threadRun.Id);
            } while (!threadRun.Status.IsTerminal);

            // Finally, we'll print out the full history for the thread that includes the augmented generation
            CollectionResult<ThreadMessage> messages
                = assistantClient.GetMessages(threadRun.ThreadId, new MessageCollectionOptions() { Order = MessageCollectionOrder.Ascending });

            foreach (ThreadMessage message in messages)
            {
                if (message.Role.ToString().ToUpper() != "ASSISTANT") continue;

                Console.Write($"[{message.Role.ToString().ToUpper()}]: ");
                foreach (MessageContent contentItem in message.Content)
                {
                    if (!string.IsNullOrEmpty(contentItem.Text))
                    {
                        cronExpression = contentItem.Text;
                        Console.WriteLine($"{contentItem.Text}");

                        if (contentItem.TextAnnotations.Count > 0)
                        {
                            Console.WriteLine();
                        }

                        // Include annotations, if any.
                        foreach (TextAnnotation annotation in contentItem.TextAnnotations)
                        {
                            if (!string.IsNullOrEmpty(annotation.InputFileId))
                            {
                                Console.WriteLine($"* File citation, file ID: {annotation.InputFileId}");
                            }
                            if (!string.IsNullOrEmpty(annotation.OutputFileId))
                            {
                                Console.WriteLine($"* File output, new file ID: {annotation.OutputFileId}");
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(contentItem.ImageFileId))
                    {
                        OpenAIFile imageInfo = fileClient.GetFile(contentItem.ImageFileId);
                        BinaryData imageBytes = fileClient.DownloadFile(contentItem.ImageFileId);
                        using FileStream stream = File.OpenWrite($"{imageInfo.Filename}.png");
                        imageBytes.ToStream().CopyTo(stream);

                        Console.WriteLine($"<image: {imageInfo.Filename}.png>");
                    }
                }
                Console.WriteLine();
            }

            return await System.Threading.Tasks.Task.FromResult(Ardalis.Result.Result<string>.Success(cronExpression));
        }
    }
}