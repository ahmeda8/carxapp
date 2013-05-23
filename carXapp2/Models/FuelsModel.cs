using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using carXapp2;

namespace carXapp2.Models
{
    class FuelsModel
    {
        private int carID;

        public FuelsModel(int carID)
        {
            this.carID = carID;
        }

        public ObservableCollection<FuelListItem> GetRecords()
        {
            var records = from fuelInfo f in App.ViewModel.Database.fuelInfo
                          where f.CarID == this.carID
                          orderby f.Date ascending
                          select f;
            ObservableCollection<FuelListItem> collection = new ObservableCollection<FuelListItem>();
            FuelListItem item;
            foreach (fuelInfo f in records)
            {
                item = new FuelListItem();
            }
            return collection;
        }
    }
}
