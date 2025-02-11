Stock Market Screener & Price Alerts
A .NET Core 8 Worker Service that fetches real-time stock prices from Polygon.io, stores them in an SQL Server database, and runs asynchronously to monitor multiple stock prices efficiently.

🚀 Features

Fetch real-time stock prices from Polygon.io
Store stock prices in a SQL Server database
Supports multiple stocks simultaneously using parallel API calls
Runs as a background worker service in .NET Core 8
Efficient database operations using Entity Framework Core
Highly scalable for tracking more assets

📌 Prerequisites

Before running the project, make sure you have:

.NET Core 8 SDK installed
SQL Server installed and running
A Polygon.io API Key (Follow steps below to create one)

🔑 How to Create a Polygon.io API Key

Go to Polygon.io
Click Sign Up and create a free account
Once logged in, navigate to API Keys in your dashboard
Copy the API Key for later use in appsettings.json

🛠️ How to Update the Database Connection String

Open appsettings.json in StockScreenerWorker project
Locate the ConnectionStrings section
Replace YOUR_SERVER with your SQL Server name and StockScreenerDb with your database name

{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=StockScreenerDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}

Save the file

🏗️ How to Setup and Run the Project

1️⃣ Clone the Repository

git clone https://github.com/HendrikPhotography/StockScreener.git
cd StockScreener

2️⃣ Install Dependencies

Navigate to the Worker Service project and install dependencies:

cd StockScreenerWorker
dotnet restore

3️⃣ Apply Database Migrations

Run the following commands to create the database schema:

dotnet ef migrations add InitialCreate --project ../StockScreener.Data
dotnet ef database update --project ../StockScreener.Data

4️⃣ Run the Worker Service

dotnet run

🔄 How to Modify Tracked Stocks

To add or remove stocks from tracking, modify the list of stocks in Worker.cs:

List<string> stockSymbols = new() { "AAPL", "GOOGL", "MSFT", "TSLA", "AMZN" };

Replace or expand this list to track more stocks.

🛠️ Troubleshooting

❌ API Rate Limits

Polygon.io enforces API rate limits. If you get rate limit errors, consider upgrading to a paid plan or reduce request frequency.

❌ Database Connection Issues

Make sure SQL Server is running and accessible

Check if the connection string in appsettings.json is correctly set

❌ No Stock Data Found

Ensure the Polygon.io API key is correct and has access to stock data

Check logs in the Output Window (Ctrl + Alt + O) for errors

📜 License

This project is licensed under the MIT License.

Happy Coding!
