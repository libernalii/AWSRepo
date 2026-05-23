using Amazon.BedrockRuntime;
using Amazon.BedrockRuntime.Model;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CinemaAPI.Controllers
{
    [ApiController]
    [Route("ai")]
    public class AIController : ControllerBase
    {
        private readonly IAmazonBedrockRuntime _bedrockClient;
        private readonly IAmazonDynamoDB _dynamoDbClient;
        private readonly string _tableName;

        // Вимоги ТЗ: Модель Meta Llama 3 
        private const string ModelId = "meta.llama3-1-8b-instruct-v1:0";

        public AIController(IAmazonBedrockRuntime bedrockClient, IAmazonDynamoDB dynamoDbClient, IConfiguration configuration)
        {
            _bedrockClient = bedrockClient;
            _dynamoDbClient = dynamoDbClient;
            _tableName = configuration["AWS:DynamoDbTable"] ?? "CinemaUsers";
        }

        [HttpPost("recommend")]
        public async Task<IActionResult> Recommend([FromBody] PromptRequest request)
        {
            if (string.IsNullOrEmpty(request.Prompt))
                return BadRequest("Промпт не може бути порожнім.");

            // Системний промпт для ШІ (вимога ТЗ щодо кастомізації)
            string systemPrompt = "Ти — інтелектуальний AI-помічник кінотеатру CinemaAPI. Твоє завдання — рекомендувати фільми на основі запиту користувача. Відповідай виключно українською мовою, лаконічно та професійно.";

            // Формування запиту через Converse API (найновіший стандарт AWS SDK)
            var converseRequest = new ConverseRequest
            {
                ModelId = ModelId,
                Messages = new List<Message>
                {
                    new Message
                    {
                        Role = ConversationRole.User,
                        Content = new List<ContentBlock>
                        {
                            new ContentBlock { Text = $"Порекомендуй фільм під такий настрій/опис: {request.Prompt}" }
                        }
                    }
                },
                System = new List<SystemContentBlock> { new SystemContentBlock { Text = systemPrompt } },
                InferenceConfig = new InferenceConfiguration
                {
                    Temperature = 0.6f,
                    MaxTokens = 250
                }
            };

            try
            {
                // 1. Виклик AWS Bedrock
                var response = await _bedrockClient.ConverseAsync(converseRequest);
                string aiResponseText = response.Output.Message.Content[0].Text;

                // 2. Збереження логу запиту в DynamoDB (Виконання критерію №3 ТЗ)
                await SaveLogToDynamoDb(request.Prompt, aiResponseText);

                return Ok(new
                {
                    Recommendation = aiResponseText,
                    Region = "eu-north-1 (Stockholm)",
                    Model = "Meta Llama 3 8B"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Помилка при зверненні до AWS сервісів",
                    error = ex.Message
                });
            }
        }

        private async Task SaveLogToDynamoDb(string userPrompt, string aiResponse)
        {
            var item = new Dictionary<string, AttributeValue>
            {
                // Використовуємо UserId як первинний ключ, який ти створила
                { "UserId", new AttributeValue { S = Guid.NewGuid().ToString() } },
                { "UserPrompt", new AttributeValue { S = userPrompt } },
                { "AiResponse", new AttributeValue { S = aiResponse } },
                { "Timestamp", new AttributeValue { S = DateTime.UtcNow.ToString("o") } }
            };

            var putRequest = new PutItemRequest
            {
                TableName = _tableName,
                Item = item
            };

            await _dynamoDbClient.PutItemAsync(putRequest);
        }
    }

    public class PromptRequest
    {
        public string Prompt { get; set; }
    }
}