using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRChat.Hubs
{
    public class ChatHub : Hub
    {
        public async Task ReceiveUserMssage(string from, string message)
        {
            await Clients.All.SendAsync("broadcastMessage", from, message); //send a 'broadCastMessage
        }
    }
}
