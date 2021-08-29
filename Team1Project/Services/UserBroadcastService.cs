using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Team1Project.Services
{
    public class UserBroadcastService :IUserBroadcastService
    {
        private readonly IHubContext<UserMessageHub> messageHub;

        public UserBroadcastService(IHubContext<UserMessageHub> messageHub)
        {
            this.messageHub = messageHub;
        }

        public void UserRoleChanged(string id, string oldRole, string newRole)
        {
            messageHub.Clients.All.SendAsync("UserRoleChanged", id, oldRole, newRole);
        }
    }
}
