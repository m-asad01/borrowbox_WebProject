using BorrowBox.ViewModels;

namespace BorrowBox.Repositories
{
    public interface IUserRepository
    {
        List<AdminUserRow> GetMembers();
        int CountMembers();
        bool ToggleBlock(string id);
    }
}