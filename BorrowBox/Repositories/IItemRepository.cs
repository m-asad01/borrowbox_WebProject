using BorrowBox.Models;

namespace BorrowBox.Repositories
{
    public interface IItemRepository
    {
        List<Item> Search(string search, string category, string listingType, int take);
        List<Item> GetAll();
        List<Item> GetByOwner(string ownerId);
        Item GetById(int id);
        int Count();
        void Add(Item item);
        void Delete(int id);
    }
}