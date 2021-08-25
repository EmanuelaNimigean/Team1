using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Team1Project.Services
{
    public class InternMessageHub: Hub
    {
        public async Task SendMessage(string intern, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", intern, message);
        }
    }
}