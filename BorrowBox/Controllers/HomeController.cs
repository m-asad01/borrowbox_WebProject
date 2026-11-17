using BorrowBox.Data;
using BorrowBox.Models;
using BorrowBox.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BorrowBox.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Public home page. Also used for searching items.
        public async Task<IActionResult> Index(string search, string category, string type)
        {
            var query = _context.Items.Include(i => i.Owner).AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(i => i.Title.Contains(search) || i.Description.Contains(search));
            }

            if (!string.IsNullOrWhiteSpace(category))
            {
                query = query.Where(i => i.Category == category);
            }

            if (!string.IsNullOrWhiteSpace(type))
            {
                query = query.Where(i => i.ListingType == type);
            }

            query = query.OrderByDescending(i => i.CreatedAt);

            var model = new HomeViewModel
            {
                Search = search,
                Category = category,
                ListingType = type
            };

            // Without a search, show only the latest 6 items
            if (!model.HasFilter)
            {
                query = query.Take(6);
            }

            model.Items = await query.ToListAsync();
            model.ItemCount = await _context.Items.CountAsync();
            model.MemberCount = (await _userManager.GetUsersInRoleAsync("User")).Count;

            return View(model);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
