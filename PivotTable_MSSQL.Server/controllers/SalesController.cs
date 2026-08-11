using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Data;
using Microsoft.Data.SqlClient;

namespace PivotTable_MSSQL.Server.Controllers
{
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly string _connectionString;

        public SalesController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SalesDb")
                ?? throw new InvalidOperationException(
                    "Connection string 'SalesDb' is not configured.");
        }

        /// <summary>
        /// Handles the DataManager request and returns data to the client.
        /// </summary>
        /// <param name="DataManagerRequest">Contains the details of the data operation requested.</param>
        /// <returns>Returns the data records along with the total count.</returns>
        [HttpPost]
        [Route("api/[controller]")]
        public object Post([FromBody] object DataManagerRequest)
        {
            // Retrieve data from the data source (database).
            IQueryable<SalesData> DataSource = GetSalesData().AsQueryable();
            int totalRecordsCount = DataSource.Count();
            // Return data based on the request.
            return new { result = DataSource, count = totalRecordsCount };
        }

        /// <summary>
        /// Retrieves the sales data from the database.
        /// </summary>
        /// <returns>Returns a list of SalesData records fetched from the database.</returns>
        [HttpGet]
        [Route("api/[controller]")]
        public async Task<List<SalesData>> GetSalesData()
        {
            const string Query = @"SELECT * FROM dbo.salesdata ORDER BY orderid;";

            using var Connection = new SqlConnection(_connectionString);
            await Connection.OpenAsync();

            using var Command = new SqlCommand(Query, Connection);
            using var DataAdapter = new SqlDataAdapter(Command);
            var DataTable = new DataTable();
            DataAdapter.Fill(DataTable);

            var DataSource = (from DataRow Data in DataTable.Rows
                              select new SalesData
                              {
                                  OrderID = Data["orderid"] == DBNull.Value ? (int?)null : Convert.ToInt32(Data["orderid"]),
                                  CustomerName = Data["customername"] == DBNull.Value ? null : Data["customername"].ToString(),
                                  Region = Data["region"] == DBNull.Value ? null : Data["region"].ToString(),
                                  Country = Data["country"] == DBNull.Value ? null : Data["country"].ToString(),
                                  ProductCategory = Data["productcategory"] == DBNull.Value ? null : Data["productcategory"].ToString(),
                                  ProductName = Data["productname"] == DBNull.Value ? null : Data["productname"].ToString(),
                                  OrderDate = Data["orderdate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(Data["orderdate"]),
                                  Quantity = Data["quantity"] == DBNull.Value ? (int?)null : Convert.ToInt32(Data["quantity"]),
                                  UnitPrice = Data["unitprice"] == DBNull.Value ? 0m : Convert.ToDecimal(Data["unitprice"]),
                                  TotalAmount = Data["totalamount"] == DBNull.Value ? 0m : Convert.ToDecimal(Data["totalamount"]),
                                  SalesPerson = Data["salesperson"] == DBNull.Value ? null : Data["salesperson"].ToString()
                              }
            ).ToList();
            return DataSource;
        }

        public class SalesData
        {
            [Key]
            public int? OrderID { get; set; }
            public string? CustomerName { get; set; }
            public string? Region { get; set; }
            public string? Country { get; set; }
            public string? ProductCategory { get; set; }
            public string? ProductName { get; set; }
            public DateTime? OrderDate { get; set; }
            public int? Quantity { get; set; }
            public decimal UnitPrice { get; set; }
            public decimal TotalAmount { get; set; }
            public string? SalesPerson { get; set; }
        }

        /// <summary>
        /// Generic model for handling CRUD operations from the Pivot Table.
        /// The Pivot Table uses this structure to send data to Insert, Update, and Remove endpoints.
        /// </summary>
        /// <typeparam name="T">The data type (e.g., SalesData)</typeparam>
        public class CRUDModel<T> where T : class
        {
            /// <summary>
            /// Action being performed (e.g., "insert", "update", "remove").
            /// </summary>
            public string? Action { get; set; }

            /// <summary>
            /// Primary key column name (e.g., "orderid").
            /// </summary>
            public string? KeyColumn { get; set; }

            /// <summary>
            /// The primary key value (e.g., the OrderID).
            /// </summary>
            public object? Key { get; set; }

            /// <summary>
            /// The single record being operated on (for Insert, Update operations).
            /// </summary>
            public T? Value { get; set; }

            /// <summary>
            /// Additional parameters sent by the client.
            /// </summary>
            public IDictionary<string, object>? Params { get; set; }
        }

        /// <summary>
        /// Inserts a new sales record into the database.
        /// This method is called when a new row is added in the Pivot Table.
        /// </summary>
        /// <param name="value">Contains the new sales data to insert.</param>
        /// <returns>Returns the inserted record with its new OrderID.</returns>
        [HttpPost]
        [Route("api/[controller]/Insert")]
        public async Task<IActionResult> Insert([FromBody] CRUDModel<SalesData> model)
        {
            if (model?.Value == null)
                return BadRequest("A sales record is required.");

            if (string.IsNullOrWhiteSpace(model.Value.CustomerName) ||
                string.IsNullOrWhiteSpace(model.Value.Country) ||
                model.Value.OrderDate == null ||
                model.Value.Quantity == null ||
                model.Value.Quantity <= 0 ||
                model.Value.UnitPrice < 0)
            {
                return BadRequest("Required fields, a positive quantity, and a non-negative unit price are required.");
            }

            try
            {
                model.Value.TotalAmount =
                    model.Value.Quantity.Value * model.Value.UnitPrice;
                const string sql = @"
            INSERT INTO dbo.salesdata
            (customername, region, country, productcategory, productname, orderdate, quantity, unitprice, totalamount, salesperson)
            OUTPUT INSERTED.orderid
            VALUES
            (@CustomerName, @Region, @Country, @ProductCategory, @ProductName, @OrderDate, @Quantity, @UnitPrice, @TotalAmount, @SalesPerson);
        ";

                using var conn = new SqlConnection(_connectionString);
                await conn.OpenAsync();

                using var cmd = new SqlCommand(sql, conn);

                // Add parameters to prevent SQL injection
                cmd.Parameters.AddWithValue("@CustomerName", (object?)model.Value.CustomerName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Region", (object?)model.Value.Region ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Country", (object?)model.Value.Country ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ProductCategory", (object?)model.Value.ProductCategory ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ProductName", (object?)model.Value.ProductName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@OrderDate", (object?)model.Value.OrderDate ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Quantity", (object?)model.Value.Quantity ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@UnitPrice", model.Value.UnitPrice);
                cmd.Parameters.AddWithValue("@TotalAmount", model.Value.TotalAmount);
                cmd.Parameters.AddWithValue("@SalesPerson", (object?)model.Value.SalesPerson ?? DBNull.Value);

                // Execute the query and get the newly created OrderID
                var newId = Convert.ToInt32(await cmd.ExecuteScalarAsync());

                // Update the model with the new ID
                model.Value.OrderID = newId;

                // UrlAdaptor expects { key, value, action } on insert.
                return Ok(new { key = newId, value = model.Value, action = "insert" });
            }
            catch (Exception)
            {
                // Log the exception in a real application. Returning a generic
                // message avoids leaking database or stack details to the client.
                return StatusCode(500, new { error = "Insert failed." });
            }
        }

        /// <summary>
        /// Updates an existing sales record in the database.
        /// This method is called when a row is edited in the Pivot Table.
        /// </summary>
        /// <param name="value">Contains the updated sales data.</param>
        /// <returns>Returns the number of rows updated.</returns>
        [HttpPost]
        [Route("api/[controller]/Update")]
        public async Task<IActionResult> Update([FromBody] CRUDModel<SalesData> model)
        {
            if (model?.Value == null)
                return BadRequest("A sales record is required.");

            if (string.IsNullOrWhiteSpace(model.Value.CustomerName) ||
                string.IsNullOrWhiteSpace(model.Value.Country) ||
                model.Value.OrderDate == null ||
                model.Value.Quantity == null ||
                model.Value.Quantity <= 0 ||
                model.Value.UnitPrice < 0)
            {
                return BadRequest("Required fields, a positive quantity, and a non-negative unit price are required.");
            }

            try
            {
                model.Value.TotalAmount =
                    model.Value.Quantity.Value * model.Value.UnitPrice;
                const string sql = @"
            UPDATE dbo.salesdata
            SET customername    = @CustomerName,
                region          = @Region,
                country         = @Country,
                productcategory = @ProductCategory,
                productname     = @ProductName,
                orderdate       = @OrderDate,
                quantity        = @Quantity,
                unitprice       = @UnitPrice,
                totalamount     = @TotalAmount,
                salesperson     = @SalesPerson
            WHERE orderid = @OrderID;
        ";

                using var conn = new SqlConnection(_connectionString);
                await conn.OpenAsync();

                using var cmd = new SqlCommand(sql, conn);

                // Add parameters to prevent SQL injection
                cmd.Parameters.AddWithValue("@CustomerName", (object?)model.Value.CustomerName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Region", (object?)model.Value.Region ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Country", (object?)model.Value.Country ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ProductCategory", (object?)model.Value.ProductCategory ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ProductName", (object?)model.Value.ProductName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@OrderDate", (object?)model.Value.OrderDate ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Quantity", (object?)model.Value.Quantity ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@UnitPrice", model.Value.UnitPrice);
                cmd.Parameters.AddWithValue("@TotalAmount", model.Value.TotalAmount);
                cmd.Parameters.AddWithValue("@SalesPerson", (object?)model.Value.SalesPerson ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@OrderID", model.Value.OrderID);

                // Execute the update
                var rows = await cmd.ExecuteNonQueryAsync();

                // UrlAdaptor expects { key, value, action } on update.
                return Ok(new { key = model.Value.OrderID, value = model.Value, action = "update" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Update failed." });
            }
        }

        /// <summary>
        /// Deletes a sales record from the database.
        /// This method is called when a row is deleted in the Pivot Table.
        /// </summary>
        /// <param name="value">Contains the OrderID of the record to delete.</param>
        /// <returns>Returns the number of rows deleted.</returns>
        [HttpPost]
        [Route("api/[controller]/Remove")]
        public async Task<IActionResult> Remove([FromBody] CRUDModel<SalesData> model)
        {
            if (model?.Key == null)
                return BadRequest("Missing key.");

            if (!int.TryParse(model.Key.ToString(), out var id))
                return BadRequest("Invalid OrderID.");

            try
            {
                const string sql = @"DELETE FROM dbo.salesdata WHERE orderid = @OrderID;";

                using var conn = new SqlConnection(_connectionString);
                await conn.OpenAsync();

                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@OrderID", id);

                // Execute the delete
                var rows = await cmd.ExecuteNonQueryAsync();

                // UrlAdaptor expects { key, action } on remove.
                return Ok(new { key = id, action = "remove" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Delete failed." });
            }
        }

    }
}
