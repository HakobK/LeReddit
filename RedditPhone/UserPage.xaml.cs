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

namespace RedditPhone
{
    public partial class UserPage : PhoneApplicationPage
    {
        MainPage authentication = new MainPage();

         public TextBlock[] Comments;
        public int SubSubSize = 100;
        public int SubSubIndex = 1;
        public int yMargin = 0;

        public UserPage()
        {
            InitializeComponent();
            Comments = new TextBlock[SubSubSize];
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            
            var fullname = await Task.Factory.StartNew(() => { return authentication.authenticatedReddit.User.FullName; });
            txtUserPage.Text = fullname;

            var karma = await Task.Factory.StartNew(() => { return authentication.authenticatedReddit.User.CommentKarma; });
            amountKarma.Text = karma.ToString();

            var created = await Task.Factory.StartNew(() => { return authentication.authenticatedReddit.User.Created; });
            created1.Text = created.ToString();

            var posts = await Task.Factory.StartNew(() => { return authentication.authenticatedReddit.User.Posts; });
      
            var text = await Task.Factory.StartNew(() => { return posts.Count().ToString(); });
            Posts.Text = text.ToString();

            await DoThings();
          

            //if (NavigationContext.QueryString.ContainsKey("key"))
            //{
            //    string val = NavigationContext.QueryString["key"];
            //    txtUserPage.Text = "Welcome " + val;
            //}
            //if (NavigationContext.QueryString.ContainsKey("comments"))
            //{
            //    string listing = "";
            //    var val1 = NavigationContext.QueryString["comments"];
            //    foreach(object s in val1)
            //    {
            //        listing = listing + " " + s.ToString();
            //    }
            //    txtInfo.Text = "Total comments: " + listing;
            //}
            //if (NavigationContext.QueryString.ContainsKey("createdat"))
            //{
            //    string val1 = NavigationContext.QueryString["createdat"];

            //    createdat.Text = "User create date: " + val1;
            //}


            
        }

        public async Task DoThings()
        {
            await Task.Factory.StartNew(() =>
            {
                IEnumerable<Comment> sub = authentication.authenticatedReddit.User.Comments;
                foreach (Comment s in sub)
                {
                    Dispatcher.BeginInvoke(() =>
                    {

                        TextBlock txt = new TextBlock();

                        ListBox1.Items.Add(s.Body);

                        Comments[SubSubIndex] = txt;
                        Comments[SubSubIndex].Margin = new Thickness(0, yMargin, 0, 0);
                        CommentUser.Children.Add(Comments[SubSubIndex]);
                        SubSubIndex++;
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