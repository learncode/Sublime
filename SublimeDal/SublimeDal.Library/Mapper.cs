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
         if (t == typeof(string)) {
            return o == DBNull.Value ? string.Empty : o;
         }

         if (t == typeof(int)) {
            return o == DBNull.Value ? 0 : o;
         }

         if (t == typeof(DateTime)) {
            return o == DBNull.Value ? null : o;
         }
         return o;
      }
   }
}
