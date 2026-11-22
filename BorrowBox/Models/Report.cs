using System.ComponentModel.DataAnnotations.Schema;

namespace BorrowBox.Models
{
    public class Report
    {
        public int Id { get; set; }

        public int ItemId { get; set; }
        public Item Item { get; set; }

        public string Reason { get; set; }

        public DateTime ReportedAt { get; set; }

        [NotMapped]
        public string ItemTitle { get; set; }

        [NotMapped]
        public string OwnerName { get; set; }
    }
}