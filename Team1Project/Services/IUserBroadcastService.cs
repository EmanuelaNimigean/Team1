using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Team1Project.Services
{
    public interface IUserBroadcastService
    {
        public void UserRoleChanged(string id, string oldRole, string newRole);
    }
}
