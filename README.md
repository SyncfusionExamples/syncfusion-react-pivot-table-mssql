# 📊 Syncfusion React Pivot Table — MSSQL Database Binding Sample 🗄️

React sample showing Syncfusion EJ2 PivotView using DataManager (UrlAdaptor) to fetch rows from an ASP.NET Core API and perform CRUD operations persisted to MSSQL.

## 🧭 Table of Contents
- ⚡️ Quick overview
- ✨ Key features
- 🧩 Server (ASP.NET Core) — run & endpoints
- 🛠️ Client (React) — DataManager usage
- 🚀 Installation & run
- 📁 Project structure
- 🏗️ Architecture & data flow
- ⚙️ Configuration & troubleshooting
- 🤝 Contributing
- 📜 License

## ⚡️ Quick overview
This repository demonstrates a pattern where Syncfusion’s PivotView performs client-side aggregation while the client uses Syncfusion DataManager (UrlAdaptor) to call an ASP.NET Core API that returns raw row data from MSSQL and persists inline edits (Create / Update / Delete). The sample focuses on CRUD integration, DrillThrough editing, and how DataManager maps client operations to server endpoints.

## ✨ Key features
- ✅ Client-side pivoting with Syncfusion EJ2 PivotView
- 🔁 DataManager (UrlAdaptor) centralizes Read/Insert/Update/Remove API calls
- ✍️ Inline add/edit/delete from pivot/grid persisted to MSSQL
- 🛡️ Server sample (ASP.NET Core) with DataManagerRequest + CRUDModel handling
- ♻️ Minimal, clear client/server separation for easy reuse

## 🧩 Server (ASP.NET Core) — run & endpoints
This project uses an ASP.NET Core Web API as the server. Run the server with the .NET SDK:

Prerequisite:
- .NET SDK 6+ (recommended)

Start server:
- Open PowerShell / terminal:
  cd Server
  dotnet run

API endpoints used by the client (controller provided in repo):
- POST  /api/Pivot               — DataManagerRequest (used by UrlAdaptor for queries)
- GET   /api/Pivot               — returns full Orders list (helper)
- POST  /api/Pivot/Insert        — insert new record (CRUDModel<Orders>)
- POST  /api/Pivot/Update        — update existing record (CRUDModel<Orders>)
- POST  /api/Pivot/Remove        — remove record (CRUDModel with key)

Notes:
- The controller expects DataManagerRequest POST payloads (Syncfusion DataManager format) and returns JSON: { result = DataSource, count = total }.
- Insert/Update/Remove endpoints accept CRUDModel<T> as shown in the controller.

Refer to Server/Controllers/PivotController.cs for the exact implementation and models (Orders and CRUDModel<T>).

## 🛠️ Client (React) — DataManager usage
The client uses DataManager + UrlAdaptor. Key configuration in Client/src/App.tsx:

- DataManager setup (example):
  - url: 'https://localhost:7284/api/Pivot' (for queries)
  - insertUrl: 'https://localhost:7284/api/Pivot/Insert'
  - updateUrl: 'https://localhost:7284/api/Pivot/Update'
  - removeUrl: 'https://localhost:7284/api/Pivot/Remove'
  - adaptor: new UrlAdaptor()

- The sample load() calls oData.executeQuery(new Query()) and maps the server response into pivotObj.dataSourceSettings.dataSource.
- DrillThrough uses beginDrillThrough to open an editable Grid and wires actionBegin event to perform oData.insert / oData.update / oData.remove.

Ensure server endpoints match DataManager URLs and that primary key casing (OrderID vs orderID) matches between client and server.

## 🚀 Installation & run
1. Clone:
   git clone https://github.com/SyncfusionExamples/syncfusion-react-pivot-table-mssql-database-binding-sample.git

2. Server:
   cd server
   dotnet restore
   dotnet run

3. Client:
   cd ../client
   npm install
   npm start

4. Open client URL (default: http://localhost:3000) and verify pivot loads and editing persists to DB.

## 📁 Project structure
- Client/ — React app (PivotView + DataManager integration)
- Server/ — ASP.NET Core Web API (PivotController, CRUDModel, Orders model)
- README.md — this file
- LICENSE — license file

## 🏗️ Architecture & data flow
1. Client DataManager sends a DataManagerRequest (POST) to /api/Pivot.
2. Server reads MSSQL (GetOrderData) and returns rows in { result, count } format.
3. PivotView renders client-side aggregation from the returned rows.
4. Editing actions (add/edit/delete) in the DrillThrough/Grid trigger DataManager insert/update/remove calls to server endpoints that persist changes to MSSQL.
5. Client refreshes pivot data after CRUD responses.

## ⚙️ Configuration & troubleshooting
- Server .NET app: update connection string in Server/Controllers/PivotController.cs (or move to appsettings and use IConfiguration).
- Common issues:
  - No data: confirm Server is running, verify /api/Pivot POST/GET responses in browser/Postman.
  - Edits not persisted: check SQL permissions and controller logs; ensure CRUD endpoints are reachable and accept the payload format DataManager sends.
  - CORS: enable CORS in ASP.NET Core Startup/Program to allow client origin.
  - Key name mismatches: ensure the key property used by DataManager.update/remove matches the Orders primary key.

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