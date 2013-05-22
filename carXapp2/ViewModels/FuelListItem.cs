using System;


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

        private string _fill;
        public string Fill
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

        private string _cost;
        public string Cost
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

        private string _milesDriven;
        public string MilesDriven
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
                return _mpg;
            }
            set
            {
                _mpg = value;
                NotifyPropertyChanged("MPG");
            }
        }
    }
}
