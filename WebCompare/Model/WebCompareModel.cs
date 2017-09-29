using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace WebCompare.Model
{
    public class WebCompareModel
    {
        // Websites
        private static string[] websites = {   "https://stocktwits.com/symbol/DOW",
      "https://stocktwits.com/symbol/SPX", "https://stocktwits.com/symbol/GOOG",
      "https://stocktwits.com/symbol/AAPL", "https://stocktwits.com/symbol/MSFT",
      "https://stocktwits.com/symbol/NVDA", "https://stocktwits.com/symbol/TWTR",
      "https://stocktwits.com/symbol/FB", "https://stocktwits.com/symbol/BBRY",
      "https://stocktwits.com/symbol/ORCL"   };

        public static string[] Websites
        {
            get
            {
                return websites;
            }
        }

        public static string RandomWebsite
        {
            get
            {
                return "https://en.wikipedia.org/wiki/Special:Random";
            }
        }

        #region Helper Methods

        /// <summary>
        /// Get the data from a website as a string
        /// </summary>
        /// <param name="url">website to pull data from</param>
        public static string GetWebData(string url)
        {

            try
            {
                using (WebClient client = new WebClient())
                {
                    string s = client.DownloadString(url);
                    return s;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Exception caught: " + e, "Exception:Session:GetWebData()", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            return null;
        }

        /*public static string GetWebData2(string url)
        {
            try
            {
                string line = "";
                string parsed = "";
                WebRequest webRequest;
                webRequest = WebRequest.Create(url);

                Stream objStream;
                objStream = webRequest.GetResponse().GetResponseStream();

                StreamReader objReader = new StreamReader(objStream);

                while (objReader != null)
                {
                    line = objReader.ReadLine();
                    parsed += Parser(line) + "\n";
                }
                return parsed;
            }
            catch (Exception e)
            {
                MessageBox.Show("Exception caught: " + e, "Exception:Session:GetWebData()", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            return null;
        }*/


        /// <summary>
        /// Parse the data using regex and weighted delimiters
        /// </summary>
        /// <param name="data">data to parse</param>
        public static string[] Parser(string[] data)
        {
            if (data == null) return null;

            string[] output = null;

            try
            {
                string temp = "";

                string regex = @"(body&quot;:&quot;).*(&quot;,&quot;links)";
                //string regexWiki = @"<p>\w+</p>";
                string regexStock = "(?<=(body&quot;:&quot;)).*(?=(&quot;,&quot;links))";
                string regexStock2 = @"(?<=(body&quot;:&quot;))(\w|\d|\n|[().,\-:;@#$%^&*\[\]'+–/\/®°⁰!?{}|`~]| )+?(?=(&quot;,&quot;links))";
                //string regexStock3 = "(?<=(<ol class='stream-list.*&quot;body&quot;:&quot;))(\\w|\\d|\n|^quot|[().,\\-:;@#$%^&*\\[\\]'+–/\\/®°⁰!?{}|`~]| )+?(?=(&quot;,&quot;links))";

                foreach (var s in data)
                {
                    var tempParsed = Regex.Matches(s, regexStock2, RegexOptions.Singleline);
                    foreach (var m in tempParsed)
                    {
                        temp += m.ToString() + ' ';
                    }
                }

                output = temp.Split(' ');
            }
            catch { }

            return output;
        }


        // Similarity
        public static double CosineSimilarity(double[] tableA, double[] tableB)
        {
            double dotProduct = 0.0, normA = 0.0, normB = 0.0;

            for (int i = 0; i < tableA.Length; i++)
            {
                dotProduct += tableA[i] * tableB[i];
                normA += Math.Pow(tableA[i], 2);
                normB += Math.Pow(tableB[i], 2);
            }
            return dotProduct / (Math.Sqrt(normA) * Math.Sqrt(normB));
        }

        #endregion


    }
}
