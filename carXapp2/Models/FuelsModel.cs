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
        public int totalMiles {get;set;}
        public float totalFuel { get; set; }
        public float totalCost { get; set; }
        private TimeSpan TotalTimeSpan;


        public FuelsModel(int carID)
        {
            this.carID = carID;
            totalMiles = 0;
            totalFuel = 0f;
            totalCost = 0f;
            TotalTimeSpan = new TimeSpan(1,0,0,0);
        }

        public ObservableCollection<FuelListItem> GetRecords()
        {
            var records = from fuelInfo f in App.ViewModel.Database.fuelInfo
                          where f.CarID == this.carID
                          orderby f.Date descending
                          
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
                    item.MilesDriven = recordList[i].Miles - recordList[i + 1].Miles;
                    item.MPG = (float)Math.Round((double)(item.MilesDriven/recordList[i].Filled),2);
                }
                catch (Exception e)
                {
                    item.MilesDriven = 0;//recordList[i].Miles;
                    item.MPG = 0f;
                }

                this.totalCost += item.Cost;
                this.totalFuel += item.Fill;
                this.totalMiles += item.MilesDriven;
                collection.Add(item);
            }
            if(recordList.Count > 0)
                TotalTimeSpan = recordList.First().Date - recordList.Last().Date;
            return collection ;
        }

        public float OverallFuelConsumption()
        {
            return (float)totalMiles / totalFuel;
        }

        public TimeSpan GetTotalTimeSpan()
        {
            if (this.TotalTimeSpan.Days > 0)
                return this.TotalTimeSpan;
            else
                return new TimeSpan(1, 0, 0, 0);
        }
    }
}
