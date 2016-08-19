using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SublimeDal.Library
{
    public static class Require
    {
       public static void NotNull<T>(T value, string message) where T : class{
          if (value == null)
             throw new ArgumentException(message);
       }

       public static void NonNegative(int value, string message) {
          if (value >= 0)
             return;

          throw new ArgumentException(message);
       }

       public static void NonNegative(float value, string message) {
          if (value >= 0)
             return;

          throw new ArgumentException(message);
       }

       public static void NonNegative(double value, string message) {
          if (value >= 0)
             return;

          throw new ArgumentException(message);
       }

       public static void NotNullOrEmptyString(string value, string message) {
          if (!string.IsNullOrWhiteSpace(value))
             return;

          throw new ArgumentException(message);
       }
    }
}
