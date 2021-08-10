using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Team1Project.Models
{
    public class Team
    {

        public int Id { get; set; }
        public string JiraBoardUrl { get; set; }
        public string Git { get; set; }
        public string Emblem { get; set; }
        public string Motto { get; set; }

        public List<Intern> Interns { get; set; }

    }
}
