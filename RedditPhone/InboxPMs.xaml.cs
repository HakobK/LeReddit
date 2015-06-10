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

namespace RedditPhone
{
    public partial class InboxPMs : PhoneApplicationPage
    {
        MainPage authentication = new MainPage();

        public TextBlock[] Messages;
        public int MessageSize = 100;
        public int MessageIndex = 1;

        public int yMargin = 0;

        public InboxPMs()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/SideMenu2.xaml?", UriKind.Relative));
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            var fullname = await Task.Factory.StartNew(() => { return authentication.authenticatedReddit.User.FullName; });
            userNamePM.Text = fullname;

            var Inbox = await Task.Factory.StartNew(() => { return authentication.authenticatedReddit.User.Inbox; });

            var privateMessages = await Task.Factory.StartNew(() => { return authentication.authenticatedReddit.User.PrivateMessages; });
            var privateMessage = await Task.Factory.StartNew(() => { return privateMessages.Count().ToString(); });
            countMessages.Text = privateMessage.ToString();

        }

        public async Task getInbox()    
        {
            await Task.Factory.StartNew(() =>
            {
                IEnumerable<PrivateMessage> privateMessage = authentication.authenticatedReddit.User.PrivateMessages;
                foreach (PrivateMessage s in privateMessage.Take(3))
                {
                    Dispatcher.BeginInvoke(() =>
                    {


                        TextBlock txt = new TextBlock();
                        lBox.Items.Add(s);
                        lBox.Items.Add(" ");

                        Messages[MessageIndex] = txt;
                        Messages[MessageIndex].Margin = new Thickness(0, yMargin, 0, 0);
                        MessageUser.Children.Add(Messages[MessageIndex]);
                        MessageIndex++;
                        yMargin = yMargin + 20;
                    });
                }
                Dispatcher.BeginInvoke(() =>
                {
                });

            });
        }
    }
}