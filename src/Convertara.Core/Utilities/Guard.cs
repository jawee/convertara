using System;

namespace Convertara.Core.Utilities;

public static class Guard
{
   public static void IsNullCheck(object arg, string name)
   {
      if (arg == null)
      {
         throw new ArgumentNullException($"{name} is null");
      }
   }
}