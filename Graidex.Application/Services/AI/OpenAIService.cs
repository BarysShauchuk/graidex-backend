using Azure.AI.OpenAI;
using Graidex.Application.DTOs.AI;
using Graidex.Application.OneOfCustomTypes;
using Graidex.Domain.Models.Tests.Answers;
using Graidex.Domain.Models.Tests.Questions;
using OneOf;
using System.Text.RegularExpressions;

namespace Graidex.Application.Services.AI
{
    public class OpenAIService : IAIService
    {
        public readonly OpenAIClient client;

        private const string AiChatModelName_gpt4 = "gpt-4-0125-preview";
        private const string AiChatModelName = AiChatModelName_gpt4;
        private const int AiChatMaxRequests = 3;

        public OpenAIService(OpenAIClient client)
        {
            this.client = client;
        }

        public async Task<OneOf<EvaluateOpenAnswerDto, ConditionFailed>> EvaluateOpenAnswerAsync(OpenQuestion question, OpenAnswer answer, CancellationToken cancellationToken)
        {
            var options = new ChatCompletionsOptions 
            {   
                Messages =
                {
                    new ChatRequestSystemMessage("Evaluate the Answer to the Question from 0 to 10. 0 means that the answer is completely wrong, 10 means that the answer is completely correct. " +
                    "If the question contains any instructions on how it should be answered, follow them while evaluating. " +
                    "If the question contains several sub-questions, all of them must be answered to get the full score." +
                    "Question can be answered briefly, unless requested otherwise. " +
                    "Reply only with the integer score number, followed by a semicolon. " +
                    "If the answer is not fully correct, after the semicolon, very briefly write the missing part of the correct Answer for the Question."),
                    new ChatRequestUserMessage($"Question: {question.Text}, Answer: {answer.Text}")
                }
            };

            options.DeploymentName = AiChatModelName;
            var responsePattern = @"^([0-9]|10);\s*(.*)$";
            Regex regex = new Regex(responsePattern);

            for (int i = 0; i < AiChatMaxRequests; i++)
            {
                Azure.Response<ChatCompletions> response;

                try 
                {
                    response = await client.GetChatCompletionsAsync(options, cancellationToken); 
                }

                catch (Azure.RequestFailedException e)
                {
                    return new ConditionFailed(e.Message);
                }

                var responseChoice = response.Value.Choices[0];

                var match = regex.Match(responseChoice.Message.Content);

                if (match.Success)
                {
                    return new EvaluateOpenAnswerDto
                    {
                        Points = int.Parse(match.Groups[1].Value),
                        Feedback = match.Groups[2].Value
                    };
                }
            }

            return new ConditionFailed("OpenAI is unable to assess this answer");
        }
    }
}
