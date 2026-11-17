using System.ComponentModel.DataAnnotations;

namespace BorrowBox.ViewModels
{
    // Data of the "Add New Item" form
    public class ItemFormViewModel
    {
        [Required(ErrorMessage = "Please enter the item title.")]
        [StringLength(100, ErrorMessage = "Title can be at most 100 characters.")]
        public string Title { get; set; }

        [Required]
        [RegularExpression("^(Books|Electronics|Sports|Other)$", ErrorMessage = "Please select a category.")]
        public string Category { get; set; } = "Books";

        [Required]
        [RegularExpression("^(Borrow|Rent)$", ErrorMessage = "Please select Borrow or Rent.")]
        [Display(Name = "Service")]
        public string ListingType { get; set; } = "Borrow";

        [Display(Name = "Price per day (Rs.)")]
        public decimal? PricePerDay { get; set; }

        [StringLength(500, ErrorMessage = "Description can be at most 500 characters.")]
        public string Description { get; set; }
    }
}
