# 📊 MyWebService - Orders API with MSSQL

> ASP.NET Core Web API for managing orders data from a MSSQL database with support for CRUD operations.

---

## 📚 Table of Contents

- [🔍 Project Overview](#-project-overview)
- [🚀 Quick Start](#-quick-start)
- [✨ Key Features](#-key-features)
- [🛠️ Supported Platforms & Dependencies](#️-supported-platforms--dependencies)
- [🧭 Project Structure](#-project-structure)
- [📋 API Endpoints](#-api-endpoints)
- [⚙️ Configuration & Environment](#️-configuration--environment)
- [🧪 Testing & CI](#-testing--ci)
- [🤝 Contributing](#-contributing)
- [📜 License & Support](#-license--support)

---

## 🔍 Project Overview

MyWebService is an ASP.NET Core Web API that exposes orders data from a MSSQL database. The `PivotController` provides RESTful endpoints for:

- Fetching order records
- Creating new orders (INSERT)
- Updating existing orders (UPDATE)
- Deleting orders (DELETE)

Built for server-side data operations and integration with Syncfusion DataManager requests.

Use cases:
- BI reporting dashboards
- Sales/financial data analysis
- Server-side data management for pivot tables and grids
- Prototyping data connectors for reporting applications

---

## 🚀 Quick Start

### Prerequisites

- .NET SDK (6.0+ recommended)
- SQL Server (or compatible) instance with `MyAppDB` database and `Orders` table
- Visual Studio 2022, Visual Studio Code, or command line

### Run the ASP.NET Core API

```bash
dotnet restore
dotnet build
dotnet run
```
First success
Using an API client (like cURL, Postman, or simply your web browser), navigate to the API's read endpoint: https://localhost:<port>/api/pivot. Confirm that JSON data representing your orders is returned.

## ✨ Key Features
SQL Server Data Access: Directly interacts with a SQL Server Orders table.
Full CRUD Operations:
- Create: Insert new order records.
- Read: Retrieve all orders, or retrieve processed data based on DataManager requests.
- Update: Modify existing order records.
- Delete: Remove order records by ID.

## 🛠️ Supported Platforms & Dependencies

Primary language: C#
Framework: ASP.NET Core (6.0+ recommended)
Database: Microsoft SQL Server
Key Dependencies (as reflected in the controller code):
- Microsoft.AspNetCore.Mvc: Provides the core functionalities for building Web APIs.
- Microsoft.Data.SqlClient: Used for connecting to and interacting with SQL Server databases.
- Syncfusion.EJ2.Base: Contains the DataManagerRequest and QueryableOperation classes for handling Syncfusion data operations.

System requirements:
- .NET SDK commensurate with the ASP.NET Core version used.
- A running instance of SQL Server or a compatible database engine.

## 🧭 Project Structure

API controller: Controllers/PivotController.cs (contains all business logic for the API).
Application startup: Program.cs (where the web host is configured and started).

## 🧪 Testing & CI
It is highly recommended to implement unit tests for the controller's logic and integrate them into a Continuous Integration (CI) pipeline.
Suggested CI workflow steps would include:
- dotnet restore: To restore project dependencies.
- dotnet build: To compile the project.
- dotnet test: To run any implemented unit tests.

## 🤝 Contributing
Contributions are welcome! Please follow these guidelines:

- Fork the repository and create a new branch for your feature or bug fix: feature/<short-desc>.
- Implement your changes, ensuring they align with the project goals and coding style.
- Thoroughly test your changes using the local API endpoints.
- Update this README.md file if your changes affect the documentation.
- Open a Pull Request, clearly describing your changes and referencing any related issues.

## 📜 License & Support

This is a **commercial product** subject to the Syncfusion End User License Agreement (EULA).

**Free Community License** is available for qualifying users/organizations:  ś
- Annual gross revenue < $1 million USD  
- 5 or fewer total developers  
- 10 or fewer total employees  

The community license allows free use in both internal and commercial applications under these conditions.  
No registration or approval is required — just comply with the terms.

**Paid Licenses** are required for:  
- Larger organizations  
- Teams exceeding the community license limits  
- Priority support, custom patches, or on-premise deployment options  

Purchase options and pricing: https://www.syncfusion.com/sales/products  
30-day free trial (full features, no credit card required): https://www.syncfusion.com/downloads/essential-js2  
Community License details & FAQ: https://www.syncfusion.com/products/communitylicense  
Full EULA: https://www.syncfusion.com/eula/es/

© 2026 Syncfusion, Inc. All Rights Reserved.