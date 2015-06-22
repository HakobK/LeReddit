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
using System.Threading.Tasks;
using RedditSharpPCL;
using System.Windows.Media;

namespace RedditPhone
{
    /// <summary>
    /// Class to handle the Inbox and PrivateMessaging of the App.
    /// </summary>
    public partial class InboxPMs : PhoneApplicationPage
    {
        // Constructor; Mainpage to check for authentication.
        MainPage authentication = new MainPage();

        private TextBlock[] Messages;
        private int MessageSize = 100;
        private int MessageIndex = 1;
        private string s;
        private int yMargin = 40;
        private Grid[] gridCollection;

        public InboxPMs()
        {
            gridCollection = new Grid[1000];
            InitializeComponent();
        }

        /// <summary>
        /// Method to navigate to the SideMenu. Says 0 references, actually works...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/SideMenu2.xaml?", UriKind.Relative));
        }

        /// <summary>
        /// Method called onto when navigated to this class.
        /// </summary>
        /// <param name="e"></param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            var fullname = await Task.Factory.StartNew(() => { return authentication.authenticatedReddit.User.FullName; });
            userNamePM.Text = "Name: " + fullname;
            var Inbox = await Task.Factory.StartNew(() => { return authentication.authenticatedReddit.User.Inbox; });
            var privateMessages = await Task.Factory.StartNew(() => { return authentication.authenticatedReddit.User.PrivateMessages; });
            var privateMessage = await Task.Factory.StartNew(() => { return privateMessages.Count().ToString(); });
            countMessages.Text = "Messages: " + privateMessage.ToString();
            await getInbox();

        }

        /// <summary>
        /// Method to get all the send and received Private Messages when Logged in/Authenticated.
        /// </summary>
        /// <returns></returns>
        private async Task getInbox()    
        {
            await Task.Factory.StartNew(() =>
            {
                IEnumerable<PrivateMessage> privateMessage = authentication.authenticatedReddit.User.PrivateMessages;
                foreach (PrivateMessage f in privateMessage.Take(10))
                {
                    Dispatcher.BeginInvoke(() =>
                    {

                        var commentsGrid = new Grid();

                        commentsGrid.Height = 150;
                        commentsGrid.Width = 445;
                        commentsGrid.VerticalAlignment = VerticalAlignment.Top;
                        commentsGrid.Margin = new Thickness(0, yMargin, 0, 0);
                        SolidColorBrush myBrush = new SolidColorBrush(Color.FromArgb(255, 35, 35, 35));
                        commentsGrid.Background = myBrush;

                        TextBlock txtpm = new TextBlock();
                        //txtpm.Margin = new Thickness(0, 0, 5, 0);
                        txtpm.TextWrapping = TextWrapping.Wrap;
                        txtpm.Text = "Author: " + f.Author + "\n" + "Subject: " + f.Subject + "\n" + "Message: " + f.Body;

                        gridCollection[MessageIndex] = commentsGrid;
                        commentsGrid.Children.Add(txtpm);


                        pmGrid.Children.Add(commentsGrid);
                        yMargin = yMargin + 160;
                    });
                    MessageIndex++;
                }
            });
        }
    }
}