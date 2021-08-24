using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Team1Project.Services
{
    public class TeamBroadcastService : ITeamBroadcastService
    {
        private readonly IHubContext<TeamMessageHub> messageHub;

        public TeamBroadcastService(IHubContext<TeamMessageHub> messageHub)
        {
            this.messageHub = messageHub;
        }
        public void NewTeamAdded(int id, string jiraBoardUrl, string git, string emblem, string motto)
        {
            messageHub.Clients.All.SendAsync("NewTeamAdded", id, jiraBoardUrl, git, emblem, motto);
        }

        public void TeamDeleted(int id)
        {
            messageHub.Clients.All.SendAsync("TeamDeleted", id);

        }

        public void TeamUpdated(int id, string jiraBoardUrl, string git, string emblem, string motto)
        {
            messageHub.Clients.All.SendAsync("NewTeamAdded", id, jiraBoardUrl, git, emblem, motto);
        }
    }
}
