﻿using System;

namespace carXapp2
{
    public class MaintListItem : ViewModelBase
    {
        private int _maintID;
        public int MaintID
        {
            get
            {
                return _maintID;
            }
            set
            {
                _maintID = value;
                NotifyPropertyChanged("MaintID");
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

        private string _partname;
        public string Partname
        {
            get
            {
                return _partname;
            }
            set
            {
                _partname = value;
                NotifyPropertyChanged("Partname");
            }
        }

        private string _partnum;
        public string Partnumber
        {
            get
            {
                return _partnum;
            }
            set
            {
                _partnum = value;
                NotifyPropertyChanged("Partnumber");
            }
        }

        private string _maintType;
        public string Mainttype
        {
            get
            {
                return _maintType;
            }
            set
            {
                _maintType = value;
                NotifyPropertyChanged("Mainttype");
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
    }
}
