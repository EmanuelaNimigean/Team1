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

        // GET: Users/Details/[ugly string id]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await userManager.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> AssignAdminRole(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            await userManager.AddToRoleAsync(user, "Administrator");
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> AssignRegularUserRole(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            await userManager.RemoveFromRoleAsync(user, "Administrator");
            return RedirectToAction(nameof(Index));
        }

        private async Task<List<UserDTO>> GetUsersWithRole()
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

            return users;
        }
    }
}
