using System;
using System.Net;
using System.Windows;
using System.Linq;
using carXapp2;

namespace carXapp2.Models
{
    public class CarsModel
    {
        private int carID;

        public CarsModel(int carID)
        {
            this.carID = carID;
        }

        public carInfo GetCar()
        {
            var query = from carInfo c in App.ViewModel.Database.carInfo
                        where c.CarID == this.carID
                        select c;
            return query.First();
        }
    }
}
