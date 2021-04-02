using EasyInterview.API.SignalR.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace EasyInterview.API.SignalR
{
    public class SignalRtcHub : Hub
    {
        public async Task NewUser(UserModel newUser)
        {
            var userInfo = new UserInfo() {
                Name = newUser.Name,
                Picture = newUser.Picture,
                ConnectionId = Context.ConnectionId 
            };
            await Clients.Others.SendAsync("NewUserArrived", JsonSerializer.Serialize(userInfo));
        }

        public async Task HelloUser(UserModel user, string recipientConnectionId)
        {
            var userInfo = new UserInfo()
            {
                Name = user.Name,
                Picture = user.Picture,
                ConnectionId = Context.ConnectionId
            };
            await Clients.Client(recipientConnectionId).SendAsync("UserSaidHello", JsonSerializer.Serialize(userInfo));
        }

        public async Task SendSignal(string signal, string connectionId)
        {
            await Clients.Client(connectionId).SendAsync("SendSignal", Context.ConnectionId, signal);
        }

        public override async Task OnDisconnectedAsync(System.Exception exception)
        {
            await Clients.All.SendAsync("UserDisconnect", Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
