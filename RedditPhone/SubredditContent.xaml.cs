using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using RedditSharpPCL;
using System.Threading;
using System.Threading.Tasks;
using System.Collections;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Ink;
using System.Windows.Input;


namespace RedditPhone
{
    public partial class SubredditContent : PhoneApplicationPage
    {
        MainPage authentication = new MainPage();

        private int verticalMargin = 25;
        private int objectSize = 1000;
        private int objectIndex = 0;

        private string THUMB_DEFAULT = "http://www.reddit.com/static/self_default2.png";
        private string thumb;
        private Grid[] gridCollection;
        private TextBlock[] tBlockCollection;
        private Image[] thumbnailCollection;
        private TextBlock[] upvotesCollection;
        private TextBlock[] voteUpCollection;
        private TextBlock[] voteDownCollection;
        private int postCount = 0;
        private IEnumerable<Post> pagePosts;

        public SubredditContent()
        {
            InitializeComponent();
            gridCollection = new Grid[objectSize];
            tBlockCollection = new TextBlock[objectSize];
            thumbnailCollection = new Image[objectSize];
            upvotesCollection = new TextBlock[objectSize];
            voteDownCollection = new TextBlock[objectSize];
            voteUpCollection = new TextBlock[objectSize];

            rName.TextWrapping = TextWrapping.Wrap;
            rName.Text = "Loading...";
            
        }


        /// <summary>
        /// This decides which method to do based on given subreddit or logged in state.
        /// </summary>
        /// <param name="e"></param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            postCount = 0;

            if (NavigationContext.QueryString.ContainsKey("subreddits"))
            {
                string subR = NavigationContext.QueryString["subreddits"];
                getContentWithSubr(subR);
            }

            else
            {
                if (Statics.loggedIn)
                {
                    btnLogin.Content = "Logout";
                    await getContentFrontPageLoggedIn(); 
                }
                else
                {
                    await getContentFrontPage();
                }
            }
        }

        //private async void print(object sender, System.Windows.Input.GestureEventArgs e)
        //{
        //    abc = new PostContent();

        //    await Task.Factory.StartNew(() =>
        //    {
        //        Dispatcher.BeginInvoke(() =>
        //        {
        //            tappedPost = (sender as Grid).Tag as Post;
        //            NavigationService.Navigate(new Uri("/PostContent.xaml?", UriKind.Relative));
        //        });
        //        // s = ((Grid)sender).Tag as Post;

        //    });

        //}

        /// <summary>
        /// Upvoting post. Does not work yet.
        /// </summary>
        /// <param name="sender">Post that is being given as a tag with the object.</param>
        /// <param name="e"></param>
        private async void upVotePost(object sender, System.Windows.Input.GestureEventArgs e)
        {
            await Task.Factory.StartNew(() =>
            {
                Dispatcher.BeginInvoke(() =>
                {
                  // Statics.tappedPost = (sender as TextBlock).Tag as Post;
                    ((sender as TextBlock).Tag as Post).Upvote();
                   MessageBox.Show("Upvoted post.");
                    
                });
                // s = ((Grid)sender).Tag as Post;
            });
        }

        /// <summary>
        /// Navigate to PostContent page. Set the tappedPost object in statics to the post that is tapped.
        /// </summary>
        /// <param name="sender">Post that is being given as a tag with the object.</param>
        /// <param name="e"></param>
        private void goToComments(object sender, System.Windows.Input.GestureEventArgs e)
        {
                Dispatcher.BeginInvoke(() => { 
                    Statics.tappedPost = (sender as Grid).Tag as Post;
                    NavigationService.Navigate(new Uri("/PostContent.xaml?", UriKind.Relative));
                });
        }

        /// <summary>
        /// Fill page with posts from a chosen subreddit on the menu page. If logged in then use the loggedin reddit object.
        /// </summary>
        /// <param name="subR">Given string for the subreddit to find</param>
        private async Task getContentWithSubr(string subR)
       {
           Reddit reddit = new Reddit();

           if(Statics.loggedIn == true)
           {
               reddit = authentication.authenticatedReddit;
           }
           else
           {
               reddit = new Reddit();
           }

           var sReddit = await Task.Factory.StartNew(() => { return reddit.GetSubreddit(subR); });
           pagePosts = await Task.Factory.StartNew(() => { return sReddit.Posts.Take(50); });
           rName.Text = sReddit.Title;
           fillPageWithPosts(pagePosts);

          try
           {
               Uri url = new Uri(sReddit.HeaderImage);
               headerImage.Opacity = 0.45;
               headerImage.Source = new BitmapImage(url);
           }
           catch (ArgumentNullException)
           {
           }

       }

        /// <summary>
        /// Fill page with frontpage from logged in reddit user
        /// </summary>
        /// <returns></returns>
        private async Task getContentFrontPageLoggedIn()
       {

           var sReddit = await Task.Factory.StartNew(() => { return authentication.authenticatedReddit.FrontPage; });
           pagePosts = await Task.Factory.StartNew(() => { return sReddit.Posts.Take(50); });
           rName.Text = sReddit.Title;
           fillPageWithPosts(pagePosts);

           try
           {
               Uri url = new Uri(sReddit.HeaderImage);
               headerImage.Opacity = 0.45;
               headerImage.Source = new BitmapImage(url);
           }
           catch (ArgumentNullException)
           {
               
           }

       }

        /// <summary>
        /// Fill page with logged out reddit user. Starts by default.
        /// </summary>
        /// <returns></returns>
        private async Task getContentFrontPage()
       {
           Reddit reddit = new Reddit();
           var sReddit = await Task.Factory.StartNew(() => { return reddit.FrontPage; });
           pagePosts = await Task.Factory.StartNew(() => { return sReddit.Posts.Take(50); });
           rName.Text = sReddit.Title;

           gridCollection = new Grid[objectSize];
           tBlockCollection = new TextBlock[objectSize];
           thumbnailCollection = new Image[objectSize];
           upvotesCollection = new TextBlock[objectSize];
           voteDownCollection = new TextBlock[objectSize];
           voteUpCollection = new TextBlock[objectSize];

           fillPageWithPosts(pagePosts);

           try
           {
               Uri url = new Uri(sReddit.HeaderImage);
               headerImage.Opacity = 0.45;
               headerImage.Source = new BitmapImage(url);
           }
           catch (ArgumentNullException)
           {

           }

       }

        /// <summary>
        /// The actual filling the screen with more posts then the current amount. Loading more posts.
        /// </summary>
        /// <param name="posts">The posts of the current post is being saved in this to load more posts from the current.</param>
        /// <param name="x">Every time a post is shown, this number goes up to keep up with the posts.</param>
       private async void loadMoreItems(IEnumerable<Post> posts, int x)
       {
           int initialCounter = 0;
           Dispatcher.BeginInvoke(() => { ContentPanel.Height = Height + 1000; });
           await Task.Factory.StartNew(() =>
           {
               foreach (Post post in posts)
               {
                   
                   if (postCount > initialCounter)
                   {
                       initialCounter++;
                   }
                   else
                   {
                       string postTitle = post.Title;
                       Dispatcher.BeginInvoke(() =>
                       {
                           thumb = post.Thumbnail.ToString();
                           var img = new Image();
                           img.Height = 95;
                           img.Width = 95;
                           img.Margin = new Thickness(43, 0, 0, 0);
                           thumbnailCollection[objectIndex] = img;

                           if (post.Thumbnail.ToString() != "self")
                           {
                               try
                               {
                                   Uri url3 = new Uri(thumb);
                                   img.Source = new BitmapImage(url3);
                                   img.HorizontalAlignment = HorizontalAlignment.Left;
                                   //img.Margin = new Thickness(0, 0, 0, 0);
                               }

                               catch (Exception)
                               {
                                   Uri url3 = new Uri(THUMB_DEFAULT);
                                   img.Source = new BitmapImage(url3);
                                   img.HorizontalAlignment = HorizontalAlignment.Left;
                                   img.VerticalAlignment = VerticalAlignment.Bottom;
                                   img.MaxHeight = 90;
                                   img.MaxWidth = 100;
                                   img.Margin = new Thickness(43, 0, 0, 0);
                               }
                           }

                           else
                           {
                               Uri url3 = new Uri(THUMB_DEFAULT);
                               img.Source = new BitmapImage(url3);
                               img.HorizontalAlignment = HorizontalAlignment.Left;
                           }

                           TextBlock postTit = new TextBlock();
                           tBlockCollection[objectIndex] = postTit;
                           postTit.Text = postTitle;
                           postTit.FontSize = 14;
                           postTit.Margin = new Thickness(143, 0, 0, 0);
                           postTit.TextWrapping = TextWrapping.Wrap;

                           TextBlock totalVotes = new TextBlock();
                           upvotesCollection[objectIndex] = totalVotes;
                           totalVotes.Text = post.Upvotes.ToString();
                           totalVotes.FontSize = 14;
                           totalVotes.Margin = new Thickness(5, 43, 0, 0);
                           totalVotes.TextWrapping = TextWrapping.Wrap;

                           TextBlock upvote = new TextBlock();
                           voteUpCollection[objectIndex] = upvote;
                           //upvote.Tap += new EventHandler<GestureEventArgs>(upVotePost);
                           upvote.Tag = post;
                           upvote.Text = "+";
                           upvote.FontSize = 20;
                           upvote.Margin = new Thickness(5, 5, 0, 0);
                           upvote.TextWrapping = TextWrapping.Wrap;

                           TextBlock downvote = new TextBlock();
                           voteDownCollection[objectIndex] = downvote;
                           downvote.Text = "-";
                           downvote.FontSize = 20;
                           downvote.Margin = new Thickness(5, 75, 0, 0);
                           downvote.TextWrapping = TextWrapping.Wrap;

                           var gridPost = new Grid();
                           gridPost.Tag = post;
                           gridPost.Tap += new EventHandler<GestureEventArgs>(goToComments);
                           

                           gridPost.Height = 100;
                           gridPost.Width = 465;
                           gridPost.MaxWidth = 465;
                           gridPost.VerticalAlignment = VerticalAlignment.Top;
                           gridPost.Margin = new Thickness(0, verticalMargin, 0, 0);
                           SolidColorBrush myBrush = new SolidColorBrush(Color.FromArgb(255, 35, 35, 35));
                           gridPost.Background = myBrush;

                           gridCollection[objectIndex] = gridPost;
                           gridPost.Children.Add(postTit);
                           gridPost.Children.Add(img);
                           gridPost.Children.Add(totalVotes);
                           gridPost.Children.Add(upvote);
                           gridPost.Children.Add(downvote);

                           ContentPanel.Children.Add(gridPost);

                           verticalMargin = verticalMargin + 120;
                       });
                       objectIndex++;
                   }
               }

           });

       }

        /// <summary>
        /// The actual filling the screen with posts. Aligning them etc.
        /// </summary>
        /// <param name="pagePosts">The posts object to fill the screen with.</param>
        private async void fillPageWithPosts(IEnumerable<Post> pagePosts)
       {
           await Task.Factory.StartNew(() =>
           {
               foreach (Post post in pagePosts)
               {
                   if (postCount < 10)
                   {
                       postCount++;
                       string postTitle = post.Title;
                       Dispatcher.BeginInvoke(() =>
                       {
                           thumb = post.Thumbnail.ToString();
                           var img = new Image();
                           img.Height = 95;
                           img.Width = 95;
                           img.Margin = new Thickness(43, 0, 0, 0);
                           thumbnailCollection[objectIndex] = img;

                           if (thumb != "self")
                           {
                               try
                               {
                                   Uri url3 = new Uri(thumb);
                                   img.Source = new BitmapImage(url3);
                                   img.HorizontalAlignment = HorizontalAlignment.Left;
                               }

                               catch (Exception)
                               {
                                   Uri url3 = new Uri(THUMB_DEFAULT);
                                   img.Source = new BitmapImage(url3);
                                   img.HorizontalAlignment = HorizontalAlignment.Left;
                                   img.VerticalAlignment = VerticalAlignment.Bottom;
                                   img.MaxHeight = 90;
                                   img.MaxWidth = 100;
                                   img.Margin = new Thickness(43, 0, 0, 0);
                               }
                           }

                           else
                           {
                               Uri url3 = new Uri(THUMB_DEFAULT);
                               img.Source = new BitmapImage(url3);
                               img.HorizontalAlignment = HorizontalAlignment.Left;
                           }
                           TextBlock postTit = new TextBlock();
                           tBlockCollection[objectIndex] = postTit;
                           postTit.Text = postTitle;
                           postTit.FontSize = 14;
                           postTit.Margin = new Thickness(143, 0, 0, 0);
                           postTit.TextWrapping = TextWrapping.Wrap;

                           TextBlock totalVotes = new TextBlock();
                           upvotesCollection[objectIndex] = totalVotes;
                           totalVotes.Text = post.Upvotes.ToString();
                           totalVotes.FontSize = 14;
                           totalVotes.Margin = new Thickness(5,43,0,0);
                           totalVotes.TextWrapping = TextWrapping.Wrap;

                           TextBlock upvote = new TextBlock();
                           voteUpCollection[objectIndex] = upvote;
                          // upvote.Tap += new EventHandler<GestureEventArgs>(upVotePost);
                           upvote.Tag = post;
                           upvote.Text = "+";
                           upvote.FontSize = 20;
                           upvote.Margin = new Thickness(5, 5, 0, 0);
                           upvote.TextWrapping = TextWrapping.Wrap;

                           TextBlock downvote = new TextBlock();
                           voteDownCollection[objectIndex] = downvote;
                           downvote.Text = "-";
                           downvote.FontSize = 20;
                           downvote.Margin = new Thickness(5, 75, 0, 0);
                           downvote.TextWrapping = TextWrapping.Wrap;

                           var gridPost = new Grid();
                           gridPost.Tag = post;
                           gridPost.Tap += new EventHandler<GestureEventArgs>(goToComments);
                           
                           gridPost.Height = 100;
                           gridPost.Width = 465;
                           gridPost.MaxWidth = 465;
                           gridPost.VerticalAlignment = VerticalAlignment.Top;
                           gridPost.Margin = new Thickness(0, verticalMargin, 0, 0);
                           SolidColorBrush myBrush = new SolidColorBrush(Color.FromArgb(255, 35, 35, 35));
                           gridPost.Background = myBrush;

                           gridCollection[objectIndex] = gridPost;
                           gridPost.Children.Add(postTit);
                           gridPost.Children.Add(img);
                           gridPost.Children.Add(totalVotes);
                           gridPost.Children.Add(upvote);
                           gridPost.Children.Add(downvote);

                           ContentPanel.Children.Add(gridPost);

                           verticalMargin = verticalMargin + 120;

                       });
                       objectIndex++;
                   }
               }
           });
       }


        /// <summary>
        /// Button to navigate to menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
       private void Button_Click(object sender, RoutedEventArgs e)
       {
           NavigationService.Navigate(new Uri("/SideMenu2.xaml", UriKind.Relative));
       }


       private void StackPanel_Tap(object sender, GestureEventArgs e)
       {
           
       }

        /// <summary>
        /// Button to load more items 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
       private void Button_Click_1(object sender, RoutedEventArgs e)
       {
           loadMoreItems(pagePosts,postCount);
       }

        /// <summary>
        /// Button to go to authentication, and logout when logged in.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
       private void Button_Click_2(object sender, RoutedEventArgs e)
       {
           Dispatcher.BeginInvoke(() => { if (Statics.loggedIn) { Statics.loggedIn = false; }; });
           NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
       }
    }
}
