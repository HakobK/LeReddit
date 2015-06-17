using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using RedditPhone.Resources;
using Newtonsoft;
using RedditSharpPCL;
using System.Threading.Tasks;
using System.IO.IsolatedStorage;

namespace RedditPhone
{

    public partial class MainPage : PhoneApplicationPage
    {
        public Reddit authenticatedReddit;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            authenticatedReddit = new Reddit();
            //sAdControl.ErrorOccurred += new EventHandler<Microsoft.Advertising.AdErrorEventArgs>(sAdControl_ErrorOccurred);
            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            if (NavigationContext.QueryString.ContainsKey("key"))
            {
                string message = NavigationContext.QueryString["key"];
                if(message == "false")
                {
                    MessageBox.Show("Invalid login");
                }
            }
        }

        private async void lgnReddit_Click(object sender, RoutedEventArgs e)
        {
            await login2(txtUsername.Text, txtPassword.Password);
        }

        public async void dostuff()
        {
            await Task.Factory.StartNew(() =>
                {
                    Dispatcher.BeginInvoke(()=> MessageBox.Show("Logged in Succesfully!"));
                });
        }
        public async Task login2(string user, string pass)
        {


            await Task.Factory.StartNew(() =>
            {
                //LoggedInReddit.LogIn(user, pass);

                try
                {
                    authenticatedReddit.LogIn(user,pass);
                    dostuff();
                    Statics.loggedIn = true;
                    Dispatcher.BeginInvoke(() =>
                    {
                        NavigationService.Navigate(new Uri("/SubredditContent.xaml?", UriKind.Relative));
                    });
                }
                catch (Exception)
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        MessageBox.Show("Login failed, try again.");
                    });
                }
            });

        }

    

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Statics.loggedIn)
            {
                NavigationService.Navigate(new Uri("/SideMenu2.xaml?", UriKind.Relative));
            }
            else
            {
                NavigationService.Navigate(new Uri("/SideMenu2.xaml?", UriKind.Relative));
            }

        }

        private void TextBlock_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/RegisterAccount.xaml?", UriKind.Relative));

        }

        private void TextBlock_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                MessageBox.Show("Then write it down you idiot.");
            });
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}