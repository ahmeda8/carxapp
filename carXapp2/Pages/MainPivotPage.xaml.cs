﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace carXapp2.Pages
{
    public partial class MainPivotPage : PhoneApplicationPage
    {
        private int carID;

        public MainPivotPage()
        {
            InitializeComponent();
        }

        private void menuAddMaint_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/AddEditMaint.xaml?carID=" + carID, UriKind.Relative));
        }

        private void menuAddFuel_click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/AddEditFuel.xaml?cid=" + carID, UriKind.Relative));
        }
    }
}