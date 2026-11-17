using BorrowBox.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BorrowBox.Data
{
    // Creates the database and adds demo data the first time the app runs
    public static class DbSeeder
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            var context = services.GetRequiredService<ApplicationDbContext>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            // Create the database and all tables if they do not exist
            await context.Database.EnsureCreatedAsync();

            // Roles
            foreach (var roleName in new[] { "Admin", "User" })
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // If users already exist, the demo data is already there
            if (await userManager.Users.AnyAsync())
            {
                return;
            }

            // Users
            var admin = await CreateUser(userManager, "admin@borrowbox.com", "Admin", "0300-0000000", "Admin@123", "Admin", false);
            var asad = await CreateUser(userManager, "asad@borrowbox.com", "Muhammad Asad", "0300-1111111", "User@123", "User", false);
            var ali = await CreateUser(userManager, "ali@example.com", "Ali Raza", "0301-2222222", "User@123", "User", false);
            var sara = await CreateUser(userManager, "sara@example.com", "Sara Khan", "0302-3333333", "User@123", "User", false);
            var usman = await CreateUser(userManager, "usman@example.com", "Usman Tariq", "0303-4444444", "User@123", "User", true);

            // Items (the newest six are shown on the home page)
            var calculator = NewItem("Calculator (Casio fx-991)", "Scientific calculator, works perfectly.", "Electronics", "Borrow", 0, ali.Id, 1);
            var camera = NewItem("Digital Camera (Canon)", "DSLR camera with a 18-55mm lens.", "Electronics", "Rent", 500, sara.Id, 2);
            var dsBook = NewItem("Data Structures Book", "Data Structures using C#, 2nd edition.", "Books", "Borrow", 0, usman.Id, 3);
            var laptop = NewItem("Laptop (Dell Inspiron)", "Core i5, 8GB RAM, good for coding.", "Electronics", "Rent", 800, ali.Id, 4);
            var football = NewItem("Football", "Size 5 football in good condition.", "Sports", "Borrow", 0, sara.Id, 5);
            var racket = NewItem("Badminton Racket Set", "Two rackets and three shuttlecocks.", "Sports", "Rent", 200, usman.Id, 6);
            var arduino = NewItem("Arduino Starter Kit", "Arduino Uno with sensors and wires.", "Electronics", "Rent", 300, asad.Id, 10);
            var osBook = NewItem("Operating Systems Book", "Operating System Concepts by Silberschatz.", "Books", "Borrow", 0, asad.Id, 11);

            context.Items.AddRange(calculator, camera, dsBook, laptop, football, racket, arduino, osBook);
            await context.SaveChangesAsync();

            // Requests
            context.ItemRequests.AddRange(
                new ItemRequest { ItemId = calculator.Id, RequesterId = asad.Id, Days = 2, Status = "Pending", RequestedAt = DateTime.Now.AddDays(-1) },
                new ItemRequest { ItemId = camera.Id, RequesterId = asad.Id, Days = 3, Status = "Approved", RequestedAt = DateTime.Now.AddDays(-2) },
                new ItemRequest { ItemId = dsBook.Id, RequesterId = asad.Id, Days = 7, Status = "Rejected", RequestedAt = DateTime.Now.AddDays(-3) },
                new ItemRequest { ItemId = arduino.Id, RequesterId = ali.Id, Days = 2, Status = "Pending", RequestedAt = DateTime.Now.AddDays(-1) }
            );

            // Reports (shown on the admin page)
            context.Reports.AddRange(
                new Report { ItemId = laptop.Id, Reason = "Wrong description", ReportedAt = DateTime.Now.AddDays(-1) },
                new Report { ItemId = football.Id, Reason = "Fake listing", ReportedAt = DateTime.Now.AddDays(-2) },
                new Report { ItemId = racket.Id, Reason = "Inappropriate price", ReportedAt = DateTime.Now.AddDays(-3) }
            );

            await context.SaveChangesAsync();
        }

        private static async Task<ApplicationUser> CreateUser(UserManager<ApplicationUser> userManager,
            string email, string fullName, string phone, string password, string role, bool isBlocked)
        {
            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                FullName = fullName,
                PhoneNumber = phone,
                IsBlocked = isBlocked
            };

            await userManager.CreateAsync(user, password);
            await userManager.AddToRoleAsync(user, role);
            return user;
        }

        private static Item NewItem(string title, string description, string category,
            string listingType, decimal price, string ownerId, int daysAgo)
        {
            return new Item
            {
                Title = title,
                Description = description,
                Category = category,
                ListingType = listingType,
                PricePerDay = price,
                OwnerId = ownerId,
                CreatedAt = DateTime.Now.AddDays(-daysAgo)
            };
        }
    }
}
