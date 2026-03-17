using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.ComponentModel.DataAnnotations;
using Syncfusion.EJ2.Base;
using Microsoft.Data.SqlClient;

namespace MyWebService.Controllers
{
    [ApiController]
    public class PivotController : ControllerBase
    {
        string ConnectionString = @"Server=localhost;Database=MyAppDB;Trusted_Connection=True;TrustServerCertificate=True;";

        /// <summary>
        /// Handles the DataManager request and returns data to the client.
        /// </summary>
        /// <param name="DataManagerRequest">Contains the details of the data operation requested.</param>
        /// <returns>Returns the data records along with the total count.</returns>    
        [HttpPost]
        [Route("api/[controller]")]
        public object Post([FromBody] DataManagerRequest DataManagerRequest)
        {
            // Retrieve data from the data source (e.g., database).
            IQueryable<Orders> DataSource = GetOrderData().AsQueryable();

            // Get the total count of records.
            int totalRecordsCount = DataSource.Count();

            // Return data based on the request.
            return new { result = DataSource, count = totalRecordsCount };
        }

        /// <summary>
        /// Retrieves the order data from the database.
        /// </summary>
        /// <returns>Returns a list of orders fetched from the database.</returns>
        [HttpGet]
        [Route("api/[controller]")]
        public List<Orders> GetOrderData()
        {
            string queryStr = "SELECT * FROM dbo.Orders ORDER BY OrderID;";
            SqlConnection sqlConnection = new(ConnectionString);
            sqlConnection.Open();
            SqlCommand sqlCommand = new(queryStr, sqlConnection);
            SqlDataAdapter DataAdapter = new(sqlCommand);
            DataTable DataTable = new();
            DataAdapter.Fill(DataTable);
            sqlConnection.Close();

            // Map data to a list. 
            List<Orders> dataSource = (from DataRow Data in DataTable.Rows
                select new Orders()
                {
                    OrderID = Convert.ToInt32(Data["OrderID"]),
                    CustomerID = Data["CustomerID"].ToString(),
                    EmployeeID = Convert.IsDBNull(Data["EmployeeID"]) ? 0 : Convert.ToUInt16(Data["EmployeeID"]),
                    ShipCity = Data["ShipCity"].ToString(),
                    Freight = Convert.ToDecimal(Data["Freight"])
                }
            ).ToList();
            return dataSource;
        }
        /// <summary>
        /// Inserts a new data item into the data collection.
        /// </summary>
        /// <param name="value">It contains the new record detail which is need to be inserted.</param>
        /// <returns>Returns void</returns>
        [HttpPost]
        [Route("api/[controller]/Insert")]
        public void Insert([FromBody] CRUDModel<Orders> value)
        {
            string Query = $"Insert into Orders(CustomerID,Freight,ShipCity,EmployeeID) values('{value.value.CustomerID}','{value.value.Freight}','{value.value.ShipCity}','{value.value.EmployeeID}')";
            SqlConnection SqlConnection = new SqlConnection(ConnectionString);
            SqlConnection.Open();
            //Execute the SQL Command.
            SqlCommand SqlCommand = new SqlCommand(Query, SqlConnection);
            //Execute this code to reflect the changes into the database.
            SqlCommand.ExecuteNonQuery();
            SqlConnection.Close();

            //Add custom logic here if needed and remove above method.
        }

        /// <summary>
        /// Update a existing data item from the data collection.
        /// </summary>
        /// <param name="value">It contains the updated record detail which is need to be updated.</param>
        /// <returns>Returns void.</returns>
        [HttpPost]
        [Route("api/[controller]/Update")]
        public void Update([FromBody] CRUDModel<Orders> value)
        {
            //Create query to update the changes into the database by accessing its properties.
            string Query = $"Update Orders set CustomerID='{value.value.CustomerID}', Freight='{value.value.Freight}',EmployeeID='{value.value.EmployeeID}',ShipCity='{value.value.ShipCity}' where OrderID='{value.value.OrderID}'";
            SqlConnection SqlConnection = new SqlConnection(ConnectionString);
            SqlConnection.Open();

            //Execute the SQL command.
            SqlCommand SqlCommand = new SqlCommand(Query, SqlConnection);

            //Execute this code to reflect the changes into the database.
            SqlCommand.ExecuteNonQuery();
            SqlConnection.Close();

            //Add custom logic here if needed and remove above method.
        }

        /// <summary>
        /// Remove a specific data item from the data collection.
        /// </summary>
        /// <param name="value">It contains the specific record detail which is need to be removed.</param>
        /// <return>Returns void.</return>
        [HttpPost]
        [Route("api/[controller]/Remove")]
        public void Remove([FromBody] CRUDModel<Orders> value)
        {
            //Create query to remove the specific from database by passing the primary key column value.
            string Query = $"Delete from Orders where OrderID={value.key}";
            SqlConnection SqlConnection = new SqlConnection(ConnectionString);
            SqlConnection.Open();

            //Execute the SQL command.
            SqlCommand SqlCommand = new SqlCommand(Query, SqlConnection);

            //Execute this code to reflect the changes into the database.
            SqlCommand.ExecuteNonQuery();
            SqlConnection.Close();

            //Add custom logic here if needed and remove above method.
        }

        public class Orders
        {
            [Key]
            public int? OrderID { get; set; }
            public string? CustomerID { get; set; }
            public int? EmployeeID { get; set; }
            public decimal? Freight { get; set; }
            public string? ShipCity { get; set; }
        }
        public class CRUDModel<T> where T : class
        {
            public string? action { get; set; }
            public string? keyColumn { get; set; }
            public object? key { get; set; }
            public T? value { get; set; }
            public List<T>? added { get; set; }
            public List<T>? changed { get; set; }
            public List<T>? deleted { get; set; }
            public IDictionary<string, object>? @params { get; set; }
        }
    }
}