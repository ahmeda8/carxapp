using System;


namespace carXapp2
{
    public class FuelRecord : ViewModelBase
    {
        private DateTime _PreciseDateTime;
        public DateTime PreciseDateTime
        {
            get { return _PreciseDateTime; }
            set
            {
                _PreciseDateTime = value;
                NotifyPropertyChanged("PreciseDateTime");
            }
        }

        private string _date;
        public string Date
        {
            get { return _date; }
            set
            {
                _date = value;
                NotifyPropertyChanged("Date");
            }
        }

        private string _odometer;
        public string Odometer
        {
            get { return _odometer; }
            set
            {
                _odometer = value;
                NotifyPropertyChanged("Odometer");
            }
        }

        private string _cost;
        public string Cost
        {
            get { return _cost; }
            set
            {
                _cost = value;
                NotifyPropertyChanged("Cost");
            }
        }

        private string _fill;
        public string Fill
        {
            get { return _fill; }
            set
            {
                _fill = value;
                NotifyPropertyChanged("Fill");
            }
        }

    }
}
