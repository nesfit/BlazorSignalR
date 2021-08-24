using System;
using System.Threading.Tasks;

namespace BlazorChat.Services
{
    public class BotService 
    {
        private readonly ChatService _chatService;
        private readonly string _botName;

        private static readonly Random Random = new Random(DateTime.Now.Millisecond);

        public BotService(ChatService chatService, string botName)
        {
            _chatService = chatService;
            _botName = botName;
        }

        public async Task HandleMessageAsync(string senderName, string message)
        {
            if(senderName.Equals(_botName)) return;

            if (message.Contains("Ping", StringComparison.CurrentCultureIgnoreCase))
            {
                await _chatService.SendAsync(PingReply[Random.Next(PingReply.Length)], _botName);
            }
        }

        private static readonly string[] PingReply =
        {
            @"Pong",
        };
    }
}