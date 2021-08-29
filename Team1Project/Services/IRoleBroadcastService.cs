using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Team1Project.Services
{
    public interface IRoleBroadcastService
    {
        public void RoleCreated(string id, string roleName);
    }
}
