## AuctionSA

An online auction web app where users can list items for sale, upload photos, and bid against each other. Built with ASP.NET Core MVC as part of my final-year ICT studies.

## Features

- Register and log in (ASP.NET Core Identity)
- Browse all auction listings
- View item details, including photos and the bid history
- Create a listing with a name, description, location, starting price, period of ownership and multiple photos
- Place bids with an optional comment (a bid must be higher than the current highest bid)
- Each listing runs for 48 hours

## Built with

- C# and ASP.NET Core 8 (MVC)
- Entity Framework Core 8 with SQL Server
- ASP.NET Core Identity for authentication
- Razor views and Bootstrap

## How it's structured

- `Controllers/` handles requests (items and bidding)
- `Data/Services/` holds the business logic behind interfaces (`IItemService`, `ItemService`) and the image upload service
- `Models/` and `ViewModels/` hold the database entities and form models
- `Migrations/` holds the database schema

## Running it locally

You need the .NET 8 SDK and SQL Server (or SQL Server LocalDB).

1. Clone the repo
2. In `auctionsa/appsettings.json`, update the `DefaultConnection` string to point to your SQL Server
3. Create the database
4. Start the app

## Still to do

- Edit and delete listings
- End auctions automatically and declare a winner
- Image validation and unit tests

## Author

Sibongakonke Bhekiswayo, Piet Retief, South Africa
GitHub: [Sbonga009](https://github.com/Sbonga009)