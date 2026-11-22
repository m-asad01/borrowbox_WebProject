using BorrowBox.Models;

namespace BorrowBox.Repositories
{
    public interface IReportRepository
    {
        List<Report> GetAll();
    }
}