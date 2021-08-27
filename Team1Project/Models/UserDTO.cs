using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Team1Project.Models
{
    public class UserDTO
    {
        public UserDTO(string id, string email, string role)
        {
            Id = id;
            Email = email;
            Role = role;
        }

        public string Id { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }
    }
}
