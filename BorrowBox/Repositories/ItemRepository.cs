using BorrowBox.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace BorrowBox.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly string _connectionString;

        public ItemRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<Item> Search(string search, string category, string listingType, int take)
        {
            if (string.IsNullOrWhiteSpace(search)) search = null;
            if (string.IsNullOrWhiteSpace(category)) category = null;
            if (string.IsNullOrWhiteSpace(listingType)) listingType = null;

            var sql = @"SELECT TOP (@Take) i.*, u.FullName AS OwnerName
                        FROM Items i
                        JOIN AspNetUsers u ON u.Id = i.OwnerId
                        WHERE (@Search IS NULL OR i.Title LIKE '%' + @Search + '%' OR i.Description LIKE '%' + @Search + '%')
                        AND (@Category IS NULL OR i.Category = @Category)
                        AND (@ListingType IS NULL OR i.ListingType = @ListingType)
                        ORDER BY i.CreatedAt DESC";

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<Item>(sql, new { Search = search, Category = category, ListingType = listingType, Take = take }).ToList();
            }
        }

        public List<Item> GetAll()
        {
            var sql = @"SELECT i.*, u.FullName AS OwnerName
                        FROM Items i
                        JOIN AspNetUsers u ON u.Id = i.OwnerId
                        ORDER BY i.CreatedAt DESC";

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<Item>(sql).ToList();
            }
        }

        public List<Item> GetByOwner(string ownerId)
        {
            var sql = @"SELECT i.*, u.FullName AS OwnerName
                        FROM Items i
                        JOIN AspNetUsers u ON u.Id = i.OwnerId
                        WHERE i.OwnerId = @OwnerId
                        ORDER BY i.CreatedAt DESC";

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<Item>(sql, new { OwnerId = ownerId }).ToList();
            }
        }

        public Item GetById(int id)
        {
            var sql = @"SELECT i.*, u.FullName AS OwnerName
                        FROM Items i
                        JOIN AspNetUsers u ON u.Id = i.OwnerId
                        WHERE i.Id = @Id";

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.QueryFirstOrDefault<Item>(sql, new { Id = id });
            }
        }

        public int Count()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.ExecuteScalar<int>("SELECT COUNT(*) FROM Items");
            }
        }

        public void Add(Item item)
        {
            var sql = @"INSERT INTO Items (Title, Description, Category, ListingType, PricePerDay, CreatedAt, OwnerId)
                        VALUES (@Title, @Description, @Category, @ListingType, @PricePerDay, @CreatedAt, @OwnerId)";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(sql, item);
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute("DELETE FROM Items WHERE Id = @Id", new { Id = id });
            }
        }
    }
}