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
        public int logCheck;
        public TextBlock[] SubscribedSubreddits;
        public IEnumerable<Subreddit> subscribedSubs;
        public int SubSubSize = 100;
        public int SubSubIndex = 1;
        public int yMargin = 0;
        public string key;
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
           // logCheck = authentication.loggedIn;
         //   await disableButtons();

          //  if (NavigationContext.QueryString.ContainsKey("isloggedin"))
          //  {
          //      //namding.Visibility = System.Windows.Visibility.Visible;
          //      key = NavigationContext.QueryString["isloggedin"];
          //      logCheck = Convert.ToInt32(key);
          //      await disableButtons();
          ////      await filloutthings();
          //  }

            if (Statics.loggedIn)
            {
                logCheck = 1;
                await disableButtons();
                await filloutthings();
            }
            else 
            { 
                logCheck = 0; 
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
        /// Method to navigate to SubredditContent
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void goToSub(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/SubredditContent.xaml?subreddits=" + (sender as TextBlock).Tag as string, UriKind.Relative));
        }

        /// <summary>
        /// Button to navigate UserPage class
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/UserPage.xaml?", UriKind.Relative));
            NavigationService.Navigate(new Uri("/UserPage.xaml?", UriKind.Relative));
        }

        /// <summary>
        /// Method to show the Subreddit a user is subscribed to, only shows when user is logged in.
        /// </summary>
        /// <returns></returns>
        public async Task filloutthings()
        {
            await Task.Factory.StartNew(() =>
            {
                //try
                //{

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
                //}
                
                //catch(Exception exc)
                //{
                //    MessageBox.Show("Failed" + exc);
                //}
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
        /// Method to change and disable buttons when the user has logged in.
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
                    if (logCheck == 1)
                    {
                        Dispatcher.BeginInvoke(() =>
                        {
                            //LogIn.Content = "Log Out";
                            //LogIn.Opacity = 0;
                            LogIn.IsEnabled = false;
                            LogIn.Visibility = Visibility.Collapsed;
                           // UserProfile.Opacity = 100;
                            UserProfile.IsEnabled = true;
                            UserProfile.Visibility = Visibility.Visible;
                            LogOut.Visibility = Visibility.Visible;
                            LogOut.IsEnabled = true;
                           // LogOut.Opacity = 100;
                        }
                        );
                    }
                    else
                    {
                        Dispatcher.BeginInvoke(() =>
                        {
                          //  LogIn.Opacity = 100;
                            LogIn.IsEnabled = true;
                            LogIn.Visibility = Visibility.Visible;
                         //   UserProfile.Opacity = 0;
                            UserProfile.IsEnabled = false;
                            UserProfile.Visibility = Visibility.Collapsed;
                            LogOut.Visibility = Visibility.Collapsed;
                            LogOut.IsEnabled = false;
                       //     LogOut.Opacity = 0;
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
        /// Method that handles a click action when a user clicks/touches a Subscribed Subreddit in the menu, to go to that Subreddit.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void subredditTxt_Tap(object sender, GestureEventArgs e)
        {
            subredditTxt.Text = "";
        }
    }
}
