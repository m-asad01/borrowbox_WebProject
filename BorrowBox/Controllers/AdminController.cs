using BorrowBox.Data;
using BorrowBox.Models;
using BorrowBox.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BorrowBox.Controllers
{
    // Only the admin can open this
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Admin home page (dashboard)
        public async Task<IActionResult> Index()
        {
            var members = await _userManager.GetUsersInRoleAsync("User");

            var listingCounts = await _context.Items
                .GroupBy(i => i.OwnerId)
                .Select(g => new { OwnerId = g.Key, Count = g.Count() })
                .ToListAsync();

            var model = new AdminDashboardViewModel();

            model.Users = members
                .OrderBy(u => u.FullName)
                .Select(u => new AdminUserRow
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Email = u.Email,
                    IsBlocked = u.IsBlocked,
                    ListingsCount = listingCounts
                        .Where(c => c.OwnerId == u.Id)
                        .Select(c => c.Count)
                        .FirstOrDefault()
                })
                .ToList();

            model.Items = await _context.Items
                .Include(i => i.Owner)
                .OrderByDescending(i => i.CreatedAt)
                .ToListAsync();

            model.Reports = await _context.Reports
                .Include(r => r.Item).ThenInclude(i => i.Owner)
                .OrderByDescending(r => r.ReportedAt)
                .ToListAsync();

            model.TotalUsers = model.Users.Count;
            model.TotalListings = model.Items.Count;
            model.ReportedCount = model.Reports.Count;
            model.ActiveRequests = await _context.ItemRequests.CountAsync(r => r.Status == "Pending");

            return View(model);
        }

        // Block or unblock a user
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleBlock(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.IsBlocked = !user.IsBlocked;
            await _userManager.UpdateAsync(user);

            TempData["Message"] = user.IsBlocked ? "User blocked." : "User unblocked.";
            return Redirect(Url.Action("Index", "Admin") + "#users");
        }

        // Remove an item (its requests and reports are removed too)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveItem(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _context.Items.Remove(item);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Item removed.";
            return Redirect(Url.Action("Index", "Admin") + "#listings");
        }
    }
}
