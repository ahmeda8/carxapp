using System;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;

namespace carXapp2
{
    public partial class about : PhoneApplicationPage
    {
        public about()
        {
            InitializeComponent();
            string desc = "The next version of your all loving car expense tracking app.\n "+
                "Now with the features you had requested.\n"+
                "I would like to thank all the users that have provided me with their valueable feedback.";
            string features = "1. mileage statistics \n2. Car statistics \n4. fuel fillup records\n5. Maintenance records\n6. Total expense tracking\n7. More graphs and charts";
            Description.Text = desc;
            Features.Text = features;
        }

        private void Contact_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            EmailComposeTask email = new EmailComposeTask();
            email.To = "ahmed_abrar@live.com";
            email.Subject = "User Enquiry from car expense tracker";
            email.Show();
        }
    }
}