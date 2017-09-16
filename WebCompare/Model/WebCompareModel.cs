using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCompare.Model
{
   public class WebCompareModel
   {
      // Websites
      private static string[] websites = {"https://en.wikipedia.org/wiki/Buffalo_Bills",
      "https://en.wikipedia.org/wiki/New_York_Giants",
      "https://en.wikipedia.org/wiki/New_York_Jets"};

      public string[] Websites
      {
         get
         {
            return websites;
         }
      }


   }
}
