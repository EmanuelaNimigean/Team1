using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Team1FirstProject.Models;

namespace Team1FirstProject.Data
{
    public class Team1FirstProjectContext : DbContext
    {
        public Team1FirstProjectContext (DbContextOptions<Team1FirstProjectContext> options)
            : base(options)
        {
        }

        public DbSet<Team1FirstProject.Models.Intern> Intern { get; set; }

        public DbSet<Team1FirstProject.Models.Team> Team { get; set; }
    }
}
