using BorrowBox.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace BorrowBox.Repositories
{
    public class RequestRepository : IRequestRepository
    {
        private readonly string _connectionString;

        public RequestRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<ItemRequest> GetByRequester(string userId)
        {
            var sql = @"SELECT r.*, i.Title AS ItemTitle, i.ListingType AS ItemListingType, o.FullName AS OwnerName
                        FROM ItemRequests r
                        JOIN Items i ON i.Id = r.ItemId
                        JOIN AspNetUsers o ON o.Id = i.OwnerId
                        WHERE r.RequesterId = @UserId
                        ORDER BY r.RequestedAt DESC";

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<ItemRequest>(sql, new { UserId = userId }).ToList();
            }
        }

        public List<ItemRequest> GetIncoming(string ownerId)
        {
            var sql = @"SELECT r.*, i.Title AS ItemTitle, u.FullName AS RequesterName
                        FROM ItemRequests r
                        JOIN Items i ON i.Id = r.ItemId
                        JOIN AspNetUsers u ON u.Id = r.RequesterId
                        WHERE i.OwnerId = @OwnerId
                        ORDER BY r.RequestedAt DESC";

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<ItemRequest>(sql, new { OwnerId = ownerId }).ToList();
            }
        }

        public ItemRequest GetById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.QueryFirstOrDefault<ItemRequest>("SELECT * FROM ItemRequests WHERE Id = @Id", new { Id = id });
            }
        }

        public bool HasPending(int itemId, string userId)
        {
            var sql = "SELECT COUNT(*) FROM ItemRequests WHERE ItemId = @ItemId AND RequesterId = @UserId AND Status = 'Pending'";

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.ExecuteScalar<int>(sql, new { ItemId = itemId, UserId = userId }) > 0;
            }
        }

        public int CountPending()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.ExecuteScalar<int>("SELECT COUNT(*) FROM ItemRequests WHERE Status = 'Pending'");
            }
        }

        public void Add(ItemRequest request)
        {
            var sql = @"INSERT INTO ItemRequests (ItemId, RequesterId, Days, Status, RequestedAt)
                        VALUES (@ItemId, @RequesterId, @Days, @Status, @RequestedAt)";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(sql, request);
            }
        }

        public void UpdateStatus(int id, string status)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute("UPDATE ItemRequests SET Status = @Status WHERE Id = @Id", new { Status = status, Id = id });
            }
        }
    }
}