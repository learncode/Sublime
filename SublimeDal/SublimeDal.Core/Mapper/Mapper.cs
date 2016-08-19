using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace SublimeDal.Library {
   public class Mapper {
      public static List<T> GetList<T>(DataTable dataTable) where T : new() {
         List<T> entities = new List<T>();

         foreach (DataRow row in dataTable.Rows) {
            T obj = new T();
            foreach (PropertyInfo propertyInfo in new T().GetType().GetProperties()) {
               var column = (Column)propertyInfo.GetCustomAttribute(typeof(Column));
               if (column == null)
                  continue;
               propertyInfo.SetValue(obj, row[column.Name] == DBNull.Value ? null : row[column.Name]);
            }
            entities.Add(obj);
         }

         return entities;
      }
   }
}
