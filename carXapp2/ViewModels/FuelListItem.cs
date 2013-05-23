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

        private float _mpg;
        public float MPG
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
