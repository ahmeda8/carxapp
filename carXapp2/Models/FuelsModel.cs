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
            List<fuelInfo> recordList = records.ToList<fuelInfo>();
            for (int i = 0; i < recordList.Count; i++)
            {
                item = new FuelListItem();
                item.Cost = recordList[i].Cost;
                item.Date = recordList[i].Date.ToShortDateString();
                item.Fill = recordList[i].Filled;
                item.FuelID = recordList[i].FuelID;
                try
                {
                    item.MilesDriven = recordList[i].Miles - recordList[i - 1].Miles;
                    item.MPG = recordList[i].Filled / item.MilesDriven;
                }
                catch (Exception e)
                {
                    item.MilesDriven = recordList[i].Miles;
                    item.MPG = 0f;
                }

                collection.Add(item);
            }
            return collection;
        }
    }
}
