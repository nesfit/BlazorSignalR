using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorChat.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorChat.Pages
{
    public class ChatRoomComponent : ComponentBase
    {
        // flag to indicate chat status
        protected bool IsChatting = false;

        // name of the user who will be chatting
        protected string Username;

        // on-screen message
        protected string Message;

        // new message input
        protected string NewMessage;

        // list of messages in chat
        protected List<MessageModel> Messages = new List<MessageModel>();

        private ChatService _chatService;
        private BotService _botService;

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        public async Task Chat()
        {
            // check username is valid
            if (string.IsNullOrWhiteSpace(Username))
            {
                Message = "Please enter a name";
                return;
            };

            _chatService = new ChatService();
            _botService = new BotService(_chatService, $"{Username}Bot");
            try
            {
                // Start chatting and force refresh UI.
                IsChatting = true;
                await Task.Delay(1);

                // remove old messages if any
                Messages.Clear();

                await _chatService.ConnectAsync(NavigationManager.BaseUri, Username, HandleNewMessage);
                await _chatService.SendAsync($"[Notice] {Username} joined chat room.");
            }
            catch (Exception e)
            {
                Message = $"ERROR: Failed to start chat client: {e.Message}";
                IsChatting = false;
            }
        }

        protected void HandleNewMessage(string senderName, string message)
        {
            bool isMine = senderName.Equals(Username, StringComparison.OrdinalIgnoreCase);

            Messages.Add(new MessageModel(senderName, message, isMine));

            _botService.HandleMessageAsync(senderName, message).ConfigureAwait(false);

            // Inform blazor the UI needs updating
            StateHasChanged();
        }

        protected async Task DisconnectAsync()
        {
            if (IsChatting)
            {
                await _chatService.SendAsync($"[Notice] {Username} left chat room.");
                await _chatService.DisconnectAsync();
                IsChatting = false;
                _chatService = null;
            }
        }

        protected async Task SendAsync(string message)
        {
            if (IsChatting && !string.IsNullOrWhiteSpace(message))
            {
                await _chatService.SendAsync(message);

                NewMessage = string.Empty;
            }
        }

        public class MessageModel
        {
            public MessageModel(string username, string body, bool mine)
            {
                Username = username;
                Body = body;
                Mine = mine;
            }

            public string Username { get; set; }
            public string Body { get; set; }
            public bool Mine { get; set; }

            public bool IsNotice => Body.StartsWith("[Notice]");

            public string CSS => Mine ? "sent" : "received";
        }
    }
}
