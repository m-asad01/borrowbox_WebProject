using BorrowBox.Models;

namespace BorrowBox.ViewModels
{
    public class AdminUserRow
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public int ListingsCount { get; set; }
        public bool IsBlocked { get; set; }
    }

    public class AdminDashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalListings { get; set; }
        public int ActiveRequests { get; set; }
        public int ReportedCount { get; set; }

        public List<AdminUserRow> Users { get; set; } = new List<AdminUserRow>();
        public List<Item> Items { get; set; } = new List<Item>();
        public List<Report> Reports { get; set; } = new List<Report>();
    }
}
