using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace BlazorChat
{
    public class BlazorChatSampleHub : Hub
    {
        public const string HubUrl = "/chat";
        public const string BroadcastMethodName = "Broadcast";


        public async Task Broadcast(string username, string message)
        {
            await Clients.All.SendAsync(BroadcastMethodName, username, message);
        }

        public override Task OnConnectedAsync()
        {
            Console.WriteLine($"{Context.ConnectionId} connected");
            return base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception e)
        {
            Console.WriteLine($"Disconnected {e?.Message} {Context.ConnectionId}");
            await base.OnDisconnectedAsync(e);
        }
    }
}