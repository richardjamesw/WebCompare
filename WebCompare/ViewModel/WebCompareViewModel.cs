﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;    //for iNotifyPropertyChanged
using System.Windows.Input; // ICommand
using WebCompare.Model;
using Prism.Commands;

namespace WebCompare.ViewModel
{
    public class WebCompareViewModel : INotifyPropertyChanged
    {
        #region Instance Variables
        private static object lockObj = new object();
        private static volatile WebCompareViewModel instance;

        public static WebCompareViewModel Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObj)
                    {
                        if (instance == null)
                            instance = new WebCompareViewModel();
                    }
                }
                return instance;
            }
        }

        public WebCompareViewModel()
        {
            StartCommand = new DelegateCommand(OnStart, CanStart);
        }
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string str)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(str));
            }
        }
        #endregion

        #region Properties

        // user entereed url
        private string userURL = "https://stocktwits.com/symbol/QQQ";
        public string UserURL
        {
            get
            {
                return userURL;
            }
            set
            {
                userURL = value;
                NotifyPropertyChanged("UserURL");

            }
        }


        public string[] ListWebsites
        {
            get
            {
                return WebCompareModel.Websites;
            }
        }

        private string results = "Results will appear here in descending order of similarity.";
        public string Results
        {
            get
            {
                return results;
            }

            set
            {
                if (results != value)
                {
                    results = value;
                    NotifyPropertyChanged("Results");
                }

            }
        }

        private string status = "Enter a URL and press start above. \n Or try https://stocktwits.com/symbol/GOOG.";
        public string Status
        {
            get
            {
                return status;
            }

            set
            {
                if (status != value)
                {
                    status = value;
                    NotifyPropertyChanged("Status");
                }

            }
        }
        #endregion

        #region Commands

        public DelegateCommand StartCommand { get; private set; }
        private void OnStart()
        {
            Session.Instance.Start();
        }
        private bool CanStart()
        {
            return Session.Instance.CanStart();
        }
        #endregion

    }
}


// Old method
// Button to call start method
//public ICommand BtnGo
//{
//    get
//    {
//        return new Commands.DelegateCommand(o => Session.Instance.Start());
//    }
//}
