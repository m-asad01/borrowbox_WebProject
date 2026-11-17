using BorrowBox.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BorrowBox.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Item> Items { get; set; }
        public DbSet<ItemRequest> ItemRequests { get; set; }
        public DbSet<Report> Reports { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Item>()
                .Property(i => i.PricePerDay)
                .HasColumnType("decimal(10,2)");

            // A user cannot be deleted while he still owns items or has requests
            builder.Entity<Item>()
                .HasOne(i => i.Owner)
                .WithMany()
                .HasForeignKey(i => i.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ItemRequest>()
                .HasOne(r => r.Requester)
                .WithMany()
                .HasForeignKey(r => r.RequesterId)
                .OnDelete(DeleteBehavior.Restrict);

            // When an item is removed, its requests and reports are removed too
            builder.Entity<ItemRequest>()
                .HasOne(r => r.Item)
                .WithMany(i => i.Requests)
                .HasForeignKey(r => r.ItemId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Report>()
                .HasOne(r => r.Item)
                .WithMany(i => i.Reports)
                .HasForeignKey(r => r.ItemId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
