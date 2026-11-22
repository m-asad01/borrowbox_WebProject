using BorrowBox.ViewModels;
using Dapper;
using Microsoft.Data.SqlClient;

namespace BorrowBox.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<AdminUserRow> GetMembers()
        {
            var sql = @"SELECT u.Id, u.FullName, u.Email, u.IsBlocked,
                               (SELECT COUNT(*) FROM Items i WHERE i.OwnerId = u.Id) AS ListingsCount
                        FROM AspNetUsers u
                        JOIN AspNetUserRoles ur ON ur.UserId = u.Id
                        JOIN AspNetRoles r ON r.Id = ur.RoleId
                        WHERE r.Name = 'User'
                        ORDER BY u.FullName";

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<AdminUserRow>(sql).ToList();
            }
        }

        public int CountMembers()
        {
            var sql = @"SELECT COUNT(*)
                        FROM AspNetUsers u
                        JOIN AspNetUserRoles ur ON ur.UserId = u.Id
                        JOIN AspNetRoles r ON r.Id = ur.RoleId
                        WHERE r.Name = 'User'";

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.ExecuteScalar<int>(sql);
            }
        }

        public bool ToggleBlock(string id)
        {
            var sql = @"UPDATE AspNetUsers
                        SET IsBlocked = CASE WHEN IsBlocked = 1 THEN 0 ELSE 1 END
                        WHERE Id = @Id;
                        SELECT IsBlocked FROM AspNetUsers WHERE Id = @Id";

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.ExecuteScalar<bool>(sql, new { Id = id });
            }
        }
    }
}