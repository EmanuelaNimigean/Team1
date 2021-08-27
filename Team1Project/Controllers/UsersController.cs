// <copyright file="UsersController.cs" company="Principal33 Solutions">
// Copyright (c) Principal33 Solutions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Team1Project.Models;

namespace HelloWorldWeb.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UsersController : Controller
    {
        public static readonly string ADMIN_ROLE = "Administrator";
        public static readonly string OPERATOR_ROLE = "Operator";
        public static readonly string REGULAR_USER_ROLE = "User";

        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UsersController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await GetUsersWithRole());
        }

        public async Task<IActionResult> AssignRole(string id, string currentRole, string newRole)
        {
            var user = await userManager.FindByIdAsync(id);
            await userManager.RemoveFromRoleAsync(user, currentRole);
            await userManager.AddToRoleAsync(user, newRole);

            return RedirectToAction(nameof(Index));
        }

        private async Task<UserTableDTO> GetUsersWithRole()
        {
            List<UserDTO> users = new List<UserDTO>();
            List<IdentityUser> usersWithRoles = new List<IdentityUser>();

            var allUsers = await userManager.Users.ToListAsync();
            var allRoles = await roleManager.Roles.ToListAsync();

            var admins = await userManager.GetUsersInRoleAsync(ADMIN_ROLE);
            var operators = await userManager.GetUsersInRoleAsync(OPERATOR_ROLE);
            
            foreach(var role in allRoles)
            {
                var currentRoleUsers = await userManager.GetUsersInRoleAsync(role.Name);
                foreach (var user in currentRoleUsers)
                {
                    users.Add(new UserDTO(user.Id, user.Email, role.Name));
                    usersWithRoles.Add(user);
                }
            }

            var regularUsers = allUsers.Except(usersWithRoles).ToList();

            foreach (var user in regularUsers)
            {
                users.Add(new UserDTO(user.Id, user.Email, REGULAR_USER_ROLE));
            }
            users.Sort((user1, user2) => user1.Email.CompareTo(user2.Email));

            return new UserTableDTO(users, allRoles);
        }
    }
}
