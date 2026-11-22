using BorrowBox.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace BorrowBox.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly string _connectionString;

        public ReportRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<Report> GetAll()
        {
            var sql = @"SELECT rp.*, i.Title AS ItemTitle, o.FullName AS OwnerName
                        FROM Reports rp
                        JOIN Items i ON i.Id = rp.ItemId
                        JOIN AspNetUsers o ON o.Id = i.OwnerId
                        ORDER BY rp.ReportedAt DESC";

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<Report>(sql).ToList();
            }
        }
    }
}