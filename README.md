# AllCompaniesAPI & DbExtensions

## 📌 Purpose of the Projects

**AllCompaniesAPI**  
An ASP.NET Core API designed to manage BIST (Borsa İstanbul) company data.  
- Provides **Swagger** documentation for easy API exploration.  
- Uses **Hangfire** to run recurring data-fetch jobs in the background.

**DbExtensions**  
A helper library for **Entity Framework Core** that adds convenient methods such as `MultipleAddOrUpdate<T>` to simplify bulk insert and update operations.

---

## ⚙️ Execution Requirements
- **.NET 8 SDK**
- **MySQL** database (used for EF Core context and Hangfire storage)
- **Redis** server (used for caching)

---

## 🛠 Service Registrations (Program.cs) & CompaniesJob Workflow

- **Swagger**: Configured with a custom title, description, and XML comments.  
- **MySQL**: EF Core context is registered using a MySQL connection string.  
- **Hangfire**: Configured with MySQL-backed storage, server, and dashboard. A recurring job named `fetch data` runs every 4 hours.  
- **Redis**: A singleton `IConnectionMultiplexer` is used to connect to Redis.  
- **CompaniesJob Workflow**:
  1. Fetches the company list from the **KAP** (Public Disclosure Platform) site.
  2. Inserts/updates the data in the database.
  3. Stores each record in Redis as JSON.

---

## 📄 Using the `AllCompaniesAPI.http` File
- Open `AllCompaniesAPI.http` in a REST client (e.g., **VS Code REST Client** or **Visual Studio HTTP Editor**).  
- The `@AllCompaniesAPI_HostAddress` variable defines the base API address.  
- You can execute the provided GET request directly.

---

## 🚀 Steps to Run Locally

1. Install and start:
   - **.NET 8 SDK**
   - **MySQL** (e.g., `localhost:3306`)
   - **Redis** (e.g., `localhost:6379`)
2. Download the source code.
3. Restore dependencies:
  --bash--
dotnet restore

4.Build the Project
--bash--
dotnet build

5.Apply EF Core migrations:
--bash--
dotnet ef database update

6.Run the API:
--bash--
dotnet run --project AllCompaniesAPI

7.Access the interfaces:
Swagger UI → http://localhost:5095/swagger
Hangfire Dashboard → http://localhost:5095/hangfire


