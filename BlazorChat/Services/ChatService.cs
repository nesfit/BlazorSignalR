using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace BlazorChat.Services
{
    public class ChatService
    {
        private string _hubUrl;
        private string _userName;
        private HubConnection _hubConnection;

        public async Task ConnectAsync(string baseUrl, string userName, Action<string, string> onNewMessageHandler)
        {
            _hubUrl = baseUrl.TrimEnd('/') + BlazorChatSampleHub.HubUrl;
            _userName = userName;
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(_hubUrl)
                .Build();

            _hubConnection.On<string, string>(BlazorChatSampleHub.BroadcastMethodName, onNewMessageHandler);

            await _hubConnection.StartAsync();
        }
        
        public async Task SendAsync(string message)
        {
            await _hubConnection.SendAsync(BlazorChatSampleHub.BroadcastMethodName, _userName, message);
        }

        public async Task DisconnectAsync()
        {
            await _hubConnection.StopAsync();
            await _hubConnection.DisposeAsync();
            _hubConnection = null;
        }
    }
}