using System;

namespace carXapp2
{
    public class CarListItem : ViewModelBase
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

        private string _year;
        public string Year
        {
            get { return _year; }
            set
            {
                _year = value;
                NotifyPropertyChanged("Year");
            }
        }

        private string _make;
        public string Make
        {
            get { return _make; }
            set
            {
                _make = value;
                NotifyPropertyChanged("Make");
            }
        }

        private string _model;
        public string Model
        {
            get { return _model; }
            set
            {
                _model = value;
                NotifyPropertyChanged("Model");
            }
        }

        private string _lic;
        public string Lic
        {
            get { return _lic; }
            set
            {
                _lic = value;
                NotifyPropertyChanged("Lic");
            }
        }

    }
}
