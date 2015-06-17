﻿using System;
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
    public partial class InboxPMs : PhoneApplicationPage
    {
        MainPage authentication = new MainPage();

        public TextBlock[] Messages;
        public int MessageSize = 100;
        public int MessageIndex = 1;
        public string s;
        public int yMargin = 40;
        public Grid[] gridCollection;

        public InboxPMs()
        {
            gridCollection = new Grid[1000];
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/SideMenu2.xaml?", UriKind.Relative));
        }

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

        public async Task getInbox()    
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