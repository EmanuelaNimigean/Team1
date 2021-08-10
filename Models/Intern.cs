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

        public int GetAge(DateTime birthDate)
        {
            TimeSpan age;
            DateTime zeroTime = new DateTime(1, 1, 1);
            age = DateTime.Now - this.BirthDate;
            int years = (zeroTime + age).Year - 1;
            return years;
        }
    }
}
