using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;



namespace carXapp2
{
    public class AppDataContext : DataContext
    {
        public AppDataContext(string connectionString)
            : base(connectionString)
        { }

        public Table<carInfo> carInfo;
        public Table<fuelInfo> fuelInfo;
        public Table<settingsInfo> settingsInfo;
        public Table<maintInfo> maintInfo;
        public Table<reminderInfo> reminderInfo;
    }

    #region Table and class for car info

    [Table]
    public class carInfo : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private int _carID;

        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int CarID
        {
            get { return _carID; }
            set
            {
                if (_carID != value)
                {
                    NotifyPropertyChanging("CarID");
                    _carID = value;
                    NotifyPropertyChanged("CarID");
                }
            }
        }

        private string _carMake;

        [Column]

        public string CarMake
        {
            get { return _carMake; }
            set
            {
                if (_carMake != value)
                {
                    NotifyPropertyChanging("CarMake");
                    _carMake = value;
                    NotifyPropertyChanged("CarMake");
                }
            }
        }

        private string _carModel;

        [Column]

        public string CarModel
        {
            get { return _carModel; }
            set
            {
                if (_carModel != value)
                {
                    NotifyPropertyChanging("CarModel");
                    _carModel = value;
                    NotifyPropertyChanged("CarModel");
                }
            }
        }

        private string _carYear;

        [Column]

        public string CarYear
        {
            get { return _carYear; }
            set
            {
                if (_carYear != value)
                {
                    NotifyPropertyChanging("CarYear");
                    _carYear = value;
                    NotifyPropertyChanged("CarYear");
                }
            }
        }

        private string _carLic;

        [Column]

        public string CarLic
        {
            get { return _carLic; }
            set
            {
                if (_carLic != value)
                {
                    NotifyPropertyChanging("CarLic");
                    _carLic = value;
                    NotifyPropertyChanged("CarLic");
                }
            }
        }

        private string _carVin;

        [Column]

        public string CarVin
        {
            get { return _carVin; }
            set
            {
                if (_carVin != value)
                {
                    NotifyPropertyChanging("CarVin");
                    _carVin = value;
                    NotifyPropertyChanged("CarVin");
                }
            }
        }
        private string _carInsurance;

        [Column]

        public string CarInsurance
        {
            get { return _carInsurance; }
            set
            {
                if (_carInsurance != value)
                {
                    NotifyPropertyChanging("CarInsurance");
                    _carInsurance = value;
                    NotifyPropertyChanged("CarInsurance");
                }
            }
        }

        private DateTime _carInsExp;
        [Column]

        public DateTime CarInsExpiry
        {
            get { return _carInsExp; }
            set
            {
                if (_carInsExp != value)
                {
                    NotifyPropertyChanging("CarInsExpiry");
                    _carInsExp = value;
                    NotifyPropertyChanged("CarInsExpiry");
                }
            }
        }

        private DateTime _carRegExp;
        [Column]

        public DateTime CarRegExpiry
        {
            get { return _carRegExp; }
            set
            {
                if (_carRegExp != value)
                {
                    NotifyPropertyChanging("CarRegExpiry");
                    _carRegExp = value;
                    NotifyPropertyChanged("CarRegExpiry");
                }
            }
        }
       
        #region INotifyPropertyChanging Members

        public event PropertyChangingEventHandler PropertyChanging;

        // Used to notify that a property is about to change
        private void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify that a property changed
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
#endregion

    #region Table and class for fuel info
    [Table]

    public class fuelInfo : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private int _fuelID;
         [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int FuelID
        {
            get { return _fuelID; }
            set
            {
                if (_fuelID != value)
                {
                    NotifyPropertyChanging("FuelID");
                    _fuelID = value;
                    NotifyPropertyChanged("FuelID");
                }
            }
        }


        private int _carID;
        [Column]
        public int CarID
        {
            get { return _carID; }
            set
            {
                if (_carID != value)
                {
                    NotifyPropertyChanging("CarID");
                    _carID = value;
                    NotifyPropertyChanged("CarID");
                }
            }
        }

        private DateTime _date;
        [Column]
        public DateTime Date
        {
            get { return _date; }
            set
            {
                if (_date != value)
                {
                    NotifyPropertyChanging("Date");
                    _date = value;
                    NotifyPropertyChanged("Date");
                }
            }
        }

        private int _miles;
        [Column]
        public int Miles
        {
            get { return _miles; }
            set
            {
                if (_miles != value)
                {
                    NotifyPropertyChanging("Miles");
                    _miles = value;
                    NotifyPropertyChanged("Miles");
                }
            }
        }
        private float _filled;

        [Column]
        public float Filled
        {
            get { return _filled; }
            set
            {
                if (_filled != value)
                {
                    NotifyPropertyChanging("Filled");
                    _filled = value;
                    NotifyPropertyChanged("Filled");
                }
            }
        }
        private float _cost;
         [Column]
        public float Cost
        {
            get { return _cost; }
            set
            {
                if (_cost != value)
                {
                    NotifyPropertyChanging("Cost");
                    _cost = value;
                    NotifyPropertyChanged("Cost");
                }
            }
        }

         private string _dateStr;

         [Column]
         public string DateStr
         {
             get { return _dateStr; }
             set
             {
                 if (_dateStr != value)
                 {
                     NotifyPropertyChanging("DateStr");
                     _dateStr = value;
                     NotifyPropertyChanged("DateStr");
                 }
             }
         }

         private double _mileage;
         [Column]
         public double Mileage
         {
             get { return _mileage; }
             set
             {
                 if (_mileage != value)
                 {
                     NotifyPropertyChanging("Mileage");
                     _mileage = value;
                     NotifyPropertyChanged("Mileage");
                 }
             }
         }

         private float _distFromLast;
         [Column]
         public float DistanceFromLast
         {
             get { return _distFromLast; }
             set
             {
                 if (_distFromLast != value)
                 {
                     NotifyPropertyChanging("DistanceFromLast");
                     _distFromLast = value;
                     NotifyPropertyChanged("DistanceFromLast");
                 }
             }
         }

         #region INotifyPropertyChanging Members

         public event PropertyChangingEventHandler PropertyChanging;

         // Used to notify that a property is about to change
         private void NotifyPropertyChanging(string propertyName)
         {
             if (PropertyChanging != null)
             {
                 PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
             }
         }

         #endregion

         #region INotifyPropertyChanged Members

         public event PropertyChangedEventHandler PropertyChanged;

         // Used to notify that a property changed
         private void NotifyPropertyChanged(string propertyName)
         {
             if (PropertyChanged != null)
             {
                 PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
             }
         }

         #endregion

    }
#endregion

    #region Table and class for maint info
    [Table]
    public class maintInfo : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private int _maintID;
        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int MaintID
        {
            get { return _maintID; }
            set
            {
                if (_maintID != value)
                {
                    NotifyPropertyChanging("MaintID");
                    _maintID = value;
                    NotifyPropertyChanged("MaintID");
                }
            }
        }

        private int _carID;
        [Column]
        public int CarID
        {
            get { return _carID; }
            set
            {
                if (_carID != value)
                {
                    NotifyPropertyChanging("CarID");
                    _carID = value;
                    NotifyPropertyChanged("CarID");
                }
            }
        }

        private DateTime _date;
        [Column]
        public DateTime Date
        {
            get { return _date; }
            set
            {
                if (_date != value)
                {
                    NotifyPropertyChanging("Date");
                    _date = value;
                    NotifyPropertyChanged("Date");
                }
            }
        }

        private string _maintType;
        [Column]
        public string MaintType
        {
            get { return _maintType; }
            set
            {
                if (_maintType != value)
                {
                    NotifyPropertyChanging("MaintType");
                    _maintType = value;
                    NotifyPropertyChanged("MaintType");
                }
            }
        }
        private string _partName;
        [Column]
        public string PartName
        {
            get { return _partName; }
            set
            {
                if (_partName != value)
                {
                    NotifyPropertyChanging("PartName");
                    _partName = value;
                    NotifyPropertyChanged("PartName");
                }
            }
        }

        private string _partNumber;
        [Column]
        public string PartNumber
        {
            get { return _partNumber; }
            set
            {
                if (_partNumber != value)
                {
                    NotifyPropertyChanging("PartNumber");
                    _partNumber = value;
                    NotifyPropertyChanged("PartNumber");
                }
            }
        }

        private float _partCost;
        [Column]
        public float PartCost
        {
            get { return _partCost; }
            set
            {
                if (_partCost != value)
                {
                    NotifyPropertyChanging("PartCost");
                    _partCost = value;
                    NotifyPropertyChanged("PartCost");
                }
            }
        }

        private float _laborCharges;
        [Column]
        public float LaborCharges
        {
            get { return _laborCharges; }
            set
            {
                if (_laborCharges != value)
                {
                    NotifyPropertyChanging("LaborCharges");
                    _laborCharges = value;
                    NotifyPropertyChanged("LaborCharges");
                }
            }
        }

        private float _totalCost;
        [Column]
        public float TotalCost
        {
            get { return _totalCost; }
            set
            {
                if (_totalCost != value)
                {
                    NotifyPropertyChanging("TotalCost");
                    _totalCost = value;
                    NotifyPropertyChanged("TotalCost");
                }
            }
        }

        private int _miles;
        [Column]
        public int Miles
        {
            get { return _miles; }
            set
            {
                if (_miles != value)
                {
                    NotifyPropertyChanging("Miles");
                    _miles = value;
                    NotifyPropertyChanged("Miles");
                }
            }
        }

        private string _dateStr;

        [Column]
        public string DateStr
        {
            get { return _dateStr; }
            set
            {
                if (_dateStr != value)
                {
                    NotifyPropertyChanging("DateStr");
                    _dateStr = value;
                    NotifyPropertyChanged("DateStr");
                }
            }
        }

        #region INotifyPropertyChanging Members

        public event PropertyChangingEventHandler PropertyChanging;

        // Used to notify that a property is about to change
        private void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify that a property changed
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
 
    }

    #endregion

    #region Table for settings
    [Table]

    public class settingsInfo : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private int _settingID;
        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int SettingID
        {
            get { return _settingID; }
            set
            {
                if (_settingID != value)
                {
                    NotifyPropertyChanging("SettingID");
                    _settingID = value;
                    NotifyPropertyChanged("SettingID");
                }
            }
        }

        private string _settingPage;
        [Column]
        public string SettingPage
        {
            get { return _settingPage; }
            set
            {
                if (_settingPage != value)
                {
                    NotifyPropertyChanging("SettingPage");
                    _settingPage = value;
                    NotifyPropertyChanged("SettingPage");
                }
            }
        }

         private string _settingCurrency = "$";
        [Column]
         public string SettingCurrency
        {
            get { return _settingCurrency; }
            set
            {
                if (_settingCurrency != value)
                {
                    NotifyPropertyChanging("SettingCurrency");
                    _settingCurrency = value;
                    NotifyPropertyChanged("SettingCurrency");
                }
            }
        }

        private string _settingVolume = "gal";
        [Column]
        public string SettingVolume
        {
            get { return _settingVolume; }
            set
            {
                if (_settingVolume != value)
                {
                    NotifyPropertyChanging("SettingVolume");
                    _settingVolume = value;
                    NotifyPropertyChanged("SettingVolume");
                }
            }
        }

        private string _settingDistance = "miles";
        [Column]
        public string SettingDistance
        {
            get { return _settingDistance; }
            set
            {
                if (_settingDistance != value)
                {
                    NotifyPropertyChanging("SettingDistance");
                    _settingDistance = value;
                    NotifyPropertyChanged("SettingDistance");
                }
            }
        }

        private bool _settingMPG1;
        [Column]
        public bool SettingMPG1
        {
            get { return _settingMPG1; }
            set
            {
                if (_settingMPG1 != value)
                {
                    NotifyPropertyChanging("SettingMPG1");
                    _settingMPG1 = value;
                    NotifyPropertyChanged("SettingMPG1");
                }
            }
        }

        private bool _settingMPG2;
        [Column]
        public bool SettingMPG2
        {
            get { return _settingMPG2; }
            set
            {
                if (_settingMPG2 != value)
                {
                    NotifyPropertyChanging("SettingMPG2");
                    _settingMPG2 = value;
                    NotifyPropertyChanged("SettingMPG2");
                }
            }
        }
        #region INotifyPropertyChanging Members

        public event PropertyChangingEventHandler PropertyChanging;

        // Used to notify that a property is about to change
        private void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify that a property changed
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
	#endregion

    #region Table for reminder
    [Table]

    public class reminderInfo : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private int _reminderID;
        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int ReminderID
        {
            get { return _reminderID; }
            set
            {
                if (_reminderID != value)
                {
                    NotifyPropertyChanging("ReminderID");
                    _reminderID = value;
                    NotifyPropertyChanged("ReminderID");
                }
            }
        }

        private int _carID;
        [Column]
        public int CarID
        {
            get { return _carID; }
            set
            {
                if (_carID != value)
                {
                    NotifyPropertyChanging("CarID");
                    _carID = value;
                    NotifyPropertyChanged("CarID");
                }
            }
        }

        private DateTime _date;
        [Column]
        public DateTime Date
        {
            get { return _date; }
            set
            {
                if(_date != value)
                {
                     NotifyPropertyChanging("Date");
                    _date = value;
                    NotifyPropertyChanged("Date");
                }
                }
            }

        private int _dueMiles;
        [Column]
        public int DueMiles
        {
            get { return _dueMiles; }
            set
            {
                if(_dueMiles != value)
                {
                     NotifyPropertyChanging("DueMiles");
                    _dueMiles = value;
                    NotifyPropertyChanged("DueMiles");
                }
                
            }
        }
         private string _item;
        [Column]
        public string Item
        {
            get { return _item; }
            set
            {
                if(_item != value)
                {
                     NotifyPropertyChanging("Item");
                    _item = value;
                    NotifyPropertyChanged("Item");
                }
                
            }
        }
        private string _desc;
        [Column]
        public string Desc
        {
            get { return _desc; }
            set
            {
                if (_desc != value)
                {
                    NotifyPropertyChanging("Desc");
                    _desc = value;
                    NotifyPropertyChanged("Desc");
                }

            }
        }

        private string _dateStr;

        [Column]
        public string DateStr
        {
            get { return _dateStr; }
            set
            {
                if (_dateStr != value)
                {
                    NotifyPropertyChanging("DateStr");
                    _dateStr = value;
                    NotifyPropertyChanged("DateStr");
                }
            }
        }
        #region INotifyPropertyChanging Members

        public event PropertyChangingEventHandler PropertyChanging;

        // Used to notify that a property is about to change
        private void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify that a property changed
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

}
        
 
    
#endregion

}
