using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.AI;
using IcmChatAPI.Models;
using ChatResponse = IcmChatAPI.Models.ChatResponse;
namespace IcmChatAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController: ControllerBase
    {
        private readonly IChatClient _chatClient;
        public ChatController(IChatClient chatClient)
        {
            _chatClient = chatClient;
        }
        [HttpPost]
        public async Task<ChatResponse> SendQuery([FromBody] ChatRequest query)
        {
            List<ChatMessage> messages = [ 
                new ChatMessage(ChatRole.System, "You are a helpful assistant."),
                new ChatMessage(ChatRole.User, query.Query)
            ];
            var response = await _chatClient.GetResponseAsync(messages, new ChatOptions());
            return new ChatResponse
            {
                Message = response.Text,
                Status = "Success"
            };
        }
    }
}