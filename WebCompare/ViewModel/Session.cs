using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;    //for iNotifyPropertyChanged
using System.Windows.Input; // ICommand
using WebCompare.Model;

namespace WebCompare.ViewModel
{
   public sealed class Session
   {
      #region Instance Variables & Constructor
      private readonly WebCompareModel model;
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
      }
      #endregion

      #region INotifyPropertyChanged
      public event PropertyChangedEventHandler PropertyChanged;
      private void NotifyPropertyChanged(string str)
      {
         PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(str));
      }
      #endregion

      #region Properties
      public ICommand BtnGo
      {
         get { return new Commands.DelegateCommand(Start()); }
      }

      #endregion

      #region Commands

      #endregion


      #region Session

      private Action<object> Start()
      {
         // Get Data from Websites

         // Calculate cosine vectors

         // Compare to the entered URL by the user

         // Display the results in order
         
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
         catch { }

         return null;
      }

      #endregion


   }// End Session class
}// Namespace
