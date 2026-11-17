using BorrowBox.Models;

namespace BorrowBox.ViewModels
{
    public class HomeViewModel
    {
        public List<Item> Items { get; set; } = new List<Item>();
        public int MemberCount { get; set; }
        public int ItemCount { get; set; }

        // Search values (empty when the visitor is not searching)
        public string Search { get; set; }
        public string Category { get; set; }
        public string ListingType { get; set; }

        public bool HasFilter
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Search)
                    || !string.IsNullOrWhiteSpace(Category)
                    || !string.IsNullOrWhiteSpace(ListingType);
            }
        }
    }
}
