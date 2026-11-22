using BorrowBox.Models;
using BorrowBox.Repositories;
using BorrowBox.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BorrowBox.Controllers
{
    [Authorize(Roles = "User")]
    public class UserController : Controller
    {
        private readonly IItemRepository _itemRepository;
        private readonly IRequestRepository _requestRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(IItemRepository itemRepository, IRequestRepository requestRepository, UserManager<ApplicationUser> userManager)
        {
            _itemRepository = itemRepository;
            _requestRepository = requestRepository;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var model = new UserDashboardViewModel();
            model.FullName = user.FullName;
            model.MyListings = _itemRepository.GetByOwner(user.Id);
            model.MyRequests = _requestRepository.GetByRequester(user.Id);
            model.IncomingRequests = _requestRepository.GetIncoming(user.Id);

            model.MyListingsCount = model.MyListings.Count;
            model.PendingCount = model.MyRequests.Count(r => r.Status == "Pending");
            model.ApprovedCount = model.MyRequests.Count(r => r.Status == "Approved");

            return View(model);
        }
    }
}