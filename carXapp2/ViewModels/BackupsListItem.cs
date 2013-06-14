using System;
using System.Net;
using System.Windows;

namespace carXapp2
{
    public class BackupsListItem : ViewModelBase
    {
        private int _id;
        public int ID
        {
            get { return _id; }
            set
            {
                _id = value;
                NotifyPropertyChanged("ID");
            }
        }

        private string _filename;
        public string Filename
        {
            get { return _filename; }
            set
            {
                _filename = value;
                NotifyPropertyChanged("Filename");
            }
        }

        private string _downloadurl;
        public string DownloadUrl
        {
            get { return _downloadurl; }
            set
            {
                _downloadurl = value;
                NotifyPropertyChanged("DownloadUrl");
            }
        }

        private DateTime _created;
        public DateTime Created
        {
            get { return _created; }
            set
            {
                _created = value;
                NotifyPropertyChanged("Created");
            }
        }

        private string _userid;
        public string UserID
        {
            get { return _userid; }
            set
            {
                _userid = value;
                NotifyPropertyChanged("UserID");
            }
        }
    }
}
