# Syncfusion React Pivot Table – MSSQL Database Binding Sample

End-to-end sample that demonstrates how to bind a **Microsoft SQL Server (MSSQL)** database to the Syncfusion React Pivot Table and perform **CRUD** (Create, Read, Update, Delete) operations through an ASP.NET Core Web API backend using the Syncfusion `UrlAdaptor`.

## Overview

This project shows how to:

- Connect a Syncfusion React PivotTable to an MSSQL database.
- Expose data through an ASP.NET Core Web API that uses `Microsoft.Data.SqlClient`.
- Load, aggregate, and pivot sales data in the browser.
- Insert, update, and delete records via the API and reflect the changes immediately in the PivotTable.
- Wire the Syncfusion `DataManager` `UrlAdaptor` to the Web API for seamless server-driven CRUD.

## Prerequisites

- [Node.js](https://nodejs.org/) 18+ and npm
- [.NET SDK](https://dotnet.microsoft.com/download) 8.0 or later
- [SQL Server](https://www.microsoft.com/en-us/sql-server) (LocalDB, Express, or full edition) with a `salesdb` database
- A modern web browser (Edge, Chrome, Firefox, or Safari)

## Project Structure

```
syncfusion-react-pivot-table-mssql-database-binding-sample/
├── README.md
├── PivotTable_MSSQL.Server/      # ASP.NET Core Web API (MSSQL + CRUD via UrlAdaptor)
│   ├── Program.cs
│   ├── controllers/
│   │   └── SalesController.cs
│   ├── appsettings.json
│   └── PivotTable_MSSQL.Server.csproj
└── pivottable_mssql.client/      # React + Vite + Syncfusion PivotTable
    ├── index.html
    ├── package.json
    ├── vite.config.ts
    └── src/
        ├── App.tsx
        ├── main.tsx
        └── App.css
```

## Backend – ASP.NET Core Web API (MSSQL)

The backend uses `Microsoft.Data.SqlClient` to read and write the `dbo.salesdata` table. CRUD is exposed through a single `SalesController` whose routes match the Syncfusion `UrlAdaptor` contract.

### Key Packages

- `Microsoft.Data.SqlClient`
- `Microsoft.AspNetCore.OpenApi`
- `Syncfusion.EJ2.AspNet.Core`

### Configuration

Add a `SalesDb` connection string to `appsettings.json` (or via user secrets / environment variables):

```json
{
  "ConnectionStrings": {
    "SalesDb": "Server=.;Database=salesdb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

> Replace with your actual SQL Server instance, credentials, and database name.

### `SalesController` Endpoints

| Method | Route               | Description                                                                 |
| ------ | ------------------- | --------------------------------------------------------------------------- |
| POST   | `/api/sales`        | Fetch sales data (DataManager `UrlAdaptor` load request, returns count).    |
| GET    | `/api/sales`        | Retrieve all sales records (helper for direct inspection).                 |
| POST   | `/api/sales/Insert` | Insert a new sales record.                                                  |
| POST   | `/api/sales/Update` | Update an existing sales record.                                            |
| POST   | `/api/sales/Remove` | Delete a sales record (identified by the `Key` field).                      |

The `Insert`, `Update`, and `Remove` endpoints accept a `CRUDModel<T>` payload with `Action`, `KeyColumn`, `Key`, `Value`, and `Params` properties — the standard contract used by the Syncfusion `UrlAdaptor`.

### CORS

CORS is configured in `Program.cs` to allow the React client running on `https://localhost:7086`:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactClient",
        policy => policy
            .WithOrigins("https://localhost:7086")
            .AllowAnyMethod()
            .AllowAnyHeader());
});
```

Adjust the origin to match the URL where the client runs.

### Run the API

```bash
cd PivotTable_MSSQL.Server
dotnet restore
dotnet run
```

By default the API listens on the URL configured in `Properties/launchSettings.json` (for example `https://localhost:7001` or `http://localhost:5000`).

## Frontend – React + Syncfusion PivotTable

The client is built with **Vite**, **React**, and **TypeScript** and uses the Syncfusion React PivotTable component together with the `DataManager` `UrlAdaptor`.

### Key Packages

- `@syncfusion/ej2-react-pivotview`
- `react` / `react-dom`

The `DataManager` is configured with `url: '/api/sales'` and `adaptor: new UrlAdaptor()`, which automatically routes read, insert, update, and remove operations to the matching controller endpoints.

### Run the Client

```bash
cd pivottable_mssql.client
npm install
npm run dev
```

Open the URL printed by Vite (typically `http://localhost:5173` or `https://localhost:7086`) in your browser.

## Configuring the API URL in the Client

This sample does **not** use a Vite dev-server proxy. The client talks to the API directly from the browser, so:

1. The API must be running and reachable from the client machine.
2. The API's CORS policy must allow the client's origin (default `https://localhost:7086`, see `Program.cs`).
3. The `url` passed to the `DataManager` in `App.tsx` (for example `'/api/sales'`) must point at the API. If the client and API are hosted on different origins, set it to the full API URL, e.g. `'https://localhost:7001/api/sales'`.

## How It Works

1. The API opens a connection to the `salesdb` database using the `SalesDb` connection string.
2. The React app initializes a Syncfusion `DataManager` with the `UrlAdaptor` pointing at `/api/sales`.
3. The PivotTable issues a `POST /api/sales` to load data; the API returns rows plus the total count.
4. When the user adds, edits, or deletes a record in the PivotTable, the `UrlAdaptor` issues the matching `Insert`, `Update`, or `Remove` request.
5. The API executes the parameterized SQL command against MSSQL and returns the standard `{ key, value, action }` response.
6. The PivotTable refreshes to reflect the latest data.

## CRUD with Syncfusion PivotTable

The PivotTable is configured to use the `editSettings` along with the `DataManager` `UrlAdaptor`, which routes CRUD to the controller automatically:

```ts
editSettings: {
  allowAdding: true,
  allowEditing: true,
  allowDeleting: true,
  mode: 'Dialog',
},
dataSourceSettings: {
  dataSource: new DataManager({
    url: '/api/sales',
    adaptor: new UrlAdaptor(),
    crossDomain: true,
  }),
  // rows, columns, values, filters...
}
```

No manual `actionBegin`/`axios` wiring is required — the `UrlAdaptor` handles the protocol for you.

## MSSQL Schema

The `dbo.salesdata` table is expected to exist with the following columns. Adjust names in `SalesController` if your schema differs.

| Column          | Type           | Description                  |
| --------------- | -------------- | ---------------------------- |
| orderid         | INT (PK)       | Primary key, autoincrement   |
| customername    | NVARCHAR       | Customer name                |
| region          | NVARCHAR       | Sales region                 |
| country         | NVARCHAR       | Sales country                |
| productcategory | NVARCHAR       | Product category             |
| productname     | NVARCHAR       | Product name                 |
| orderdate       | DATETIME       | Order date                   |
| quantity        | INT            | Units sold                   |
| unitprice       | DECIMAL(18,2)  | Price per unit               |
| totalamount     | DECIMAL(18,2)  | Total amount (qty × price)   |
| salesperson     | NVARCHAR       | Salesperson name             |

A minimal DDL to create the table:

```sql
CREATE TABLE dbo.salesdata (
    orderid         INT IDENTITY(1,1) PRIMARY KEY,
    customername    NVARCHAR(200)   NULL,
    region          NVARCHAR(100)   NULL,
    country         NVARCHAR(100)   NULL,
    productcategory NVARCHAR(100)   NULL,
    productname     NVARCHAR(200)   NULL,
    orderdate       DATETIME        NULL,
    quantity        INT             NULL,
    unitprice       DECIMAL(18, 2)  NOT NULL DEFAULT 0,
    totalamount     DECIMAL(18, 2)  NOT NULL DEFAULT 0,
    salesperson     NVARCHAR(200)   NULL
);
```

## Troubleshooting

- **CORS errors:** Confirm the client's origin is listed in the `ReactClient` policy in `Program.cs`, and that the API is running.
- **Empty PivotTable:** Verify the `SalesDb` connection string, that `salesdb` exists, and that `dbo.salesdata` has rows.
- **Login / authentication errors:** Adjust the connection string (for example, add `User Id=...;Password=...;` for SQL auth, or `Integrated Security=...` for Windows auth).
- **Schema mismatches:** If your column names differ, update both the `SELECT *` query and the field mappings in `SalesController.cs`.

## See Also

- [Syncfusion React PivotTable Documentation](https://ej2.syncfusion.com/react/documentation/pivotview/getting-started)
- [Syncfusion DataManager UrlAdaptor](https://ej2.syncfusion.com/react/documentation/data/getting-started)
- [Microsoft.Data.SqlClient Documentation](https://learn.microsoft.com/en-us/sql/connect/ado-net/introduction-microsoft-data-sqlclient-namespace)
