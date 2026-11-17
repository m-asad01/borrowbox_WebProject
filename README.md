# BorrowBox

Web Technologies Semester Project - Muhammad Asad (BSEF24A034)

- **Deliverable 1:** static pages (HTML + Bootstrap) are in the `Deliverable1-Static` folder.
- **Deliverable 2:** ASP.NET Core MVC (.NET 8) + Identity + SQL Server (LocalDB), in the `BorrowBox` folder.

## How to run
1. Open `BorrowBox.sln` in Visual Studio 2022.
2. Press **F5**.
3. The database `BorrowBoxDb` and the demo data are created automatically on the first run.

## Demo accounts
| Role  | Email                | Password  |
|-------|----------------------|-----------|
| Admin | admin@borrowbox.com  | Admin@123 |
| User  | asad@borrowbox.com   | User@123  |
| User  | ali@example.com      | User@123  |
| User  | sara@example.com     | User@123  |
| User  | usman@example.com    | User@123  (blocked) |

New accounts created from the Sign Up page are normal users.

## Reset the database
Visual Studio > View > SQL Server Object Explorer > (localdb)\MSSQLLocalDB > Databases >
right click `BorrowBoxDb` > Delete (tick "Close existing connections"). Run the project again.
