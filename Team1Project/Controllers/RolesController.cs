// <copyright file="RolesController.cs" company="Principal33 Solutions">
// Copyright (c) Principal33 Solutions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Team1Project.Services;

namespace HelloWorldWeb.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IRoleBroadcastService broadcastService;

        public RolesController(RoleManager<IdentityRole> roleManager, IRoleBroadcastService broadcastService)
        {
            this.roleManager = roleManager;
            this.broadcastService = broadcastService;
        }

        // GET: RolesController
        public ActionResult Index()
        {
            return View(roleManager.Roles);
        }

        // GET: RolesController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: RolesController/Create
        public ActionResult Create()
        {
            return View(new IdentityRole());
        }

        // POST: RolesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IdentityRole role)
        {
            try
            {
                await roleManager.CreateAsync(role);
                broadcastService.RoleCreated(role.Id, role.Name);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
