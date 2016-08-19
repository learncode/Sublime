using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SublimeDal.Library {
   public static class StringExtensions {
      private static readonly string paramPrefix = "@";
      public static string GetParameterName(this string current) {
         return string.Format("{0}{1}", paramPrefix, current);
      }
   }
}
