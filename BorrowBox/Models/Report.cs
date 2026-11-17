namespace BorrowBox.Models
{
    // A report against an item (the admin reviews these)
    public class Report
    {
        public int Id { get; set; }

        public int ItemId { get; set; }
        public Item Item { get; set; }

        public string Reason { get; set; }

        public DateTime ReportedAt { get; set; }
    }
}
