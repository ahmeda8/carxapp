using System;
using System.Net;
using System.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

using carXapp2;
namespace carXapp2.classes
{
    public class Settings
    {
        public Settings()
        {
            var settingsIndb = from settingsInfo data in App.ViewModel.Database.settingsInfo
                               select data;
            var SetInfo = new ObservableCollection<settingsInfo>(settingsIndb);
            CurrencyString = SetInfo[0].SettingCurrency;
            if (SetInfo[0].SettingMPG1)
                MPGMethod = 1;
            else
                MPGMethod = 2;
            VolumeString = SetInfo[0].SettingVolume;
            DistanceString = SetInfo[0].SettingDistance;
            MileageString = SetInfo[0].SettingDistance + "/" + SetInfo[0].SettingVolume;
        }
        private static string curString = "$";

        public static string CurrencyString
        {
            get { return curString; }
            set { curString = value; }
        }

        private static string volumeString = "gal";
        public static string VolumeString
        {
            get { return volumeString; }
            set { volumeString = value; }
        }

        private static string distanecString = "Miles";
        public static string DistanceString
        {
            get { return distanecString; }
            set { distanecString = value; }
        }
        private static string _mileageString = "Miles/gal";
        public static string MileageString
        {
            get { return _mileageString; }
            set { _mileageString = value; }
        }
        private static int mpgMethod=1;
        public static int MPGMethod
        {
            get { return mpgMethod; }
            set { mpgMethod = value; }
        }

    }
}
