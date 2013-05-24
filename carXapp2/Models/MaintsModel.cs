using System;
using System.Net;
using System.Windows;
using System.Linq;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using carXapp2;

namespace carXapp2.Models
{
    public class MaintsModel
    {
        private int carID;
        private float TotalPartsCost;
        private float TotalLaborCost;

        public MaintsModel(int carID)
        {
            this.carID = carID;
            TotalLaborCost = 0f;
            TotalPartsCost = 0f;
        }

        public ObservableCollection<MaintListItem> GetRecords()
        {
            var query = from maintInfo m in App.ViewModel.Database.maintInfo
                        where m.CarID == this.carID
                        orderby m.Date descending
                        select m;
            MaintListItem item;
            List<maintInfo> tempList = query.ToList<maintInfo>();
            ObservableCollection<MaintListItem> collection = new ObservableCollection<MaintListItem>();
            foreach (maintInfo t in tempList)
            {
                item = new MaintListItem();
                item.Cost = t.LaborCharges + t.PartCost;
                this.TotalLaborCost += t.LaborCharges;
                this.TotalPartsCost += t.PartCost;
                item.Date = t.Date.ToShortDateString();
                item.MaintID = t.MaintID;
                item.Partname = t.PartName;
                item.Partnumber = t.PartNumber;
                item.Mainttype = t.MaintType;
                collection.Add(item);
            }

            return collection;
        }

        public float GetTotalCost()
        {
            return this.TotalLaborCost + this.TotalPartsCost;
        }

    }
}
