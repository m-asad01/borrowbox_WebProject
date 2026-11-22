# BorrowBox

Web Technologies Semester Project - Muhammad Asad (BSEF24A034)

BorrowBox is a peer-to-peer platform where people can borrow items for free or rent them for a price.

## Deliverables

| Delivery | Tag | What was done |
|----------|-----|---------------|
| D1 | `D1` | Static pages with HTML, CSS and Bootstrap (folder `Deliverable1-Static`) |
| D2 | `D2` | ASP.NET Core MVC, Identity login and sign up, D1 pages made dynamic |
| D3 | `D3` | Dapper, 4 repositories with interfaces and Dependency Injection, Ajax (`$.get`, `$.post`, `$.ajax`), layout section, local IIS deployment |

## Requirements

- Visual Studio 2022
- .NET 8
- SQL Server LocalDB (comes with Visual Studio)

## Run in Visual Studio

1. Open `BorrowBox.sln`
2. Press F5

The database and demo data are created automatically on the first run.

## Demo accounts

| Role | Email | Password |
|------|-------|----------|
| Admin | admin@borrowbox.com | Admin@123 |
| User | asad@borrowbox.com | User@123 |

## Project structure

- `Controllers` - Home, Account, User, Items, Admin
- `Models` and `ViewModels` - data classes
- `Repositories` - 4 repositories with their interfaces (Dapper)
- `Data` - Identity database context and demo data
- `Views` - pages and the shared layout
- `wwwroot` - CSS

## Local deployment on IIS

Address: **http://localhost:8090**

First time setup:

1. Turn on IIS (Windows Features) and install the ASP.NET Core 8 Hosting Bundle
2. Publish the project:
   `dotnet publish BorrowBox\BorrowBox.csproj -c Release -o C:\BorrowBoxSite`
3. Give `IIS_IUSRS` Read and execute permission on the folder `C:\BorrowBoxSite`
4. In IIS Manager add a website: name `BorrowBox`, physical path `C:\BorrowBoxSite`, port `8090`
5. Set the application pool `BorrowBox`: .NET CLR version `No Managed Code`, Identity `LocalSystem`, Load User Profile `True`
6. Open `http://localhost:8090`

The site opens only on the computer where it is hosted (localhost). The IIS site has its own database, separate from the one used when running with F5.

## Hosting again after changing the code

1. IIS Manager > Application Pools > `BorrowBox` > Stop
2. Run the publish command again:
   `dotnet publish BorrowBox\BorrowBox.csproj -c Release -o C:\BorrowBoxSite`
3. Application Pools > `BorrowBox` > Start
4. Refresh the browser with Ctrl + F5

The website, port and pool settings do not need to be created again.

## Technologies

ASP.NET Core MVC (.NET 8), Identity, SQL Server LocalDB, Dapper, jQuery Ajax, Bootstrap, IIS