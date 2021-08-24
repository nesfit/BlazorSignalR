using System;
using System.Threading.Tasks;

namespace BlazorChat.Services
{
    public class BotService 
    {
        private readonly ChatService _chatService;
        private readonly string _botName;

        public BotService(ChatService chatService, string botName)
        {
            _chatService = chatService;
            _botName = botName;
        }

        public async Task HandleMessageAsync(string senderName, string message)
        {
            if(senderName.Equals(_botName)) return;

            if (message.Contains("Joke", StringComparison.CurrentCultureIgnoreCase))
            {
                Random random = new Random(DateTime.Now.Millisecond);
                await _chatService.SendAsync(Jokes[random.Next(Jokes.Length)], _botName);
            }
        }

        private static readonly string[] Jokes =
        {
            @"Algorithm... Word used by programmers when they don't want to explain what they did.",
            @"""Knock, knock."" ... ""Who’s there?"" ... very long pause... ""Java.""",
            @"Its not a bug, its a feature.",
            @"Q: how many programmers does it take to change a light bulb? ... A: none, that's a hardware problem",
            @"Q: ""Whats the object-oriented way to become wealthy?"" ...  A: Inheritance",
            @" [""hip"",""hip""] ... (hip hip array!)"
        };
    }
}