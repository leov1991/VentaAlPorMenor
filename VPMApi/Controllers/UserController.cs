﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using VPMApi.Data;
using VPMApi.Models;
using VPMDataManager.Library.DataAccess;
using VPMDataManager.Library.Models;

namespace VPMApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _config;

        public UserController(ApplicationDbContext context, UserManager<IdentityUser> userManager, IConfiguration config)
        {
            _context = context;
            _userManager = userManager;
            _config = config;
        }


        [HttpGet]
        public UserModel GetById()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            UserData data = new UserData(_config);

            return data.GetUserById(id).First();

        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("admin/all")]
        public List<ApplicationUserModel> GetAllUsers()
        {
            List<ApplicationUserModel> output = new List<ApplicationUserModel>();

            var users = _context.Users.ToList();
            var userRoles = from ur in _context.UserRoles
                            join r in _context.Roles
                            on ur.RoleId equals r.Id
                            select new { ur.UserId, ur.RoleId, r.Name };

            foreach (var user in users)
            {
                ApplicationUserModel u = new ApplicationUserModel
                {
                    Id = user.Id,
                    Email = user.Email
                };

                // Crear el diccionario de roles
                u.Roles = userRoles.Where(x => x.UserId == u.Id).ToDictionary(k => k.RoleId, v => v.Name);

                output.Add(u);
            }



            return output;

        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("admin/roles")]

        public Dictionary<string, string> GetAllRoles()
        {
            var roles = _context.Roles.ToDictionary(x => x.Id, x => x.Name);

            return roles;

        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("admin/addToRole")]
        public async Task AddToRole(UserRolePairModel pair)
        {
            var user = await _userManager.FindByIdAsync(pair.UserId);
            await _userManager.AddToRoleAsync(user, pair.RoleName);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("admin/removeFromRole")]
        public async Task RemoveToRole(UserRolePairModel pair)
        {
            var user = await _userManager.FindByIdAsync(pair.UserId);
            await _userManager.RemoveFromRoleAsync(user, pair.RoleName);

        }

    }
}