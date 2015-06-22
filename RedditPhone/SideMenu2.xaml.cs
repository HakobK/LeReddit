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
using System.Windows.Input;


namespace RedditPhone
{
    /// <summary>
    /// The class for the Sidemenu of the App.
    /// </summary>
    public partial class SideMenu2 : PhoneApplicationPage
    {
        // Constructor; Mainpage to check Authentication; yMargin to align everything underneath eachother.
        MainPage authentication = new MainPage();
        private TextBlock[] SubscribedSubreddits;
        private IEnumerable<Subreddit> subscribedSubs;
        private int SubSubSize = 100;
        private int SubSubIndex = 1;
        private int yMargin = 0;
        public SideMenu2()
        {
            InitializeComponent();
            SubscribedSubreddits = new TextBlock[SubSubSize];
        }

        /// <summary>
        /// Method activates when this class is Navigated to.
        /// </summary>
        /// <param name="e"></param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {

            if (Statics.loggedIn)
            {
                await disableButtons();
                await filloutthings();
            }
            else 
            { 
                await disableButtons(); 
            }
        }

        /// <summary>
        /// Button links to SubredditContent class.
        /// Text field filled in as an example.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (subredditTxt.Text == "" || subredditTxt.Text == "e.g. 'microsoft'")
            {
                Dispatcher.BeginInvoke(() =>
                {
                    MessageBox.Show("Please fill in subreddit.");

                });
            }
            else
            {
                NavigationService.Navigate(new Uri("/SubredditContent.xaml?subreddits=" + subredditTxt.Text, UriKind.Relative));
            }
        }

        /// <summary>
        /// Method to navigate to SubredditContent with clicked subscribed subreddit when logged in.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void goToSub(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/SubredditContent.xaml?subreddits=" + (sender as TextBlock).Tag as string, UriKind.Relative));
        }

        /// <summary>
        /// Button to navigate User profile page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/UserPage.xaml?", UriKind.Relative));
            NavigationService.Navigate(new Uri("/UserPage.xaml?", UriKind.Relative));
        }

        /// <summary>
        /// Method to show the Subreddits a user is subscribed to, only shows when user is logged in.
        /// </summary>
        /// <returns></returns>
        private async Task filloutthings()
        {
            await Task.Factory.StartNew(() =>
            {
                    subscribedSubs = authentication.authenticatedReddit.User.SubscribedSubreddits;
                    foreach (Subreddit s in subscribedSubs)
                    {
                        Dispatcher.BeginInvoke(() =>
                        {
                            TextBlock txt = new TextBlock(); 
                            txt.Text = s.DisplayName;
                            txt.Tag = s.DisplayName;
                            txt.Tap += new EventHandler<GestureEventArgs>(goToSub);
                            
                            SubscribedSubreddits[SubSubIndex] = txt;
                            SubscribedSubreddits[SubSubIndex].Margin = new Thickness(0, yMargin, 0, 0);
                            loggedSubSub.Children.Add(SubscribedSubreddits[SubSubIndex]);
                            SubSubIndex++;
                            yMargin = yMargin + 25;
                            namding.Visibility = System.Windows.Visibility.Visible;
                        });
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

        /// <summary>
        /// Method to change and disable buttons based on if the the user has logged in or not.
        /// logCheck 1 means user is logged in.
        /// </summary>
        /// <returns></returns>
        private async Task disableButtons()
        {
            await Task.Factory.StartNew(() =>
            {
                try
                {
                    //LoggedInReddit.LogIn(user, pass);
                    if (Statics.loggedIn)
                    {
                        Dispatcher.BeginInvoke(() =>
                        {
                            LogIn.IsEnabled = false;
                            LogIn.Visibility = Visibility.Collapsed;
                            UserProfile.IsEnabled = true;
                            UserProfile.Visibility = Visibility.Visible;
                            LogOut.Visibility = Visibility.Visible;
                            LogOut.IsEnabled = true;
                        }
                        );
                    }
                    else
                    {
                        Dispatcher.BeginInvoke(() =>
                        {
                            LogIn.IsEnabled = true;
                            LogIn.Visibility = Visibility.Visible;
                            UserProfile.IsEnabled = false;
                            UserProfile.Visibility = Visibility.Collapsed;
                            LogOut.Visibility = Visibility.Collapsed;
                            LogOut.IsEnabled = false;
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

        /// <summary>
        /// Method to log out the user. Current bug: User logs out, button does not change into "Log In"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            authentication.authenticatedReddit = new Reddit();
            Statics.loggedIn = false;
            NavigationService.Navigate(new Uri("/SubredditContent.xaml?", UriKind.Relative));
        }

        /// <summary>
        ///  Method to nagivate to the SubredditContent
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void goToHomepage_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/SubredditContent.xaml?", UriKind.Relative));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void subredditTxt_Tap(object sender, GestureEventArgs e)
        {
            subredditTxt.Text = "";
        }
    }
}
