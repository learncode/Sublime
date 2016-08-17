using System;
using System.Collections.Generic;
using System.Data;
using SublimeDal.Core.Context;
using SublimeDal.Library;

namespace SublimeDal.Console {
   public class Customer {
      [Column(Name = "CustomerID")]
      public string CustomerId { get; set; }
      [Column(Name = "CompanyName")]
      public string CompanyName { get; set; }
      [Column(Name = "ContactTitle")]
      public string Title { get; set; }
      [Column(Name = "Region")]
      public string Region { get; set; }
   }

   public class CustomerOrderDetail {
      [Column(Name = "UnitPrice")]
      public decimal UnitPrice { get; set; }
      [Column(Name = "ProductName")]
      public string ProductName { get; set; }
      [Column(Name = "Quantity")]
      public int Quantity { get; set; }
      [Column(Name = "Discount")]
      public int Discount { get; set; }
      [Column(Name = "ExtendedPrice")]
      public decimal ExtendedPrice { get; set; }
   }

   class Program {
      static void Main(string[] args) {
         DbContext context = new DbContext(new ConnectionProvider(new ConnectionStringProvider()));
         string connectionStringKey = @"northwind";

         #region Simple select query
         string query = @"SELECT TOP 10 * FROM customers";
         List<Customer> customers = context.Query<Customer>(connectionStringKey, query, null);
         #endregion

         #region Parametrized SQL query
         string parameterizedQuery = @"SELECT * FROM Customers WHERE CustomerID = @customerId";
         string paramName = "customerId";
         string customerId = @"ALFKI";
         Tuple<string, object> t = new Tuple<string, object>(paramName, customerId);
         customers = context.Query<Customer>(connectionStringKey, parameterizedQuery, new List<Tuple<string, object>>() { t });
         #endregion

         #region execute proc
         Tuple<string, object, DbType, ParameterDirection> procedureParam = new Tuple<string, object, DbType, ParameterDirection>("OrderID", 10248, DbType.Int32, ParameterDirection.Input);
         List<CustomerOrderDetail> details = context.Procedure<CustomerOrderDetail>(connectionStringKey, "dbo.CustOrdersDetail", new List<Tuple<string, object, DbType, ParameterDirection>>() { procedureParam });
         #endregion

         #region insert/update/delete data
         string insertQuery = @"insert into region (RegionID, RegionDescription)values (@regionId, @description)";
         var valueTupleList = new List<Tuple<string, object>>();
         Tuple<string, object> t1 = new Tuple<string, object>("regionId", 5);
         Tuple<string, object> t2 = new Tuple<string, object>("description", "MidWestern");

         int result = context.ExecuteQuery(connectionStringKey, insertQuery, new List<Tuple<string, object>>() { t1, t2 });

         string updateQuery = @"UPDATE region SET RegionDescription = @description WHERE RegionID = @regionId";
         t2 = new Tuple<string, object>("description", "Mid-Western");
         result = context.ExecuteQuery(connectionStringKey, updateQuery, new List<Tuple<string, object>>() { t1, t2 });

         string deleteQuery = @"DELETE FROM region WHERE RegionID = @regionId";
         result = context.ExecuteQuery(connectionStringKey, deleteQuery, new List<Tuple<string, object>>() { t1 });
         #endregion
      }
   }
}
