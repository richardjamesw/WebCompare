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

namespace WebCompare.ViewModel
{
    public sealed class Session
    {
        #region Instance Variables & Constructor
        private WebCompareModel wcModel = new WebCompareModel();
        private WebCompareViewModel wcViewModel = WebCompareViewModel.Instance;
        HTable[] tables = new HTable[10];
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
            if (wcViewModel.UserURL == null)
                return null;

            //Action<object> x = null;
            string data = "";
            string[] parsedData;

            // test
            //System.IO.File.WriteAllText(@"TestText.html", "");
            //if (wcViewModel.UserURL != null)
            //{
            //    data = GetWebData(wcViewModel.UserURL);
            //    //System.IO.File.AppendAllText(@"TestText.html", data);
            //    parsedData = Parser(data);


            //}

            // Get Data from Websites
            for (int w = 0; w < wcModel.Websites.Length; ++w)
            {
                //if (wcViewModel.UserURL != null)
                // Get data
                data = GetWebData(wcModel.Websites[w]);

                // Parse data
                parsedData = Parser(data);

                // Fill respective table
                if (data != null)
                {
                    foreach (string line in parsedData)
                    {
                        foreach (string s in line.Split(' '))
                        {
                            tables[w].Put(s, 1);
                        }
                    }
                }
            }    // End get data from websites

            //  Get Data from User's website
            data = GetWebData(wcViewModel.UserURL);
            parsedData = Parser(data);
            // Fill table 10
            if (data != null)
            {
                foreach (string line in parsedData)
                {
                    foreach (string s in line.Split(' '))
                    {
                        tables[10].Put(s, 1);
                    }
                }
            }

            // Calculate cosine vectors
            // Compare to the entered URL by the user

            // Display the results in order

            return null;

        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Get the data from a website as a string
        /// </summary>
        /// <param name="url">website to pull data from</param>
        public string GetWebData(string url)
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

        public string GetWebData2(string url)
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
        }


        /// <summary>
        /// Parse the data using regex and weighted delimiters
        /// </summary>
        /// <param name="data">data to parse</param>
        public string[] Parser(string data)
        {
            string regexWiki = @"<p>\w+</p>";
            string regexStock = @"(?<=(body&quot;:&quot;)).*(?=(&quot;,&quot;links))";
            string regexStock2 = @"(?<=(body&quot;:&quot;))(\w|\d|\n|[().,\-:;@#$%^&*\[\]'+–/\/®°⁰!?{}|`~]| )+?(?=(&quot;,&quot;links))";
            string regexStock3 = "(?<=(<ol class='stream-list.*&quot;body&quot;:&quot;))(\\w|\\d|\n|^quot|[().,\\-:;@#$%^&*\\[\\]'+–/\\/®°⁰!?{}|`~]| )+?(?=(&quot;,&quot;links))";
            string[] parsed = Regex.Split(data, regexWiki);
            return parsed;
        }

        // FillTable
        public void FillTable(int num, string[] data)
        {
            if (data != null)
            {
                foreach (string line in data)
                {
                    foreach (string s in line.Split(' '))
                    {
                        tables[num].Put(s, 1);
                    }
                }
            }

        }

        #endregion



    }// End Session class
}// Namespace
