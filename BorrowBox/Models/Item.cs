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

        [Required]
        public string Category { get; set; }

        [Required]
        public string ListingType { get; set; }

        public decimal PricePerDay { get; set; }

        public DateTime CreatedAt { get; set; }

        public string OwnerId { get; set; }
        public ApplicationUser Owner { get; set; }

        public List<ItemRequest> Requests { get; set; } = new List<ItemRequest>();
        public List<Report> Reports { get; set; } = new List<Report>();

        [NotMapped]
        public string OwnerName { get; set; }

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