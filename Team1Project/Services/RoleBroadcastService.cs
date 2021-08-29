using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Team1Project.Services
{
    public class RoleBroadcastService : IRoleBroadcastService
    {
        private readonly IHubContext<UserMessageHub> messageHub;

        public RoleBroadcastService(IHubContext<UserMessageHub> messageHub)
        {
            this.messageHub = messageHub;
        }
        public void RoleCreated(string id, string roleName)
        {
            messageHub.Clients.All.SendAsync("RoleCreated", id, roleName);
        }
    }
}
