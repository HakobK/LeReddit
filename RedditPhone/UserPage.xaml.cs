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

        public TextBlock[] Posts;
        public int postSize = 100;
        public int postIndex = 1;

        public int yMargin = 0;

        public UserPage()
        {
            InitializeComponent();
            Comments = new TextBlock[CommentSize];
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            await getstuff();
            await loadPosts();
            await loadComments();        
            await getComments();
            await getPosts();
        }


        public async Task loadPosts()
        {
            var posts = await Task.Factory.StartNew(() => { return authentication.authenticatedReddit.User.Posts; });
            int text = await Task.Factory.StartNew(() => { return posts.Count(); });        
            PostsCount.Text = text.ToString();
        }

        public async Task loadComments()
        {
            var Comments = await Task.Factory.StartNew(() => { return authentication.authenticatedReddit.User.Comments; });
            int comment = await Task.Factory.StartNew(() => { return Comments.Count(); });
            CountComment.Text = comment.ToString();
        }

        public async Task getstuff()
        {
            var fullname = await Task.Factory.StartNew(() => { return authentication.authenticatedReddit.User.FullName; });
            txtUserPage.Text = fullname;

            var CommentKarma = await Task.Factory.StartNew(() => { return authentication.authenticatedReddit.User.CommentKarma; });
            commentKarma.Text = CommentKarma.ToString();

            var LinkKarma = await Task.Factory.StartNew(() => { return authentication.authenticatedReddit.User.LinkKarma; });
            linkKarma.Text = LinkKarma.ToString();

            var created = await Task.Factory.StartNew(() => { return authentication.authenticatedReddit.User.Created; });
            created1.Text = created.ToString();
        }

        public async Task getComments()
        {
            await Task.Factory.StartNew(() =>
            {
                IEnumerable<Comment> comment  = authentication.authenticatedReddit.User.Comments;
                foreach (Comment s in comment.Take(3))
                {
                    Dispatcher.BeginInvoke(() =>
                    {

                        TextBlock txt = new TextBlock();
                        txt.TextWrapping = TextWrapping.Wrap;
                        ListBox1.Items.Add(s.Body);
                        ListBox1.Items.Add(" ");
                        Comments[CommentIndex] = txt;
                        Comments[CommentIndex].Margin = new Thickness(0, yMargin, 0, 0);
                        CommentUser.Children.Add(Comments[CommentIndex]);
                        CommentIndex++;
                       
                    });
                }
                Dispatcher.BeginInvoke(() =>
                {
                });

            });
        }


        public async Task getPosts()
        {
            await Task.Factory.StartNew(() =>
            {
                IEnumerable<Post> post = authentication.authenticatedReddit.User.Posts;
                foreach (Post s in post.Take(3))
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        TextBlock txt = new TextBlock();
                        txt.TextWrapping = TextWrapping.Wrap;

                        if (s.Upvotes < 2)
                        {
                            ListBox2.Items.Add(s.Title + "  (" + s.Upvotes + " upvote)");
                        }
                        else
                        {
                            ListBox2.Items.Add(s.Title + "  (" + s.Upvotes + " upvotes)");
                        }

                        ListBox2.Items.Add(" ");

                        Comments[postIndex] = txt;
                        Comments[postIndex].Margin = new Thickness(0, yMargin, 0, 0);
                        CommentUser.Children.Add(Comments[postIndex]);
                        postIndex++;
                        yMargin = yMargin + 20;
                    });
                }
                Dispatcher.BeginInvoke(() =>
                {
                });

            });
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/InboxPMs.xaml?", UriKind.Relative));
        }

    }
}