using BorrowBox.Repositories;
using BorrowBox.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BorrowBox.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IItemRepository _itemRepository;
        private readonly IRequestRepository _requestRepository;
        private readonly IReportRepository _reportRepository;
        private readonly IUserRepository _userRepository;

        public AdminController(IItemRepository itemRepository, IRequestRepository requestRepository,
                               IReportRepository reportRepository, IUserRepository userRepository)
        {
            _itemRepository = itemRepository;
            _requestRepository = requestRepository;
            _reportRepository = reportRepository;
            _userRepository = userRepository;
        }

        public IActionResult Index()
        {
            var model = new AdminDashboardViewModel();
            model.Users = _userRepository.GetMembers();
            model.Items = _itemRepository.GetAll();
            model.Reports = _reportRepository.GetAll();

            model.TotalUsers = model.Users.Count;
            model.TotalListings = model.Items.Count;
            model.ReportedCount = model.Reports.Count;
            model.ActiveRequests = _requestRepository.CountPending();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ToggleBlock(string id)
        {
            bool isBlocked = _userRepository.ToggleBlock(id);

            TempData["Message"] = isBlocked ? "User blocked." : "User unblocked.";
            return Redirect(Url.Action("Index", "Admin") + "#users");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveItem(int id)
        {
            _itemRepository.Delete(id);

            return Json(new { success = true, message = "Item removed." });
        }
    }
}