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
      private string[] websites = {   "https://en.wikipedia.org/wiki/Buffalo_Bills",
      "https://en.wikipedia.org/wiki/New_York_Giants", "https://en.wikipedia.org/wiki/New_York_Jets",
      "https://en.wikipedia.org/wiki/New_England_Patriots", "https://en.wikipedia.org/wiki/Philadelphia_Eagles",
      "https://en.wikipedia.org/wiki/Green_Bay_Packers", "https://en.wikipedia.org/wiki/Dallas_Cowboys",
      "https://en.wikipedia.org/wiki/Miami_Dolphins", "https://en.wikipedia.org/wiki/Cleveland_Browns",
      "https://en.wikipedia.org/wiki/Pittsburgh_Steelers"   };

      public string[] Websites
      {
         get
         {
            return websites;
         }
      }


   }
}
