using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Team1Project.Models;

namespace Team1Project.Services
{
    public interface ITeamBroadcastService
    {
        void NewTeamAdded(int id, string jiraBoardUrl, string git, string emblem, string motto);

        void TeamDeleted(int id);

        void TeamUpdated(int id, string jiraBoardUrl, string git, string emblem, string motto);
    }
}
