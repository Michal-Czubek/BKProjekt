using BKProjekt.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;
using System;

namespace BKProjekt.Controllers
{
    public class AdminController : Controller
    {
        private readonly BKProjektDbContext _context;
        private readonly UserManager<BKProjektUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AdminController(UserManager<BKProjektUser> userManager, BKProjektDbContext context, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }


        //metoda wyświetlająca użytkowników
        public async Task<IActionResult> ListUsers()
        {
            var role = await _roleManager.FindByNameAsync("user");
            if (role == null)
            {
                return View(new List<BKProjektUser>());
            }
            var userRoleIds = await _context.UserRoles
                .Where(ur => ur.RoleId == role.Id)
                .Select(ur => ur.UserId)
                .ToListAsync();
            var users = await _userManager.Users
                .Where(u => userRoleIds.Contains(u.Id))
                .ToListAsync();

            return View(users);
        }
        //Metoda wyświetlająca bibliotekarzy
        public async Task<IActionResult> ListKeepers()
        {
            var role = await _roleManager.FindByNameAsync("keeper");
            if (role == null)
            {
                return View(new List<BKProjektUser>());
            }
            var userRoleIds = await _context.UserRoles
                .Where(ur => ur.RoleId == role.Id)
                .Select(ur => ur.UserId)
                .ToListAsync();
            var users = await _userManager.Users
                .Where(u => userRoleIds.Contains(u.Id))
                .ToListAsync();

            return View(users);
        }
        //ustaw użytkownika jako bibliotekarza
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetKeeper(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var isInKeeperRole = await _userManager.IsInRoleAsync(user, "keeper");

            if (!isInKeeperRole)
            {
                await _userManager.RemoveFromRoleAsync(user, "user");
                await _userManager.AddToRoleAsync(user, "keeper");
            }
            return RedirectToAction("ListKeepers");
        }
        //ustaw użytkownika jako zwykłego usera
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var isInKeeperRole = await _userManager.IsInRoleAsync(user, "user");

            if (!isInKeeperRole)
            {
                await _userManager.RemoveFromRoleAsync(user, "keeper");
                await _userManager.AddToRoleAsync(user, "user");
            }
            return RedirectToAction("ListUsers");
        }
        //metoda usuwająca bibliotekarzy
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteKeeper(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                if (await _userManager.IsInRoleAsync(user, "keeper"))
                {
                    await _userManager.DeleteAsync(user);
                }

            }
            return RedirectToAction("ListKeepers");
        }
        //metoda usuwająca użytkowników
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                if(await _userManager.IsInRoleAsync(user, "user"))
                {
                    await _userManager.DeleteAsync(user);
                }
                
            }
            return RedirectToAction("ListUsers");
        }
    }
}
