using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Team1Project.Models
{
    public class Intern
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime BirthDate { get; set; }

        public string EmailAddress { get; set; }

        public int TeamId { get; set; }
        public Team Team { get; set; }
        public string GithubUsername { get; set; }

        public int getAge()
        {

            TimeSpan age;
            DateTime birthDate;
            birthDate = this.BirthDate; //new DateTime (2000,07,11);
            DateTime zeroTime = new DateTime(1, 1, 1);
            age = DateTime.Now - birthDate;
            int years = (zeroTime + age).Year - 1;

            return years;
        }
    }
}
