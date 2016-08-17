# Sublime
SublimeDAL is a lightwieght C# based ORM. It's inspired by Daapper. I'm building this as an exercise to learn how a production quality ORM needs to be built and also the performance criteria that needs to be considered. Reiterating, eventually the end goal is to learn how to design a functional ORM. 

### Initializing the context
   ```
   DbContext context = new DbContext(new ConnectionProvider(new ConnectionStringProvider()));
   string connectionStringKey = @"northwind";
   ```

### Simple select query
   ```
   string query = @"SELECT TOP 10 * FROM customers";
   List<Customer> customers = context.Query<Customer>(connectionStringKey, query, null);
   ```

### Parametrized SQL query
   ```
   string parameterizedQuery = @"SELECT * FROM Customers WHERE CustomerID = @customerId";
   string paramName = "customerId";
   string customerId = @"ALFKI";
   Tuple<string, object> t = new Tuple<string, object>(paramName, customerId);
   customers = context.Query<Customer>(connectionStringKey, parameterizedQuery, new List<Tuple<string, object>>() { t });
   ```

### Executing a procedure
   ```
   Tuple<string, object, DbType, ParameterDirection> procedureParam = new Tuple<string, object, DbType, ParameterDirection>("OrderID", 10248, DbType.Int32, ParameterDirection.Input);
   List<CustomerOrderDetail> details = context.Procedure<CustomerOrderDetail>(connectionStringKey, "dbo.CustOrdersDetail", new List<Tuple<string, object, DbType, ParameterDirection>>() { procedureParam });
   ```

### Insert data
   ```
   string insertQuery = @"insert into region (RegionID, RegionDescription)values (@regionId, @description)";
   var valueTupleList = new List<Tuple<string, object>>();
   Tuple<string, object> t1 = new Tuple<string, object>("regionId", 5);
   Tuple<string, object> t2 = new Tuple<string, object>("description", "MidWestern");

   int result = context.ExecuteQuery(connectionStringKey, insertQuery, new List<Tuple<string, object>>() { t1, t2 });
   ```
   
### Update data
   ```
   string updateQuery = @"UPDATE region SET RegionDescription = @description WHERE RegionID = @regionId";
   t2 = new Tuple<string, object>("description", "Mid-Western");
   result = context.ExecuteQuery(connectionStringKey, updateQuery, new List<Tuple<string, object>>() { t1, t2 });
   ```
   
### Delete data
   ```
   string deleteQuery = @"DELETE FROM region WHERE RegionID = @regionId";
   result = context.ExecuteQuery(connectionStringKey, deleteQuery, new List<Tuple<string, object>>() { t1 });
   ```
   
##Current known limitations
   ```
   * Cannot perform multiple inserts.
   ```
