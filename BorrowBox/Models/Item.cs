using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BorrowBox.Models
{
    public class Item
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        // Books, Electronics, Sports or Other
        [Required]
        public string Category { get; set; }

        // Borrow (free) or Rent (paid)
        [Required]
        public string ListingType { get; set; }

        // Only used when ListingType is Rent
        public decimal PricePerDay { get; set; }

        public DateTime CreatedAt { get; set; }

        // The user who owns this item
        public string OwnerId { get; set; }
        public ApplicationUser Owner { get; set; }

        public List<ItemRequest> Requests { get; set; } = new List<ItemRequest>();
        public List<Report> Reports { get; set; } = new List<Report>();

        // Icon shown on the item card (chosen from the category)
        [NotMapped]
        public string IconClass
        {
            get
            {
                if (Category == "Books") return "bi-book";
                if (Category == "Electronics") return "bi-laptop";
                if (Category == "Sports") return "bi-trophy";
                return "bi-box-seam";
            }
        }
    }
}
