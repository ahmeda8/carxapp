using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Shell;
using System.Windows.Controls.Primitives;
using Microsoft.Phone.Controls;


namespace carXapp2
{
    public class LoadingScreen : ContentControl
    {
        private Rectangle backRect;
        private TextBlock textStatus;
        private ProgressBar progressBar;
        private StackPanel stackpanel;

        private bool currentSystemTrayState;
        private static string defaultText = "Loading...";
        


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.backRect = this.GetTemplateChild("backRect") as Rectangle;
            this.stackpanel = this.GetTemplateChild("stackpanel") as StackPanel;
            this.progressBar = this.GetTemplateChild("progressBar") as ProgressBar;
            this.textStatus = this.GetTemplateChild("textStatus") as TextBlock;

            //this.Text = labeltext;

            InitializeProgressType();
        }

        internal Popup ChildWindowPopup
        {
            get;
            private set;
        }

        private static PhoneApplicationFrame RootVisual
        {
            get
            {
                return Application.Current == null ? null : Application.Current.RootVisual as PhoneApplicationFrame;
            }
        }

        public ProgressBar ProgressBar
        {
            get
            {
                return this.progressBar;
            }
        }

        public new double Opacity
        {
            get
            {
                return this.backRect.Opacity;
            }
            set
            {
                this.backRect.Opacity = value;
            }
        }

        public void Hide()
        {
            // Restore system tray
            SystemTray.IsVisible = currentSystemTrayState;
            this.progressBar.IsIndeterminate = true;
            this.ChildWindowPopup.IsOpen = false;

        }

        public void Show()
        {
            if (this.ChildWindowPopup == null)
            {
                this.ChildWindowPopup = new Popup();
               

                try
                {
                    this.ChildWindowPopup.Child = this;
                }
                catch (ArgumentException)
                {
                    throw new InvalidOperationException("The control is already shown.");
                }
            }


            if (this.ChildWindowPopup != null && Application.Current.RootVisual != null)
            {
                // Configure accordingly to the type
                InitializeProgressType();

                // Show popup
                this.ChildWindowPopup.IsOpen = true;
            }
        }


        private void HideSystemTray()
        {
            // Capture current state of the system tray
            this.currentSystemTrayState = SystemTray.IsVisible;
            // Hide it
            SystemTray.IsVisible = false;
        }

        private void InitializeProgressType()
        {
            this.HideSystemTray();
            if (this.progressBar == null)
                return;

            this.Opacity = 0.7;
            this.backRect.Visibility = System.Windows.Visibility.Visible;
            this.stackpanel.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            this.progressBar.Foreground = (Brush)Application.Current.Resources["PhoneForegroundBrush"];
            this.textStatus.Text = defaultText;
            this.textStatus.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.textStatus.Visibility = System.Windows.Visibility.Visible;
            this.textStatus.Margin = new Thickness();
            this.Height = 800;
            this.progressBar.IsIndeterminate = true;
        }
    }
}
