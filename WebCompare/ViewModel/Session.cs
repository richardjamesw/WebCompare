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

namespace WebCompare.ViewModel
{
    public sealed class Session
    {
        #region Instance Variables & Constructor
        private WebCompareModel wcModel = new WebCompareModel();
        private WebCompareViewModel wcViewModel = WebCompareViewModel.Instance;
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
            Action<object> x = null;
            if (wcViewModel.UserURL != null)
            {
                wcViewModel.DataDump = GetWebData(wcViewModel.UserURL);
            }
            
            // Get Data from Websites
            //foreach (string url in wcModel.Websites)
            //{
            //    wcViewModel.DataDump += url + ": " + GetWebData(url);
            //}



            // Calculate cosine vectors

            // Compare to the entered URL by the user

            // Display the results in order

            return  x;

        }

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
            catch (Exception e) {
                MessageBox.Show("Exception caught: " + e, "Exception:Session:GetWebData()", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            return null;
        }

        #endregion


    }// End Session class
}// Namespace
