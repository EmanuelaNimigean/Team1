using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Team1Project.Models
{
    public class UserTableDTO
    {
        public UserTableDTO(List<UserDTO> users, List<IdentityRole> roles)
        {
            this.Users = users;
            this.Roles = roles;
        }
        public List<UserDTO> Users { get; set; }
        public List<IdentityRole> Roles{ get; set; }
    }
}
