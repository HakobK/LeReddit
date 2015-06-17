using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Threading.Tasks;
using RedditPCL;
using RedditSharpPCL;

namespace RedditPhone
{
    public partial class RegisterAccount : PhoneApplicationPage
    {
        public Reddit registerer;

        public RegisterAccount()
        {
            registerer = new Reddit();
            InitializeComponent();
        }

        public async Task register(string user, string pass, string mail)
        {


            await Task.Factory.StartNew(() =>
            {
                //LoggedInReddit.LogIn(user, pass);
                try
                {
                    registerer.RegisterAccount(user, pass);
                    
                    
                    Dispatcher.BeginInvoke(() =>
                    {
                        MessageBox.Show("Succesfully registered account: " + user);
                    });

                }
                catch (Exception e)
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        MessageBox.Show(e.ToString());
                    });
                }
            });

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private async void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            await register(Username.Text,Password.Password,Email.Text);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml?", UriKind.Relative));
        }

        private void btnMenu_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/SideMenu2.xaml?", UriKind.Relative));

        }
    }
}