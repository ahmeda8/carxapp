using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using carXapp2.classes;
using Microsoft.Phone.Marketplace;

namespace carXapp2
{

    public class carsViewModel : INotifyPropertyChanged
    {
        public carDataContext carDataDb;
        private static LicenseInformation _licenseInfo = new LicenseInformation();
        public bool isTrial = false;

        public carsViewModel(string DbConnection)
        {
            carDataDb = new carDataContext(DbConnection);

        }

        

        private ObservableCollection<carInfo> _cars;

        public ObservableCollection<carInfo> Cars
        {
            get { return _cars; }
            set 
            {
                _cars = value;
                NotifyPropertyChanged("Cars");

            }
        }

        private ObservableCollection<fuelInfo> _fuel;

        public ObservableCollection<fuelInfo> Fuel
        {
            get { return _fuel; }
            set
            {
                _fuel = value;
                NotifyPropertyChanged("Fuel");

            }
        }

        private ObservableCollection<reminderInfo> _reminder;

        public ObservableCollection<reminderInfo> Reminder
        {
            get { return _reminder; }
            set
            {
                _reminder = value;
                NotifyPropertyChanged("Reminder");

            }
        }

        private reminderInfo _singleReminder;

        public reminderInfo SingleReminder
        {
            get { return _singleReminder; }
            set
            {
                _singleReminder = value;
                NotifyPropertyChanged("SingleReminder");
            }
        }

        private carInfo _singleCarRecord;

        public carInfo SingleCarRecord
        {
            get { return _singleCarRecord; }
            set
            {
                _singleCarRecord = value;
                NotifyPropertyChanged("SingleCarRecord");
            }
        }

        private ObservableCollection<settingsInfo> _settings;

        public ObservableCollection<settingsInfo> SetInfo
        {
            get { return _settings; }
            set
            {
                _settings = value;
                NotifyPropertyChanged("SetInfo");
            }
        }

        private ObservableCollection<maintInfo> _maint;
        public ObservableCollection<maintInfo> Maint
        {
            get { return _maint; }
            set
            {
                _maint = value;
                NotifyPropertyChanged("Maint");
            }
        }

        private maintInfo _singleMaintRecord;
        public maintInfo SingleMaintRecord
        {
            get { return _singleMaintRecord; }
            set
            {
                _singleMaintRecord = value;
                NotifyPropertyChanged("SingleMaintRecord");
            }
        }


        public void LoadCollectionFromDB()
        {
            var carsInDb = from carInfo cInfo in carDataDb.carInfo
                           select cInfo;
            Cars = new ObservableCollection<carInfo>(carsInDb);
            var settingDB = from settingsInfo data in carDataDb.settingsInfo
                            select data;
            SetInfo = new ObservableCollection<settingsInfo>(settingDB);
            SingleCarRecord = new carInfo();
            
        }

        public void Addcar(carInfo newCar)
        {
            carDataDb.carInfo.InsertOnSubmit(newCar);
            carDataDb.SubmitChanges();
            //Cars.Add(newCar);
        }

        public void AddMaint(maintInfo newMaint)
        {
            carDataDb.maintInfo.InsertOnSubmit(newMaint);
            carDataDb.SubmitChanges();
            Maint.Add(newMaint);
        }


        public void UpdateCarInfo(int id)
        {
            var datatoupdate = from item in carDataDb.carInfo
                               where item.CarID == id
                               select item;
             
        }

        
        public void DeleteCar(carInfo toDelete)
        {
            carDataDb.carInfo.DeleteOnSubmit(toDelete);
            Cars.Remove(toDelete);
            carDataDb.SubmitChanges();
        }

        public void DeleteAllFuel(int carID)
        {
            IEnumerable<fuelInfo> deleteFuels = from fuelInfo fuels in carDataDb.fuelInfo
                                                where fuels.CarID == carID
                                                select fuels;
            carDataDb.fuelInfo.DeleteAllOnSubmit(deleteFuels);
            carDataDb.SubmitChanges();
           
        }

        public void DeleteMaint(maintInfo toDelete)
        {
            carDataDb.maintInfo.DeleteOnSubmit(toDelete);
            Maint.Remove(toDelete);
            carDataDb.SubmitChanges();
        }

        public void DeleteAllMaint(int carID)
        {
            IEnumerable<maintInfo> deleteMaint = from maintInfo maint in carDataDb.maintInfo
                                                where maint.CarID == carID
                                                select maint;
            carDataDb.maintInfo.DeleteAllOnSubmit(deleteMaint);
            carDataDb.SubmitChanges();
           
 
        }
        public void AddReminder(reminderInfo newInfo)
        {
            carDataDb.reminderInfo.InsertOnSubmit(newInfo);
            
            carDataDb.SubmitChanges();
            Reminder.Add(newInfo);
        }

        public void DeleteReminder(reminderInfo toDelete)
        {
            carDataDb.reminderInfo.DeleteOnSubmit(toDelete);
            Reminder.Remove(toDelete);
            carDataDb.SubmitChanges();
        }

        public void DeleteAllReminder(int carID)
        {
            IEnumerable<reminderInfo> deleteReminder = from reminderInfo data in carDataDb.reminderInfo
                                                 where data.CarID == carID
                                                 select data;
            carDataDb.reminderInfo.DeleteAllOnSubmit(deleteReminder);
            carDataDb.SubmitChanges();


        }

        public void SaveChangesToDB()
        {
            carDataDb.SubmitChanges();
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify Silverlight that a property has changed.
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
 
    }
}