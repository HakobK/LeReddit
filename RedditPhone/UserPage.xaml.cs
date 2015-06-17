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
        public int CommentIndex = 1;
        public int postIndex = 1;
        public IEnumerable<Comment> comments;
        public IEnumerable<Post> posts;

        public UserPage()
        {
            InitializeComponent();     
        }

        /// <summary>
        /// Get the profile information and display this
        /// </summary>
        /// <param name="e"></param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {         
            // fill textboxes with returned value
            txtUserPage.Text = await Task.Factory.StartNew(() => { return authentication.authenticatedReddit.User.FullName; });
            commentKarma.Text = await Task.Factory.StartNew(() => { return authentication.authenticatedReddit.User.CommentKarma.ToString(); });
            linkKarma.Text = await Task.Factory.StartNew(() => { return authentication.authenticatedReddit.User.LinkKarma.ToString(); });
            created1.Text = await Task.Factory.StartNew(() => { return authentication.authenticatedReddit.User.Created.ToString(); });        
            PostsCount.Text = await Task.Factory.StartNew(() => { return authentication.authenticatedReddit.User.Posts.Count().ToString();});
            CountComment.Text = await Task.Factory.StartNew(() => { return authentication.authenticatedReddit.User.Comments.Count().ToString(); });

            await getComments();
            await getPosts();
        }

       
        /// <summary>
        /// Method to get all the comments from the user
        /// </summary>
        /// <returns></returns>
        public async Task getComments()
        {            
            await Task.Factory.StartNew(() =>
            {            
                comments = authentication.authenticatedReddit.User.Comments;
                foreach (Comment commentText in comments.Take(3))
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        // display the comments good
                        if (commentText.Upvotes < 2)
                        {
                            ListBox1.Items.Add(commentText.Body + "  (" + commentText.Upvotes + " upvote)");
                        }
                        else
                        {
                            ListBox1.Items.Add(commentText.Body + "  (" + commentText.Upvotes + " upvotes)");
                        }                   
                        ListBox1.Items.Add(" ");
                        CommentIndex++;
                    });
                }
            });                          
        }

        /// <summary>
        /// Method to get al the posts from the user with amount of upvotes
        /// </summary>
        /// <returns></returns>
       public async Task getPosts()
       {
           await Task.Factory.StartNew(() =>
            {
                posts = authentication.authenticatedReddit.User.Posts;
                foreach (Post s in posts.Take(3))
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        // display the posts good                   
                        if (s.Upvotes < 2)
                        {
                            ListBox2.Items.Add(s.Title + "  (" + s.Upvotes + " upvote)");
                        }
                        else
                        {
                            ListBox2.Items.Add(s.Title + "  (" + s.Upvotes + " upvotes)");
                        }
                        ListBox2.Items.Add(" ");
                        postIndex++;
                    });
                }
            });
       }

        /// <summary>
        /// Button to navigate to inboxPM
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/InboxPMs.xaml?", UriKind.Relative));
        }

    }
}