using BorrowBox.Models;

namespace BorrowBox.ViewModels
{
    public class UserDashboardViewModel
    {
        public string FullName { get; set; }

        public int MyListingsCount { get; set; }
        public int PendingCount { get; set; }
        public int ApprovedCount { get; set; }

        // Items listed by this user
        public List<Item> MyListings { get; set; } = new List<Item>();

        // Requests sent by this user
        public List<ItemRequest> MyRequests { get; set; } = new List<ItemRequest>();

        // Requests sent by other users for this user's items
        public List<ItemRequest> IncomingRequests { get; set; } = new List<ItemRequest>();
    }
}
