using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;    //for iNotifyPropertyChanged
using System.Windows.Input; // ICommand
using WebCompare.Model;

namespace WebCompare.ViewModel
{
    class WebCompareViewModel : INotifyPropertyChanged
    {
        #region Instance Variables
        private WebCompareModel wcModel;
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
            wcModel = new WebCompareModel();
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
        private string userURL;
        public string UserURL
        {
            get
            {
                return userURL;
            }
            set
            {
                userURL = value;
            }
        }


        public string[] ListWebsites
        {
            get
            {
                return wcModel.Websites;
            }
        }

        private string data_dump = "Enter a URL and press start above.";
        public string DataDump
        {
            get
            {
                return data_dump;
            }

            set
            {
                if (data_dump != value)
                {
                    data_dump += value;
                    NotifyPropertyChanged("DataDump");
                }

            }
        }

        #endregion

        #region Commands
        //Commands.DelegateCommand dc = new Commands.DelegateCommand(Session.Instance.Start());
        // Button to call start method
        public ICommand BtnGo
        {
            get
            {
                return new Commands.DelegateCommand(o => Session.Instance.Start());
            }
        }
        #endregion

    }
}
