using BorrowBox.Models;
using BorrowBox.Repositories;
using BorrowBox.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BorrowBox.Controllers
{
    [Authorize(Roles = "User")]
    public class ItemsController : Controller
    {
        private readonly IItemRepository _itemRepository;
        private readonly IRequestRepository _requestRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public ItemsController(IItemRepository itemRepository, IRequestRepository requestRepository, UserManager<ApplicationUser> userManager)
        {
            _itemRepository = itemRepository;
            _requestRepository = requestRepository;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new ItemFormViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ItemFormViewModel model)
        {
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

            _itemRepository.Add(item);

            TempData["Message"] = "Your item has been listed.";
            return Redirect(Url.Action("Index", "User") + "#listings");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RequestItem(int itemId, int days)
        {
            var item = _itemRepository.GetById(itemId);
            if (item == null)
            {
                return Json(new { success = false, message = "Item not found." });
            }

            var userId = _userManager.GetUserId(User);

            if (item.OwnerId == userId)
            {
                return Json(new { success = false, message = "You cannot request your own item." });
            }

            if (days < 1 || days > 30)
            {
                return Json(new { success = false, message = "Please choose between 1 and 30 days." });
            }

            if (_requestRepository.HasPending(itemId, userId))
            {
                return Json(new { success = false, message = "You already have a pending request for this item." });
            }

            var request = new ItemRequest
            {
                ItemId = itemId,
                RequesterId = userId,
                Days = days,
                Status = "Pending",
                RequestedAt = DateTime.Now
            };

            _requestRepository.Add(request);

            return Json(new { success = true, message = "Your request has been sent to the owner." });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Respond(int id, bool approve)
        {
            var request = _requestRepository.GetById(id);
            if (request == null)
            {
                return Json(new { success = false, message = "Request not found." });
            }

            var item = _itemRepository.GetById(request.ItemId);
            if (item.OwnerId != _userManager.GetUserId(User))
            {
                return Json(new { success = false, message = "You cannot answer this request." });
            }

            if (request.Status != "Pending")
            {
                return Json(new { success = false, message = "This request is already answered." });
            }

            string status = approve ? "Approved" : "Rejected";
            _requestRepository.UpdateStatus(id, status);

            return Json(new { success = true, message = "Request " + status.ToLower() + ".", status = status });
        }
    }
}