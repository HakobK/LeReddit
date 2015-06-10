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
        public int CommentSize = 100;
        public int CommentIndex = 1;
        public int yMargin = 0;

        public UserPage()
        {
            InitializeComponent();
            Comments = new TextBlock[CommentSize];
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

            var Comments = await Task.Factory.StartNew(() => { return authentication.authenticatedReddit.User.Comments; });
            var comment = await Task.Factory.StartNew(() => { return Comments.Count().ToString(); });
            CountComment.Text = comment.ToString();


            await DoThings();




        }

        public async Task DoThings()
        {
            await Task.Factory.StartNew(() =>
            {
                IEnumerable<Comment> sub = authentication.authenticatedReddit.User.Comments;
                foreach (Comment s in sub.Take(3))
                {
                    Dispatcher.BeginInvoke(() =>
                    {


                        TextBlock txt = new TextBlock();
                        ListBox1.Items.Add(s.Body);
                        ListBox1.Items.Add(" ");

                        Comments[CommentIndex] = txt;
                        Comments[CommentIndex].Margin = new Thickness(0, yMargin, 0, 0);
                        CommentUser.Children.Add(Comments[CommentIndex]);
                        CommentIndex++;
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