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
               propertyInfo.SetValue(obj, MassageValue(row[column.Name], propertyInfo.PropertyType));
            }
            entities.Add(obj);
         }

         return entities;
      }

      static object MassageValue(object o, Type t) {
         if (t == typeof(int)) {
            return o == DBNull.Value ? default(int) : o;
         }

         if(t == typeof(decimal)) {
            return o == DBNull.Value ? default(decimal) : o;
         }

         if (t == typeof(DateTime)) {
            return o == DBNull.Value ? default(DateTime) : o;
         }

         if (t == typeof(double)) {
            return o == DBNull.Value ? default(double) : o;
         }

         if (t == typeof(string)) {
            return o == DBNull.Value ? default(string) : o;
         }

         return o;
      }
   }
}
