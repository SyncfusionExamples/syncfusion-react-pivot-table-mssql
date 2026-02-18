# 📊 Bind SQL Database and perform CRUD using Syncfusion React Pivot Table

> Quick-start sample demonstrating how to expose Orders data from MSSQL via an ASP.NET Core Web API (DataManager-compatible) and interact with it from a React client using Syncfusion PivotView (drill-through CRUD).

---

## 📚 Table of Contents

- [🔍 Project Overview](#-project-overview)  
- [🚀 Quick Start](#-quick-start)  
- [✨ Key Features](#-key-features)  
- [🛠️ Supported Platforms & Dependencies](#️-supported-platforms--dependencies)  
- [🧭 Project Structure](#-project-structure)  
- [🧩 Minimal Example (API + PivotView)](#-minimal-example-api--pivotview)  
- [⚙️ Configuration & Environment](#️-configuration--environment)  
- [🧪 Testing & CI](#-testing--ci)  
- [🤝 Contributing](#-contributing)  
- [📜 License & Support](#-license--support)

---

## 🔍 Project Overview

This repository demonstrates a minimal, production-adjacent pattern to:

- Serve `Orders` records from a SQL Server `dbo.Orders` table.
- Provide full CRUD endpoints consumed by a React client using Syncfusion PivotView + DataManager (UrlAdaptor) with drill-through grid editing.

Intended as a starter/example for teams integrating Syncfusion UI with an ASP.NET Core + MSSQL backend.

---

## 🚀 Quick Start

Prerequisites
- .NET SDK 6.0+  
- SQL Server (or compatible) with database `MyAppDB` and table `dbo.Orders`  
- Node.js (LTS) + npm

Run the API
```bash
cd <api-project-folder>
dotnet restore
dotnet build
dotnet run
# note URL e.g. https://localhost:7284
```

Run the React client
```bash
cd <react-client-folder>
npm install
npm start
# default: http://localhost:3000
```

Verify
- Browse to `https://localhost:<api-port>/api/pivot` — JSON orders should be returned.
- Open the React app and perform drill-through edit/add/delete to confirm CRUD flows.

---

## ✨ Key Features

- DataManager-compatible server processing:
  - Searching, filtering, sorting, paging (POST /api/pivot)
- CRUD HTTP endpoints:
  - `GET /api/pivot` — read all Orders
  - `POST /api/pivot` — DataManagerRequest handling ⇒ `{ result, count }`
  - `POST /api/pivot/Insert` — insert record
  - `POST /api/pivot/Update` — update record
  - `POST /api/pivot/Remove` — delete record
- React sample using Syncfusion PivotView + DataManager (UrlAdaptor) for seamless drill-through CRUD
- Simple DTO mapping from DataTable to Orders model for easy adaptation

Benefits
- Offloads heavy list operations to server for better client performance
- Demonstrates full end‑to‑end CRUD example for PivotView users
- Copy-paste ready patterns for rapid prototyping

---

## 🛠️ Supported Platforms & Dependencies

Server
- Language: C#  
- Framework: ASP.NET Core 6.0+  
- NuGet: Microsoft.Data.SqlClient, Syncfusion.EJ2.Base

Client
- Framework: React  
- NPM: @syncfusion/ej2-react-pivotview, @syncfusion/ej2-data

Note: Pin exact versions in .csproj and package.json for reproducible builds.

---

## 🧭 Project Structure

```
MyWebService/
├── Controllers/
│   └── PivotController.cs      # DataManager handler + CRUD methods
├── Client/
│   └── src/App.js              # React PivotView + DataManager sample
├── Program.cs                  # App bootstrap (CORS, middleware)
├── appsettings.json            # Recommended: connection strings
└── README.md
```

---

## 🧩 Minimal Example (API + PivotView)

Server snippet (DataManager response)
```csharp
// POST /api/pivot
// returns: { result: [...], count: totalCount }
```

React client core (drill-through CRUD)
```javascript
import { PivotViewComponent } from '@syncfusion/ej2-react-pivotview';
import { DataManager, Query, UrlAdaptor } from '@syncfusion/ej2-data';

let oData = new DataManager({
  url: 'https://localhost:7284/api/Pivot',
  insertUrl: 'https://localhost:7284/api/Pivot/Insert',
  updateUrl: 'https://localhost:7284/api/Pivot/Update',
  removeUrl: 'https://localhost:7284/api/Pivot/Remove',
  adaptor: new UrlAdaptor()
});

oData.executeQuery(new Query()).then(e => {
  pivotObj.dataSourceSettings.dataSource = e.result.result;
});
```

Grid action handlers (sample)
```javascript
function gridActionBegin(args) {
  if (args.action == 'add' && args.requestType == 'save') oData.insert(args.data);
  else if (args.action == 'edit' && args.requestType == 'save') oData.update('OrderID', args.data);
  else if (args.requestType == 'delete') oData.remove('orderID', args.data[0].orderID);
}
```

---

## 🧪 Testing & CI

Recommendations
- Add unit tests (xUnit or NUnit) for DataManager logic and CRUD endpoints.
- Add GitHub Actions to build API, run tests, and build client. Example workflow:
```yaml
# .github/workflows/ci.yml
- dotnet restore && dotnet build
- dotnet test
- npm ci --prefix ./Client && npm run build --prefix ./Client
```

---

## 🤝 Contributing

- Fork → branch `feature/<desc>` → PR
- Add tests for new logic
- Follow conventional commits
- Provide reproducible steps in issues

Suggested files to add: CONTRIBUTING.md, ISSUE templates, PULL_REQUEST_TEMPLATE.md, CODE_OF_CONDUCT.md

---

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
