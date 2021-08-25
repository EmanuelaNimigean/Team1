using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Team1Project.Services
{
    public class InternBroadcastService : IInternBroadcastService
    {
        private readonly IHubContext<InternMessageHub> messageHub;

        public InternBroadcastService(IHubContext<InternMessageHub> messageHub)
        {
            this.messageHub = messageHub;
        }
        public void InternAdded(int id, string name, DateTime birthDate, string emailAddress, string githubUsername, int teamId)
        {
            messageHub.Clients.All.SendAsync("InternAdded", id, name, birthDate, emailAddress, githubUsername, teamId);
        }

        public void InternDeleted(int id)
        {
            messageHub.Clients.All.SendAsync("InternDeleted", id);
        }

        public void InternUpdated(int id, string name, DateTime birthDate, string emailAddress, string githubUsername, int teamId)
        {
            messageHub.Clients.All.SendAsync("InternUpdated", id, name, birthDate, emailAddress, githubUsername, teamId);
        }
    }
}
