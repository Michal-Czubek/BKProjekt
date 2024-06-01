using BKProjekt.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BKProjekt.Controllers
{
    public class UserListController : Controller
    {
        private readonly BKProjektDbContext _context;
        private readonly UserManager<BKProjektUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserListController(UserManager<BKProjektUser> userManager, BKProjektDbContext context, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> UserList()
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

        
        [HttpPost]
        public async Task<IActionResult> BorrowsList(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var borrows = await _context.Borrows
                .Include(b => b.Book)
                .Where(b => b.UserId == id)
                .OrderByDescending(b => b.BorrowDate)
                .ToListAsync();

            ViewBag.UserName = user.UserName;
            return View(borrows);
        }
    }
}
