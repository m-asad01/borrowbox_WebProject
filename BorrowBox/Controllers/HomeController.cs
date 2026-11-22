using BorrowBox.Repositories;
using BorrowBox.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BorrowBox.Controllers
{
    public class HomeController : Controller
    {
        private readonly IItemRepository _itemRepository;
        private readonly IUserRepository _userRepository;

        public HomeController(IItemRepository itemRepository, IUserRepository userRepository)
        {
            _itemRepository = itemRepository;
            _userRepository = userRepository;
        }

        public IActionResult Index(string search, string category, string type)
        {
            var model = new HomeViewModel
            {
                Search = search,
                Category = category,
                ListingType = type
            };

            int take = model.HasFilter ? 100 : 6;

            model.Items = _itemRepository.Search(search, category, type, take);
            model.ItemCount = _itemRepository.Count();
            model.MemberCount = _userRepository.CountMembers();

            return View(model);
        }

        public IActionResult SearchItems(string search, string category, string type)
        {
            bool hasFilter = !string.IsNullOrWhiteSpace(search)
                          || !string.IsNullOrWhiteSpace(category)
                          || !string.IsNullOrWhiteSpace(type);

            int take = hasFilter ? 100 : 6;

            var items = _itemRepository.Search(search, category, type, take);

            return PartialView("_ItemList", items);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}