using BorrowBox.Models;

namespace BorrowBox.Repositories
{
    public interface IRequestRepository
    {
        List<ItemRequest> GetByRequester(string userId);
        List<ItemRequest> GetIncoming(string ownerId);
        ItemRequest GetById(int id);
        bool HasPending(int itemId, string userId);
        int CountPending();
        void Add(ItemRequest request);
        void UpdateStatus(int id, string status);
    }
}