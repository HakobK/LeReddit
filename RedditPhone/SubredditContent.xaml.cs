﻿using System;
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
        PostContent abc;

        public int verticalMargin = 25;
        public int objectSize = 1000;
        public int objectIndex = 0;
        public string username;
        public string password;
        public string thumbDefault = "http://www.reddit.com/static/self_default2.png";
        public string thumb;
        public string ifNotSet = "self";
        public Reddit LoggedInReddit = new Reddit();
        public Grid[] gridCollection;
        public TextBlock[] tBlockCollection;
        public Image[] thumbnailCollection;
        public Comment[] postListComments;
        public TextBlock[] upvotesCollection;
        public TextBlock[] voteUpCollection;
        public TextBlock[] voteDownCollection;
        public string subredditStatus;
        public int postCount = 0;
        public IEnumerable<Post> pagePosts;
        public Uri currentPostUri;
        public Post testPost;
        public Post tappedPost = new Post();

        public SubredditContent()
        {
            InitializeComponent();
            //abc = new PostContent();
            gridCollection = new Grid[objectSize];
            tBlockCollection = new TextBlock[objectSize];
            thumbnailCollection = new Image[objectSize];
            upvotesCollection = new TextBlock[objectSize];
            voteDownCollection = new TextBlock[objectSize];
            voteUpCollection = new TextBlock[objectSize];

            rName.FontSize = 27;
            rName.TextWrapping = TextWrapping.Wrap;
            rName.Text = "Loading...";
            
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            postCount = 0;

            if (NavigationContext.QueryString.ContainsKey("subreddits"))
            {
                string subR = NavigationContext.QueryString["subreddits"];
                subredditStatus = subR;
                getContentWithSubr(subR);
            }

            else
            {
                if (Statics.loggedIn)
                {
                    subredditStatus = "frontpageloggedin";
                    await getContentFrontPageLoggedIn(authentication.authenticatedReddit); 
                }
                else
                {
                    await getContentFrontPage();
                }
            }
        }

        private async void print(object sender, System.Windows.Input.GestureEventArgs e)
        {
            abc = new PostContent();

            await Task.Factory.StartNew(() =>
            {
                Dispatcher.BeginInvoke(() =>
                {
                    tappedPost = (sender as Grid).Tag as Post;
                    NavigationService.Navigate(new Uri("/PostContent.xaml?", UriKind.Relative));
                });
                // s = ((Grid)sender).Tag as Post;

            });

        }
        private async void upVotePost(object sender, System.Windows.Input.GestureEventArgs e)
        {

            await Task.Factory.StartNew(() =>
            {
                Dispatcher.BeginInvoke(() =>
                {
                    tappedPost = (sender as TextBlock).Tag as Post;
                    tappedPost.SetVote(VotableThing.VoteType.Upvote);
                });
                // s = ((Grid)sender).Tag as Post;

            });

        }

        //private async void print(object sender, System.Windows.Input.GestureEventArgs e)
        //{

        //    tappedPost = (sender as Grid).Tag as Post;
        //    await Task.Factory.StartNew(() =>
        //    {
        //        Dispatcher.BeginInvoke(() => NavigationService.Navigate(new Uri("/PostContent.xaml?", UriKind.Relative)));
        //    }
        //        );
        //}

       public async void getContentWithSubr(string subR)
       {
           Reddit reddit = new Reddit();
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
               MessageBox.Show("Error loading picture");
           }

       }

       public async Task login(string user, string pass)
       {     
           await Task.Factory.StartNew(() =>
           {

               try
               {
                       LoggedInReddit.LogIn(username, password); 
               }
               catch (Exception e)
               {
                   Dispatcher.BeginInvoke(() =>
                   {
                       MessageBox.Show(e.ToString());

                       //NavigationService.Navigate(new Uri("/MainPage.xaml?key=" + "false", UriKind.Relative));
                   });
               }
           });

       }

       public async Task getContentFrontPageLoggedIn(Reddit reddit)
       {

           var sReddit = await Task.Factory.StartNew(() => { return reddit.FrontPage; });
           pagePosts = await Task.Factory.StartNew(() => { return sReddit.Posts.Take(50); });
           //var text = await Task.Factory.StartNew(() => { return posts.Count().ToString(); });
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

       public async Task getContentFrontPage()
       {
           Reddit reddit2 = new Reddit();
           var sReddit = await Task.Factory.StartNew(() => { return reddit2.FrontPage; });
           pagePosts = await Task.Factory.StartNew(() => { return sReddit.Posts.Take(50); });
         //  var text = await Task.Factory.StartNew(() => { return pagePosts.Count().ToString(); });
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

       public async void getContentFrontPageNew()
       {
           MessageBox.Show("Getting new posts");
           ContentPanel.Children.Clear();
           tBlockCollection = null;
           thumbnailCollection = null;
           verticalMargin = 25;
           objectSize = 25;
           objectIndex = 0;
           Reddit reddit = new Reddit();
           reddit = LoggedInReddit;
           var sReddit = await Task.Factory.StartNew(() => { return reddit.FrontPage; });
           var posts = await Task.Factory.StartNew(() => { return sReddit.New.Take(50); });
           //var text = await Task.Factory.StartNew(() => { return posts.Count().ToString(); });
           rName.Text = sReddit.Title;

           fillPageWithPosts(posts);

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
 

        public async void loadMoreItems(IEnumerable<Post> posts, int x)
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
                                   Uri url3 = new Uri(thumbDefault);
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
                               Uri url3 = new Uri(thumbDefault);
                               img.Source = new BitmapImage(url3);
                               img.HorizontalAlignment = HorizontalAlignment.Left;
                               // img.Margin = new Thickness(0, 0, 0, 0);
                           }

                           TextBlock postTit = new TextBlock();
                           tBlockCollection[objectIndex] = postTit;
                           postTit.Text = postTitle + " " + postCount.ToString();
                           postTit.FontSize = 14;
                           postTit.Margin = new Thickness(143, 0, 0, 0);
                           postTit.TextWrapping = TextWrapping.Wrap;
                           //  txt.Margin = new Thickness(60,0,0,0);

                           TextBlock totalVotes = new TextBlock();
                           upvotesCollection[objectIndex] = totalVotes;
                           totalVotes.Text = post.Upvotes.ToString();
                           totalVotes.FontSize = 14;
                           totalVotes.Margin = new Thickness(5, 43, 0, 0);
                           totalVotes.TextWrapping = TextWrapping.Wrap;

                           TextBlock upvote = new TextBlock();
                           voteUpCollection[objectIndex] = upvote;
                           upvote.Tap += new EventHandler<GestureEventArgs>(upVotePost);
                           upvote.Tag = post;
                           upvote.Text = "+";
                           upvote.FontSize = 20;
                           upvote.Margin = new Thickness(5, 5, 0, 0);
                           upvote.TextWrapping = TextWrapping.Wrap;

                           TextBlock downvote = new TextBlock();
                           voteUpCollection[objectIndex] = downvote;
                           downvote.Text = "-";
                           downvote.FontSize = 20;
                           downvote.Margin = new Thickness(5, 75, 0, 0);
                           downvote.TextWrapping = TextWrapping.Wrap;

                           var gridPost = new Grid();
                           //Dispatcher.BeginInvoke(() => { postListComments = post.Comments; });
                           gridPost.Tag = post;
                           gridPost.Tap += new EventHandler<GestureEventArgs>(print);
                           //   gridPost.MaxHeight = 70;

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


                           //TextBlock txt = new TextBlock();
                           //tBlockCollection[objectIndex] = txt;
                           
                           //txt.Text = postTitle + " " + postCount.ToString() ;
                           //txt.FontSize = 14;

                           //txt.Margin = new Thickness(95, 0, 0, 0);
                           //txt.TextWrapping = TextWrapping.Wrap;
                           ////  txt.Margin = new Thickness(60,0,0,0);

                           //var gridPost = new Grid();
                           ////Dispatcher.BeginInvoke(() => { postListComments = post.Comments; });
                           //gridPost.Tap += new EventHandler<GestureEventArgs>(print);
                           //gridPost.Tag = post;
                           //gridPost.MaxHeight = 70;
                           //gridPost.Height = 80;
                           //gridPost.Width = 465;
                           //gridPost.MaxWidth = 465;
                           //gridPost.VerticalAlignment = VerticalAlignment.Top;
                           //gridPost.Margin = new Thickness(0, verticalMargin, 0, 0);
                           //SolidColorBrush myBrush = new SolidColorBrush(Color.FromArgb(255, 35, 35, 35));
                           //gridPost.Background = myBrush;

                           //gridCollection[objectIndex] = gridPost;
                           //gridPost.Children.Add(txt);
                           //gridPost.Children.Add(img);

                           //ContentPanel.Children.Add(gridPost);
                           
                           //verticalMargin = verticalMargin + 90;

                       });

                       objectIndex++;
                       
                       
                   }
               }

               //!!!
           });


       }

        public async void fillPageWithPosts(IEnumerable<Post> pagePosts)
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

                           if (thumb != ifNotSet)
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
                                   Uri url3 = new Uri(thumbDefault);
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
                               Uri url3 = new Uri(thumbDefault);
                               img.Source = new BitmapImage(url3);
                               img.HorizontalAlignment = HorizontalAlignment.Left;
                               // img.Margin = new Thickness(0, 0, 0, 0);
                           }
                           TextBlock postTit = new TextBlock();
                           tBlockCollection[objectIndex] = postTit;
                           postTit.Text = postTitle + " " + postCount.ToString();
                           postTit.FontSize = 14;
                           postTit.Margin = new Thickness(143, 0, 0, 0);
                           postTit.TextWrapping = TextWrapping.Wrap;
                           //  txt.Margin = new Thickness(60,0,0,0);

                           TextBlock totalVotes = new TextBlock();
                           upvotesCollection[objectIndex] = totalVotes;
                           totalVotes.Text = post.Upvotes.ToString();
                           totalVotes.FontSize = 14;
                           totalVotes.Margin = new Thickness(5,43,0,0);
                           totalVotes.TextWrapping = TextWrapping.Wrap;

                           TextBlock upvote = new TextBlock();
                           voteUpCollection[objectIndex] = upvote;
                           upvote.Tap += new EventHandler<GestureEventArgs>(upVotePost);
                           upvote.Tag = post;
                           upvote.Text = "+";
                           upvote.FontSize = 20;
                           upvote.Margin = new Thickness(5, 5, 0, 0);
                           upvote.TextWrapping = TextWrapping.Wrap;

                           TextBlock downvote = new TextBlock();
                           voteUpCollection[objectIndex] = downvote;
                           downvote.Text = "-";
                           downvote.FontSize = 20;
                           downvote.Margin = new Thickness(5, 75, 0, 0);
                           downvote.TextWrapping = TextWrapping.Wrap;

                           var gridPost = new Grid();
                           //Dispatcher.BeginInvoke(() => { postListComments = post.Comments; });
                           gridPost.Tag = post;
                           gridPost.Tap += new EventHandler<GestureEventArgs>(print);
                        //   gridPost.MaxHeight = 70;
                           
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



       private void newTap(object sender, GestureEventArgs e)
       {
           getContentFrontPageNew();
       }

       private void Button_Click(object sender, RoutedEventArgs e)
       {
           NavigationService.Navigate(new Uri("/SideMenu2.xaml", UriKind.Relative));
       }

       private void newTxt_Tap(object sender, GestureEventArgs e)
       {
           getContentFrontPageNew();
       }

       private void StackPanel_Tap(object sender, GestureEventArgs e)
       {
           
       }

       private void Button_Click_1(object sender, RoutedEventArgs e)
       {
           loadMoreItems(pagePosts,postCount);
       }
    }
}

//await Task.Factory.StartNew(() =>
//{
//    foreach (Post post in posts)
//    {

//        string postTitle = post.Title;
//        Dispatcher.BeginInvoke(() =>
//        {
//            // MessageBox.Show(post.Thumbnail.ToString());
//            thumb = post.Thumbnail.ToString();
//            var img = new Image();
//            img.Height = 80;
//            img.Width = 80;
//            img.Margin = new Thickness(10, 0, 0, 0);
//            thumbnailCollection[objectIndex] = img;

//            if (thumb != ifNotSet)
//            {
//                try
//                {
//                    Uri url3 = new Uri(thumb);
//                    img.Source = new BitmapImage(url3);
//                    img.HorizontalAlignment = HorizontalAlignment.Left;
//                    //img.Margin = new Thickness(0, 0, 0, 0);
//                }

//                catch (Exception)
//                {
//                    Uri url3 = new Uri(thumbDefault);
//                    img.Source = new BitmapImage(url3);
//                    img.HorizontalAlignment = HorizontalAlignment.Left;
//                    img.VerticalAlignment = VerticalAlignment.Bottom;
//                    img.MaxHeight = 70;
//                    img.MaxWidth = 100;
//                    // img.Margin = new Thickness(0, 0, 0, 0);
//                }
//            }

//            else
//            {
//                Uri url3 = new Uri(thumbDefault);
//                img.Source = new BitmapImage(url3);
//                img.HorizontalAlignment = HorizontalAlignment.Left;
//                // img.Margin = new Thickness(0, 0, 0, 0);
//            }
//            TextBlock txt = new TextBlock();
//            tBlockCollection[objectIndex] = txt;
//            txt.Text = postTitle;
//            txt.FontSize = 14;

//            txt.Margin = new Thickness(95, 0, 0, 0);
//            txt.TextWrapping = TextWrapping.Wrap;
//            //  txt.Margin = new Thickness(60,0,0,0);

//            var panel1 = new Grid();
//            //Dispatcher.BeginInvoke(() => { postListComments = post.Comments; });
//            panel1.Tap += new EventHandler<GestureEventArgs>(print);
//            panel1.MaxHeight = 70;
//            panel1.Height = 80;
//            panel1.Width = 465;
//            panel1.MaxWidth = 465;
//            panel1.VerticalAlignment = VerticalAlignment.Top;
//            panel1.Margin = new Thickness(0, verticalMargin, 0, 0);
//            SolidColorBrush myBrush = new SolidColorBrush(Color.FromArgb(255, 35, 35, 35));
//            panel1.Background = myBrush;

//            gridCollection[objectIndex] = panel1;
//            panel1.Children.Add(txt);
//            panel1.Children.Add(img);

//            ContentPanel.Children.Add(panel1);

//            verticalMargin = verticalMargin + 90;

//        });

//        objectIndex++;

//    }
//});
