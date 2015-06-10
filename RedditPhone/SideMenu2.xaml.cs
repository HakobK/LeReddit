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
    public partial class SideMenu2 : PhoneApplicationPage
    {
        MainPage authentication = new MainPage();
        public int logCheck;
        public TextBlock[] SubscribedSubreddits;
        public int SubSubSize = 100;
        public int SubSubIndex = 1;
        public int yMargin = 0;
        public string key;
        public SideMenu2()
        {
            InitializeComponent();
            SubscribedSubreddits = new TextBlock[SubSubSize];
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            logCheck = 0;

            if (NavigationContext.QueryString.ContainsKey("isloggedin"))
            {
                key = NavigationContext.QueryString["isloggedin"];
                logCheck = Convert.ToInt32(key);
                await filloutthings();
            }

            await disableButtons();
            
        }

        /// <summary>
        /// Button links to SubredditContent class
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/SubredditContent.xaml?subreddits=" + subredditTxt.Text, UriKind.Relative));
        }

        /// <summary>
        /// Button links to UserPage class
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/UserPage.xaml?", UriKind.Relative));
            NavigationService.Navigate(new Uri("/UserPage.xaml?", UriKind.Relative));
        }

        public async Task filloutthings()
        {
            await Task.Factory.StartNew(() =>
            {
                try
                {
                    IEnumerable<Subreddit> sub = authentication.authenticatedReddit.User.SubscribedSubreddits;
                    foreach (Subreddit s in sub)
                    {
                        Dispatcher.BeginInvoke(() =>
                        {
                            TextBlock txt = new TextBlock(); txt.Text = s.DisplayName;

                            SubscribedSubreddits[SubSubIndex] = txt;
                            SubscribedSubreddits[SubSubIndex].Margin = new Thickness(0, yMargin, 0, 0);
                            loggedSubSub.Children.Add(SubscribedSubreddits[SubSubIndex]);
                            SubSubIndex++;
                            yMargin = yMargin + 20;
                        });
                    }
                }
                catch(Exception err)
                {
                    
                }
            });
        }

        /// <summary>
        /// Button links to Authentication to login
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml?", UriKind.Relative));
        }

        private async Task disableButtons()
        {
            await Task.Factory.StartNew(() =>
            {
                try
                {
                    //LoggedInReddit.LogIn(user, pass);
                    if (logCheck == 1)
                    {
                        Dispatcher.BeginInvoke(() =>
                        {
                            //LogIn.Content = "Log Out";
                            LogIn.Opacity = 0;
                            LogIn.IsEnabled = false;
                            LogIn.Visibility = Visibility.Collapsed;
                            UserProfile.Opacity = 100;
                            UserProfile.IsEnabled = true;
                            UserProfile.Visibility = Visibility.Visible;
                            LogOut.Visibility = Visibility.Visible;
                            LogOut.IsEnabled = true;
                            LogOut.Opacity = 100;
                        }
                        );
                    }
                    else
                    {
                        Dispatcher.BeginInvoke(() =>
                        {
                            LogIn.Opacity = 100;
                            LogIn.IsEnabled = true;
                            LogIn.Visibility = Visibility.Visible;
                            UserProfile.Opacity = 0;
                            UserProfile.IsEnabled = false;
                            UserProfile.Visibility = Visibility.Collapsed;
                            LogOut.Visibility = Visibility.Collapsed;
                            LogOut.IsEnabled = false;
                            LogOut.Opacity = 0;
                        }
                        );

                    }
                }
                catch(Exception exc)
                {
                    MessageBox.Show("Failed" + exc);
                }

            });
        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            authentication.authenticatedReddit = new Reddit();
            authentication.loggedIn = 0;
            logCheck = 0;
            NavigationService.Navigate(new Uri("/SubredditContent.xaml?", UriKind.Relative));
        }
    }
}
