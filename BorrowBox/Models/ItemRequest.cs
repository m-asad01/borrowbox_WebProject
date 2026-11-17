namespace BorrowBox.Models
{
    // A request sent by a user to borrow or rent an item
    public class ItemRequest
    {
        public int Id { get; set; }

        public int ItemId { get; set; }
        public Item Item { get; set; }

        public string RequesterId { get; set; }
        public ApplicationUser Requester { get; set; }

        public int Days { get; set; }

        // Pending, Approved or Rejected
        public string Status { get; set; } = "Pending";

        public DateTime RequestedAt { get; set; }
    }
}
