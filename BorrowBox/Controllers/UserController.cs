using BorrowBox.Data;
using BorrowBox.Models;
using BorrowBox.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BorrowBox.Controllers
{
    // Only logged-in members (role User) can open this
    [Authorize(Roles = "User")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // User home page (dashboard)
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var model = new UserDashboardViewModel();
            model.FullName = user.FullName;

            model.MyListings = await _context.Items
                .Where(i => i.OwnerId == user.Id)
                .OrderByDescending(i => i.CreatedAt)
                .ToListAsync();

            model.MyRequests = await _context.ItemRequests
                .Include(r => r.Item).ThenInclude(i => i.Owner)
                .Where(r => r.RequesterId == user.Id)
                .OrderByDescending(r => r.RequestedAt)
                .ToListAsync();

            model.IncomingRequests = await _context.ItemRequests
                .Include(r => r.Item)
                .Include(r => r.Requester)
                .Where(r => r.Item.OwnerId == user.Id)
                .OrderByDescending(r => r.RequestedAt)
                .ToListAsync();

            model.MyListingsCount = model.MyListings.Count;
            model.PendingCount = model.MyRequests.Count(r => r.Status == "Pending");
            model.ApprovedCount = model.MyRequests.Count(r => r.Status == "Approved");

            return View(model);
        }
    }
}
