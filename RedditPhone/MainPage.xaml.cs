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

namespace RedditPhone
{

    public partial class MainPage : PhoneApplicationPage
    {
        //This object will be used everywhere if logged in.
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

        /// <summary>
        /// If logged in already will message you.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            if (Statics.loggedIn)
            {
                Dispatcher.BeginInvoke(()=>MessageBox.Show("Already logged in,"));
            }
        }

        /// <summary>
        /// Do the login method and pass the filled in information.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void lgnReddit_Click(object sender, RoutedEventArgs e)
        {
            await login(txtUsername.Text, txtPassword.Password);
        }

        private async void messageLoggedIn()
        {
            await Task.Factory.StartNew(() =>
                {
                    Dispatcher.BeginInvoke(()=> MessageBox.Show("Logged in Succesfully!"));
                });
        }

        /// <summary>
        /// Method used to login. Sets the authenticatedreddit Reddit object to the logged in reddit.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        private async Task login(string user, string pass)
        {
            await Task.Factory.StartNew(() =>
            {
                try
                {
                    authenticatedReddit.LogIn(user,pass);
                    messageLoggedIn();
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

        /// <summary>
        /// Go to menu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
                NavigationService.Navigate(new Uri("/SideMenu2.xaml?", UriKind.Relative));
        }

        /// <summary>
        /// Go to register account.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBlock_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/RegisterAccount.xaml?", UriKind.Relative));

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