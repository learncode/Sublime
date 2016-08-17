using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
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
         string query = @"SELECT TOP 10 * FROM customers";
         List<Customer> customers = context.Query<Customer>(connectionStringKey, query, null);

         string parameterizedQuery = @"SELECT * FROM Customers WHERE CustomerID = @customerId";
         string paramName = "customerId";
         string customerId = @"ALFKI";

         Tuple<string, object> t = new Tuple<string, object>(paramName, customerId);
         customers = context.Query<Customer>(connectionStringKey, parameterizedQuery, new List<Tuple<string, object>>() { t });

         Tuple<string, object, DbType, ParameterDirection> procedureParam = new Tuple<string, object, DbType, ParameterDirection>("OrderID", 10248, DbType.Int32, ParameterDirection.Input);
         List<CustomerOrderDetail> details = context.Procedure<CustomerOrderDetail>(connectionStringKey, "dbo.CustOrdersDetail", new List<Tuple<string, object, DbType, ParameterDirection>>() { procedureParam });
      }
   }
}
