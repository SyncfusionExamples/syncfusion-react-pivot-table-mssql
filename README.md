# 📊 Connecting Microsoft SQL Server Data to Syncfusion Pivot Table with CRUD Support

This sample demonstrates how to connect and retrieve data from a **Microsoft SQL Server database** and bind it to the **Syncfusion EJ2 React Pivot Table** component with full **CRUD (Create, Read, Update, and Delete)** operations support.

The Syncfusion Pivot Table component is bound to a Microsoft SQL Server database using [DataManager](https://ej2.syncfusion.com/react/documentation/data/getting-started) with [UrlAdaptor](https://ej2.syncfusion.com/react/documentation/data/adaptors/#url-adaptor). The **UrlAdaptor** serves as the base adaptor for facilitating communication between remote data services (ASP.NET Core Web API) and the Pivot Table component. This approach enables seamless data retrieval, client-side aggregation, and real-time CRUD operations that persist back to the database.

---

## 🧭 Table of Contents
- 📖 Overview
- ✨ Key features  
- 🛠️ Prerequisites
- 🚀 Getting started
- 🧩 Server implementation
- 📱 Client implementation
- 📁 Project structure
- 🏗️ How it works
- ⚙️ Configuration
- 🐛 Troubleshooting
- 📜 License & support

---

## 📖 Overview

This sample showcases the integration between:

**Frontend (Client):**
- **React 19.2.0** with **TypeScript 5.9.3**
- **Syncfusion EJ2 React PivotView (v32.2.4)** for data visualization
- **DataManager with UrlAdaptor** for remote data binding and CRUD operations
- **Vite 7.3.1** for fast development and optimized builds

**Backend (Server):**
- **ASP.NET Core 8.0 Web API** with RESTful endpoints
- **Microsoft.Data.SqlClient** for SQL Server connectivity
- **Syncfusion.EJ2.AspNet.Core (v32.2.3)** for DataManagerRequest handling
- **Direct database operations** using ADO.NET

**Database:**
- **Microsoft SQL Server 2019+** or **Azure SQL Database**
- Orders table with fields: OrderID, CustomerID, EmployeeID, ShipCity, Freight

---

## ✨ Key features

- 📊 **Remote Data Binding:** Connect Pivot Table to SQL Server via DataManager with UrlAdaptor
- 🔄 **Full CRUD Support:** Create, Read, Update, and Delete operations from the client
- 🎯 **Client-side Aggregation:** PivotView performs data pivoting and summarization in the browser
- ✏️ **Inline Editing:** Edit records directly through drill-through grid interface
- 🔁 **Automatic Synchronization:** DataManager handles all client-server communication
- 🛡️ **Type Safety:** Full TypeScript support with strongly-typed models
- ⚡ **Modern Development:** Hot module replacement, fast builds, and optimized production bundles
- 🔐 **CORS-enabled API:** Supports cross-origin requests with Swagger documentation

---

## 🛠️ Prerequisites

Before running this sample, ensure you have the following installed:

**Required:**
- **.NET SDK 8.0 or later** - [Download here](https://dotnet.microsoft.com/download)
- **Node.js 18.0 or later** with npm - [Download here](https://nodejs.org)
- **SQL Server 2019+** or **Azure SQL Database**
- **Modern web browser** (Chrome, Edge, Firefox, or Safari)

**Recommended:**
- **Visual Studio 2022** or **Visual Studio Code** with C# extension
- **SQL Server Management Studio** or **Azure Data Studio** for database management

---

## 🚀 Getting started

Follow these steps to run the sample on your local machine:

### Step 1: Clone the Repository

```bash
git clone <repository-url>
cd syncfusion-react-pivot-table-mssql-database-binding-sample
```

### Step 2: Setup the Database

Create the database and `Orders` table in SQL Server:

```sql
CREATE DATABASE MyAppDB;
GO

USE MyAppDB;
GO

CREATE TABLE Orders (
    OrderID INT PRIMARY KEY IDENTITY(1,1),
    CustomerID NVARCHAR(50) NOT NULL,
    EmployeeID INT,
    ShipCity NVARCHAR(100),
    Freight DECIMAL(10, 2)
);
GO

-- Insert sample data
INSERT INTO Orders (CustomerID, EmployeeID, ShipCity, Freight)
VALUES 
    ('ALFKI', 1, 'Berlin', 32.38),
    ('ANATR', 2, 'México D.F.', 11.61),
    ('ANTON', 3, 'México D.F.', 65.83),
    ('AROUT', 4, 'London', 41.34),
    ('BERGS', 5, 'Luleå', 51.30);
GO
```

### Step 3: Configure the Server

Navigate to `server/Controllers/PivotController.cs` and update the connection string:

```csharp
string ConnectionString = @"Server=YOUR_SERVER_NAME;Database=MyAppDB;Trusted_Connection=True;TrustServerCertificate=True;";
```

**Connection String Examples:**

```csharp
// Windows Authentication (Local SQL Server)
string ConnectionString = @"Server=localhost;Database=MyAppDB;Trusted_Connection=True;TrustServerCertificate=True;";

// SQL Server Authentication
string ConnectionString = @"Server=YOUR_SERVER;Database=MyAppDB;User Id=YOUR_USERNAME;Password=YOUR_PASSWORD;TrustServerCertificate=True;";

// Azure SQL Database
string ConnectionString = @"Server=YOUR_SERVER.database.windows.net;Database=MyAppDB;User Id=YOUR_USERNAME;Password=YOUR_PASSWORD;Encrypt=True;";
```

### Step 4: Run the Server

Open a terminal in the `server` folder:

```bash
cd server
dotnet restore
dotnet build
dotnet run
```

**Expected Output:**
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:7284
```

✅ **Verify:** Open `https://localhost:7284/swagger` in your browser to access the API documentation.

### Step 5: Run the Client

Open a **new terminal** in the `client` folder:

```bash
cd client
npm install
npm run dev
```

**Expected Output:**
```
VITE v7.3.1  ready in 234 ms
➜  Local:   http://localhost:5173/
```

✅ **Verify:** Open `http://localhost:5173` in your browser to see the Pivot Table with data from SQL Server.

### Step 6: Test CRUD Operations

1. **View Data:** The pivot table displays orders aggregated by Ship City (rows) and Customer ID (columns)
2. **Drill Through:** Click any cell value to see detailed records in a grid
3. **Add Record:** Click "Add" in the drill-through grid, fill in the fields, and save
4. **Edit Record:** Select a row, click "Edit", modify values, and save
5. **Delete Record:** Select a row and click "Delete" to remove it

All changes are immediately persisted to the SQL Server database.

---

## 🧩 Server implementation

### Technology Stack
- ASP.NET Core 8.0 Web API
- Microsoft.Data.SqlClient 6.1.4 (ADO.NET)
- Syncfusion.EJ2.AspNet.Core 32.2.3
- Swashbuckle.AspNetCore 10.1.2 (Swagger)

### API Endpoints

The `PivotController` provides the following endpoints:

| Method | Endpoint | Purpose | Used By |
|--------|----------|---------|---------|
| POST | `/api/Pivot` | Handles DataManagerRequest for querying data | UrlAdaptor (Read) |
| GET | `/api/Pivot` | Returns all Orders (helper endpoint) | Testing |
| POST | `/api/Pivot/Insert` | Inserts a new Order record | DataManager (Create) |
| POST | `/api/Pivot/Update` | Updates an existing Order record | DataManager (Update) |
| POST | `/api/Pivot/Remove` | Deletes an Order record by key | DataManager (Delete) |

### Data Models

**Orders Model:**
```csharp
public class Orders
{
    [Key]
    public int? OrderID { get; set; }
    public string? CustomerID { get; set; }
    public int? EmployeeID { get; set; }
    public decimal? Freight { get; set; }
    public string? ShipCity { get; set; }
}
```

**CRUDModel (Generic):**
```csharp
public class CRUDModel<T> where T : class
{
    public string? action { get; set; }        // "insert", "update", or "remove"
    public string? keyColumn { get; set; }
    public object? key { get; set; }           // Primary key value
    public T? value { get; set; }              // The record data
    public List<T>? added { get; set; }
    public List<T>? changed { get; set; }
    public List<T>? deleted { get; set; }
    public IDictionary<string, object>? @params { get; set; }
}
```

### Key Implementation Details

**1. DataManagerRequest Handling (POST /api/Pivot):**
```csharp
[HttpPost]
[Route("api/[controller]")]
public object Post([FromBody] DataManagerRequest DataManagerRequest)
{
    IQueryable<Orders> DataSource = GetOrderData().AsQueryable();
    int totalRecordsCount = DataSource.Count();
    return new { result = DataSource, count = totalRecordsCount };
}
```

**2. Database Operations:**
- Uses `SqlConnection` and `SqlCommand` for direct database access
- Executes parameterized SQL queries for INSERT, UPDATE, and DELETE
- Returns data as `List<Orders>` converted to JSON

**3. CORS Configuration (Program.cs):**
```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});
```

---

## � Client implementation

### Technology Stack
- React 19.2.0
- TypeScript 5.9.3
- Vite 7.3.1
- Syncfusion EJ2 React PivotView 32.2.4
- ESLint 9.39.1

### DataManager Configuration

The **DataManager** with **UrlAdaptor** is configured in `client/src/App.tsx` to handle all data operations:

```typescript
import { DataManager, UrlAdaptor } from '@syncfusion/ej2-data';
import { PivotViewComponent } from '@syncfusion/ej2-react-pivotview';

let oData: DataManager = new DataManager({
  url: 'https://localhost:7284/api/Pivot',           // Read operation
  insertUrl: 'https://localhost:7284/api/Pivot/Insert', // Create operation
  updateUrl: 'https://localhost:7284/api/Pivot/Update', // Update operation
  removeUrl: 'https://localhost:7284/api/Pivot/Remove', // Delete operation
  adaptor: new UrlAdaptor()
});

const dataSourceSettings = {
  dataSource: oData,
  expandAll: true,
  rows: [{ name: 'shipCity', caption: 'Ship City' }],
  columns: [{ name: 'customerID', caption: 'Customer ID' }],
  values: [{ name: 'freight', caption: 'Freight' }],
  filters: [],
  fieldMapping: [
    { name: 'employeeID', caption: 'Employee ID' },
    { name: 'orderID', caption: 'Order ID' }
  ]
};

const editSettings = {
  allowEditing: true,
  allowAdding: true,
  allowDeleting: true,
  mode: 'Normal'
};
```

### PivotView Configuration

The PivotView component is configured with:

**Data Source Settings:**
```typescript
const dataSourceSettings = {
  dataSource: oData,                    // DataManager instance
  expandAll: true,                      // Expand all rows by default
  rows: [{ name: 'shipCity', caption: 'Ship City' }],
  columns: [{ name: 'customerID', caption: 'Customer ID' }],
  values: [{ name: 'freight', caption: 'Freight' }],
  filters: [],
  fieldMapping: [
    { name: 'employeeID', caption: 'Employee ID' },
    { name: 'orderID', caption: 'Order ID' }
  ]
};
```

**Edit Settings:**
```typescript
const editSettings = {
  allowEditing: true,     // Enable inline editing
  allowAdding: true,      // Enable adding new records
  allowDeleting: true,    // Enable deleting records
  mode: 'Normal'          // Normal editing mode
};
```

### Drill-Through Event Handler

The `beginDrillThrough` event is used to configure the drill-through grid for CRUD operations:

```typescript
function beginDrillThrough(args: any) {
  for (var i = 0; i < args.gridObj.columns.length; i++) {
    if (args.gridObj.columns[i].field == "orderID") {
      // Set OrderID as the primary key for CRUD operations
      args.gridObj.columns[i].isPrimaryKey = true;
    } else {
      // Make all fields visible in the editing grid
      args.gridObj.columns[i].visible = true;
    }
  }
}
```

**Purpose:**
- **Primary Key Assignment:** Marks `orderID` as the primary key to ensure correct record targeting for updates and deletes
- **Field Visibility:** Makes all fields visible in the drill-through grid (not just pivot-bound fields)
- **CRUD Support:** Enables inline add, edit, and delete operations directly from the grid

### Complete Component

```typescript
return (
  <PivotViewComponent
    id='PivotView'
    ref={(scope: any) => { pivotObj = scope; }}
    height={350}
    dataSourceSettings={dataSourceSettings}
    editSettings={editSettings}
    beginDrillThrough={beginDrillThrough}
  />
);
```

### How DataManager Works

When a CRUD operation is performed:

1. **Create:** DataManager sends POST request to `insertUrl` with the new record
2. **Read:** DataManager sends POST request to `url` with DataManagerRequest payload
3. **Update:** DataManager sends POST request to `updateUrl` with modified record and key
4. **Delete:** DataManager sends POST request to `removeUrl` with the record key

The **UrlAdaptor** automatically formats requests and handles responses according to Syncfusion's data protocol.

**Important:** Ensure field name casing matches between client (`orderID`) and server (`OrderID` in database, but serialized as `orderID` in JSON).

---

##  Project structure

```
syncfusion-react-pivot-table-mssql-database-binding-sample/
│
├── 📁 client/                          # React + TypeScript Frontend
│   ├── 📁 src/
│   │   ├── App.tsx                    # Main PivotView component with DataManager
│   │   ├── App.css                    # Component styles
│   │   ├── main.tsx                   # Application entry point
│   │   ├── index.css                  # Global styles
│   │   └── 📁 assets/                 # Static assets
│   ├── 📁 public/                     # Public static files
│   ├── package.json                   # Dependencies (React 19, Vite 7.3, EJ2)
│   ├── vite.config.ts                # Vite configuration
│   ├── tsconfig.json                 # TypeScript config
│   ├── eslint.config.js              # ESLint config
│   └── index.html                     # HTML entry point
│
├── 📁 server/                          # ASP.NET Core 8.0 Backend
│   ├── 📁 Controllers/
│   │   └── PivotController.cs         # CRUD endpoints + DataManager support
│   ├── 📁 Properties/
│   │   └── launchSettings.json        # Server launch settings (port 7284)
│   ├── Program.cs                     # App startup (CORS, Swagger, etc.)
│   ├── MyWebService.csproj            # Project file with NuGet packages
│   ├── appsettings.json               # Configuration
│   └── appsettings.Development.json   # Dev-specific config
│
└── README.md                           # This file
```

### Key Files
- **`client/src/App.tsx`** — PivotView component with DataManager configuration, edit settings, and drill-through event handler
- **`server/Controllers/PivotController.cs`** — All API endpoints, Orders model, CRUDModel<T>, and database operations
- **`server/Program.cs`** — CORS policy, Swagger configuration, controller registration

---

## 🏗️ How it works

### Architecture Overview
```
┌─────────────────────┐
│   React Client      │
│  (Port 5173)        │
│                     │
│  - PivotViewComp    │
│  - DataManager      │
│  - UrlAdaptor       │
└──────────┬──────────┘
           │ HTTPS (JSON)
           │ DataManagerRequest
           ▼
┌─────────────────────┐
│  ASP.NET Core API   │
│  (Port 7284)        │
│                     │
│  - PivotController  │
│  - CRUD Endpoints   │
│  - SqlConnection    │
└──────────┬──────────┘
           │ ADO.NET
           │ SqlClient
           ▼
┌─────────────────────┐
│   MSSQL Database    │
│                     │
│  Database: MyAppDB  │
│  Table: Orders      │
└─────────────────────┘
```

### Data Binding Process

**How UrlAdaptor Connects Pivot Table to SQL Server:**

1. **DataManager with UrlAdaptor** is configured with the API endpoint URLs
2. **PivotView component** uses DataManager as its data source
3. **UrlAdaptor** automatically sends HTTP requests to the server endpoints
4. **Server API** queries SQL Server and returns data in the expected format
5. **PivotView** receives the data and performs client-side aggregation

### Request Flow Details

**1. Initial Data Load (READ Operation)**
1. Client: PivotView component mounts
2. DataManager sends: **POST** `/api/Pivot` (DataManagerRequest)
3. Server: `PivotController.Post()` receives request
4. Server executes: `SELECT * FROM Orders` via `GetOrderData()`
5. Server returns: `{ result: [...orders], count: 100 }`
6. Client: PivotView aggregates data by Ship City & Customer ID
7. Pivot table renders on screen

**2. Update Record (UPDATE Operation)**
1. User clicks cell → Drill-through grid opens
2. User edits values → Clicks Save
3. `beginDrillThrough` marks `orderID` as primary key
4. DataManager sends: **POST** `/api/Pivot/Update` (CRUDModel)
5. Server: `PivotController.Update()` executes `UPDATE Orders SET ...`
6. Server returns success
7. Client refreshes → Shows updated aggregation

**3. Add New Record (CREATE Operation)**
1. User clicks "Add New" in drill-through grid
2. User fills fields → Clicks Save
3. DataManager sends: **POST** `/api/Pivot/Insert` (CRUDModel)
4. Server: `PivotController.Insert()` executes `INSERT INTO Orders ...`
5. Server returns success
6. Client refreshes → New record included in aggregation

**4. Delete Record (DELETE Operation)**
1. User selects record → Clicks Delete
2. DataManager sends: **POST** `/api/Pivot/Remove` (CRUDModel with key)
3. Server: `PivotController.Remove()` executes `DELETE FROM Orders WHERE OrderID = ...`
4. Server returns success
5. Client refreshes → Record removed from aggregation

### Key Advantages of This Approach

✅ **Separation of Concerns:** Client handles UI and aggregation; server handles data access  
✅ **Performance:** Client-side pivoting reduces server load  
✅ **Flexibility:** Easy to add custom logic on either client or server  
✅ **Type Safety:** TypeScript on client, C# on server with strongly-typed models  
✅ **Scalability:** Stateless API design supports horizontal scaling  

---

## ⚙️ Configuration

### Server Configuration

**Update Connection String:**  
Edit `server/Controllers/PivotController.cs`:

```csharp
// Windows Authentication
string ConnectionString = @"Server=localhost;Database=MyAppDB;Trusted_Connection=True;TrustServerCertificate=True;";

// SQL Server Authentication
string ConnectionString = @"Server=YOUR_SERVER;Database=MyAppDB;User Id=YOUR_USER;Password=YOUR_PASSWORD;TrustServerCertificate=True;";

// Azure SQL Database
string ConnectionString = @"Server=YOUR_SERVER.database.windows.net;Database=MyAppDB;User Id=YOUR_USER;Password=YOUR_PASSWORD;Encrypt=True;";
```

**Update CORS Policy (for production):**  
Edit `server/Program.cs`:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy
            .WithOrigins("https://yourdomain.com")  // Specify allowed origins
            .AllowAnyMethod()
            .AllowAnyHeader());
});
```

### Client Configuration

**Update API URLs:**  
Edit `client/src/App.tsx` if your server runs on a different port or domain:

```typescript
let oData: DataManager = new DataManager({
  url: 'YOUR_API_URL/api/Pivot',
  insertUrl: 'YOUR_API_URL/api/Pivot/Insert',
  updateUrl: 'YOUR_API_URL/api/Pivot/Update',
  removeUrl: 'YOUR_API_URL/api/Pivot/Remove',
  adaptor: new UrlAdaptor()
});
```

---

## 🐛 Troubleshooting

### Common Issues and Solutions

**❌ Problem: No data displayed in pivot table**
- Confirm server is running at `https://localhost:7284`
- Verify `/api/Pivot` POST/GET responses in browser DevTools (F12 → Network tab)
- Test endpoint directly: Navigate to `https://localhost:7284/api/Pivot`
- Check database connection string is correct

**❌ Problem: CORS error - Access denied**
- Enable CORS in `server/Program.cs` (see configuration above)
- Ensure `app.UseCors("AllowAll")` is called before `app.MapControllers()`
- Restart backend server after changes
- Clear browser cache (Ctrl+Shift+Delete)

**❌ Problem: Edits not persisting to database**
- Check SQL Server is running
- Verify database `MyAppDB` and table `Orders` exist
- Test connection using SQL Server Management Studio or Azure Data Studio
- Check database user has INSERT/UPDATE/DELETE permissions
- Review server logs for SQL execution errors
- Ensure `orderID` is marked as primary key in `beginDrillThrough` event

**❌ Problem: Primary key mismatch errors**
- Ensure field names match exactly (case-sensitive):
  - Client: `orderID` (camelCase) in DataManager
  - Server: Must return `orderID` in JSON response
  - Database: Column name case doesn't matter (ADO.NET handles mapping)

**❌ Problem: Build fails - 'Syncfusion' not found**
- Backend: Run `dotnet clean`, `dotnet restore`, `dotnet build`
- Frontend: Run `npm install` or `npm install --legacy-peer-deps`

**❌ Problem: Port already in use**
- Change port in `server/Properties/launchSettings.json`
- Update client DataManager URLs to match new port

---

## 📜 License & support

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
