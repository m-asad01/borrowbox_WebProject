using System.ComponentModel.DataAnnotations.Schema;

namespace BorrowBox.Models
{
    public class ItemRequest
    {
        public int Id { get; set; }

        public int ItemId { get; set; }
        public Item Item { get; set; }

        public string RequesterId { get; set; }
        public ApplicationUser Requester { get; set; }

        public int Days { get; set; }

        public string Status { get; set; } = "Pending";

        public DateTime RequestedAt { get; set; }

        [NotMapped]
        public string ItemTitle { get; set; }

        [NotMapped]
        public string ItemListingType { get; set; }

        [NotMapped]
        public string OwnerName { get; set; }

        [NotMapped]
        public string RequesterName { get; set; }
    }
}