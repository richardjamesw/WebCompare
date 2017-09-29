using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using System.ComponentModel;    //for iNotifyPropertyChanged
using System.Windows.Input; // ICommand
using WebCompare.Model;
using System.Text.RegularExpressions;
using System.IO;
using HtmlAgilityPack;

namespace WebCompare.ViewModel
{
    public class Session : INotifyPropertyChanged
    {
        #region Instance Variables & Constructor

        private WebCompareViewModel wcViewModel = WebCompareViewModel.Instance;
        HTable[] tables = new HTable[WebCompareModel.Websites.Length + 1];
        private static object lockObj = new object();
        private static volatile Session instance;
        public static Session Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObj)
                    {
                        if (instance == null)
                            instance = new Session();
                    }
                }
                return instance;
            }
        }

        public Session()
        {
            for (int t = 0; t < tables.Length; ++t)
            {
                tables[t] = new HTable();
            }
        }
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string str)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(str));
        }
        #endregion

        #region Session

        /// <summary>
        /// Start method
        /// </summary>
        /// <returns></returns>
        public Action<object> Start()
        {
            // if nothings entered in url return
            if (wcViewModel.UserURL == "")
            {
                wcViewModel.DataDump += "\nPlease enter a valid URL";
                return null;
            }

            wcViewModel.DataDump = "";
            NotifyPropertyChanged("DataDump");
            string[] data = null;
            string[] parsedData = null;

            // Get Data from Websites
            for (int w = 0; w <= WebCompareModel.Websites.Length; ++w)
            {
                if (w != WebCompareModel.Websites.Length)
                {
                    // Get data
                    WebCompareViewModel.Instance.DataDump += "\nGETTING data from: " + WebCompareModel.Websites[w];
                    data = GetWebDataAgility(WebCompareModel.Websites[w]);

                    // Parse each message into
                    wcViewModel.DataDump += "\nPARSING data from: " + WebCompareModel.Websites[w];
                    parsedData = WebCompareModel.Parser(data);

                    // Fill respective table
                    wcViewModel.DataDump += "\nFILLING TABLE from: " + WebCompareModel.Websites[w] + "\n";
                    if (parsedData != null)
                    {
                        foreach (string word in parsedData.Where(x => x != ""))
                        {
                            tables[w].Put(word, 1);
                        }
                    }
                }
                else   // We are at the last table, aka the User entered table
                {
                    // Get data
                    wcViewModel.DataDump += "\nGETTING data from USER entered webpage";
                    data = GetWebDataAgility(wcViewModel.UserURL);

                    // Parse each message into
                    wcViewModel.DataDump += "\nPARSING data from USER entered webpage";
                    parsedData = WebCompareModel.Parser(data);

                    // Fill respective table
                    wcViewModel.DataDump += "\nFILLING TABLE from USER entered webpage\n";
                    if (parsedData != null)
                    {
                        foreach (string word in parsedData.Where(x => x != ""))
                        {
                            tables[w].Put(word, 1);
                        }
                    }
                }
                
            }    // End get data from websites

            // Calculate cosine vectors
            wcViewModel.DataDump += "\nCALCULATING cosine vectors\n";
            // Compare to the entered URL by the user

            // Display the results in order

            return null;

        }

        #endregion

        #region Helper Files

        // HTMPAglityPack Get
        private static string[] GetWebDataAgility(string url)
        {
            string[] data = null;
            try
            {
                var webGet = new HtmlWeb();
                var doc = webGet.Load(url);

                var nodes = doc.DocumentNode.SelectNodes("//*[@id=\"updates\"]//li");
                data = new string[nodes.Count()];
                int c = 0;
                foreach (var n in nodes)
                {
                    data[c] = n.GetAttributeValue("data-src", null);
                    ++c;
                }

            }
            catch (Exception e) { Console.WriteLine("Error in GetWebDataAgility(): " + e); }

            return data;
        }

        #endregion



    }// End Session class
}// Namespace




/* tests
 //// test

    //https://twitter.com/StockTwits
            System.IO.File.WriteAllText(@"TestText.html", "");
            string data = WebCompareModel.GetWebData("https://en.wikipedia.org/wiki/Buffalo_Bills");
            System.IO.File.AppendAllText(@"TestText.html", data);


            //System.IO.File.WriteAllText(@"TestText.html", "");
            //data = WebCompareModel.GetWebData(wcViewModel.UserURL);
            //System.IO.File.AppendAllText(@"TestText.html", data);
            //parsedData = WebCompareModel.Parser(data);



    // Title
    // get messages //*[@id="updates"]/li[1]
                //HtmlNode node = doc.DocumentNode.SelectSingleNode("//*[@id=\"updates\"]");

                //HtmlNode node = doc.DocumentNode.SelectSingleNode("//*[@id=\"updates\"]//li[1]");
                //if (node != null) data += node.OuterHtml;

                //node = doc.DocumentNode.SelectSingleNode("//title");
                node = doc.DocumentNode.SelectSingleNode("//*[@id=\"updates\"]");
                if (node != null) data += node.InnerHtml;
                // Messages
                while (node.HasChildNodes)
                {
                    data += "/n" + node.A;
                }

                //HtmlNodeCollection nodes = node.ChildAttributes.Where(["class"].Value = "box"); //= doc.DocumentNode.SelectNodes("//*[@id=\"updates\"]").Where(x => Attributes["class"].Value == "box");

                if (nodes != null)
                {
                    foreach (var x in nodes)
                    {
                        data += "/n" + x.InnerHtml;
                    }
                }
                //.Where(x => x.Attributes["class"].Value == "box"))
 */
