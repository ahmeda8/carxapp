using System;
using System.IO.IsolatedStorage;


namespace carXapp2
{
    public class FuelListItem : ViewModelBase
    {
        private int _fuelID;
        public int FuelID
        {
            get
            {
                return _fuelID;
            }
            set
            {
                _fuelID = value;
                NotifyPropertyChanged("FuelID");
            }
        }

        private string _date;
        public string Date
        {
            get
            {
                return _date;
            }
            set
            {
                _date = value;
                NotifyPropertyChanged("Date");
            }
        }

        private string _mpgUnit;
        public string MpgUnit
        {
            get
            {

                return _mpgUnit;
            }
            set
            {
                _mpgUnit = value;
                NotifyPropertyChanged("MpgUnit");
            }
        }

        private string _currencyUnit;
        public string CurrencyUnit
        {
            get
            {

                return _currencyUnit;
            }
            set
            {
                _currencyUnit = value;
                NotifyPropertyChanged("CurrencyUnit");
            }
        }

        private string _distanceUnit;
        public string DistanceUnit
        {
            get
            {

                return _distanceUnit;
            }
            set
            {
                _distanceUnit = value;
                NotifyPropertyChanged("DistanceUnit");
            }
        }

        private string _volumeUnit;
        public string VolumeUnit
        {
            get
            {

                return _volumeUnit;
            }
            set
            {
                _volumeUnit = value;
                NotifyPropertyChanged("VolumeUnit");
            }
        }

        private float _fill;
        public float Fill
        {
            get
            {
                return _fill;
            }
            set
            {
                _fill = value;
                NotifyPropertyChanged("Fill");
            }
        }

        private float _cost;
        public float Cost
        {
            get
            {
                return _cost;
            }
            set
            {
                _cost = value;
                NotifyPropertyChanged("Cost");
            }
        }

        private int _milesDriven;
        public int MilesDriven
        {
            get
            {
                return _milesDriven;
            }
            set
            {
                _milesDriven = value;
                NotifyPropertyChanged("MilesDriven");
            }
        }

        private string _mpg;
        public string MPG
        {
            get
            {
                return _mpg+" "+MpgUnit;
            }
            set
            {
                _mpg = value;
                NotifyPropertyChanged("MPG");
            }
        }
    }
}
