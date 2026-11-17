using BorrowBox.Data;
using BorrowBox.Models;
using BorrowBox.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BorrowBox.Controllers
{
    // Only members (role User) can add items and send requests
    [Authorize(Roles = "User")]
    public class ItemsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ItemsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // ---------- Add a new item ----------
        [HttpGet]
        public IActionResult Create()
        {
            return View(new ItemFormViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ItemFormViewModel model)
        {
            // A rent item must have a price
            if (model.ListingType == "Rent" && (model.PricePerDay == null || model.PricePerDay <= 0))
            {
                ModelState.AddModelError("PricePerDay", "Please enter a price per day for rent items.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var item = new Item
            {
                Title = model.Title,
                Description = model.Description,
                Category = model.Category,
                ListingType = model.ListingType,
                PricePerDay = model.ListingType == "Rent" ? model.PricePerDay.Value : 0,
                CreatedAt = DateTime.Now,
                OwnerId = _userManager.GetUserId(User)
            };

            _context.Items.Add(item);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Your item has been listed.";
            return Redirect(Url.Action("Index", "User") + "#listings");
        }

        // ---------- Send a request for an item ----------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RequestItem(int itemId, int days)
        {
            var item = await _context.Items.FindAsync(itemId);
            if (item == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);

            if (item.OwnerId == userId)
            {
                TempData["Error"] = "You cannot request your own item.";
                return RedirectToAction("Index", "User");
            }

            if (days < 1 || days > 30)
            {
                TempData["Error"] = "Please choose between 1 and 30 days.";
                return RedirectToAction("Index", "User");
            }

            var alreadyRequested = await _context.ItemRequests.AnyAsync(r =>
                r.ItemId == itemId && r.RequesterId == userId && r.Status == "Pending");

            if (alreadyRequested)
            {
                TempData["Error"] = "You already have a pending request for this item.";
                return RedirectToAction("Index", "User");
            }

            var request = new ItemRequest
            {
                ItemId = itemId,
                RequesterId = userId,
                Days = days,
                Status = "Pending",
                RequestedAt = DateTime.Now
            };

            _context.ItemRequests.Add(request);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Your request has been sent to the owner.";
            return Redirect(Url.Action("Index", "User") + "#requests");
        }

        // ---------- Owner approves or rejects a request ----------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Respond(int id, bool approve)
        {
            var request = await _context.ItemRequests
                .Include(r => r.Item)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (request == null)
            {
                return NotFound();
            }

            // Only the owner of the item can answer the request
            if (request.Item.OwnerId != _userManager.GetUserId(User))
            {
                return Forbid();
            }

            if (request.Status == "Pending")
            {
                request.Status = approve ? "Approved" : "Rejected";
                await _context.SaveChangesAsync();
                TempData["Message"] = approve ? "Request approved." : "Request rejected.";
            }

            return Redirect(Url.Action("Index", "User") + "#incoming");
        }
    }
}
