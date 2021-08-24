using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Team1Project.Services
{
    public class TeamMessageHub : Hub
    {
        public async Task SendMessage(string team, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", team, message);
        }
    }
}
