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
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace RedditPhone
{
    public partial class PostContent : PhoneApplicationPage
    {
        SubredditContent subredditPage = new SubredditContent();
        public IEnumerable<Comment> comments;
        public Post r;
        public IEnumerable<Post> test;
        public int verticalMargin = 25;
        public int objectSize = 1000;
        public int objectIndex = 0;
        public Grid[] gridCollection;
        public TextBlock[] tBlockCollection;
        public string opgeteld = "0";
        public TextBlock[] upvotesCollection;
        public TextBlock[] voteUpCollection;
        public TextBlock[] voteDownCollection;

        
 

        public PostContent()
        {
            upvotesCollection = new TextBlock[objectSize];
            voteDownCollection = new TextBlock[objectSize];
            voteUpCollection = new TextBlock[objectSize];
            gridCollection = new Grid[objectSize];
            InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {

        //    //Reddit reddit = new Reddit();

        //    //var sReddit = await Task.Factory.StartNew(() => { return reddit.FrontPage; });
        //    //test = await Task.Factory.StartNew(() => { return sReddit.Posts.Take(1); });



            await Task.Factory.StartNew(() =>
            {


                //foreach (Comment com in Statics.tappedPost.Comments)
                //{
                    Dispatcher.BeginInvoke(() =>
                   {
                       fillPageWithComments(Statics.tappedPost);
                   });
                //}

            });


        //    //postNameText.Text = r.Title;
        //    //comments = await Task.Factory.StartNew(() => { return r.Comments.Take(1); });
        //    //doShit(test);
        //    //await Task.Factory.StartNew(() => { fillPageWithComments(comments); });
            
            

        }

        public async void doShit(IEnumerable<Post> posts)
        {
                  await Task.Factory.StartNew(() => { 
                  foreach (Post s in posts)
                  {
                      Dispatcher.BeginInvoke(() =>
                      {
                          r = s;
                      });
                  }

                  });

                  comments = await Task.Factory.StartNew(() => { return r.Comments.Take(50);
                  });
        }


        public void submitComment(Post post, string message)
        {

        }

        public void submitCommentComment(Post post, Comment comment, string message)
        {

        }

        public void voteComment(Post post, Comment comment, string upOrDown)
        {

        }

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    doShit(test);
        //}

        //public async void doShitWithComments(IEnumerable<Comment> comments)
        //{
        //    await Task.Factory.StartNew(() =>
        //    {
        //        foreach (Comment com in comments)
        //        {
        //            Dispatcher.BeginInvoke(() =>
        //            {
        //                opgeteld = opgeteld + "+" + com.Author;
        //            });
        //        }
        //    });
        //}

        public async void fillPageWithComments(Post post)
        {
            await Task.Factory.StartNew(() =>
            {
                Dispatcher.BeginInvoke(() => 
                {    
                postNameText.Text = post.Title;
                postNameText.FontSize = 35;
                postNameText.TextWrapping = TextWrapping.Wrap;
                postSubreddit.Text = "/r/" + post.Subreddit;
                postAuthor.Text = "Author: " + post.AuthorName;
                });


                foreach (Comment com in post.Comments)
                {


                    string commentBody = com.Body;
                    string commentAuthor = com.Author;
                    string commentID = com.Id;
                    

                        Dispatcher.BeginInvoke(() =>
                        {
                         
                            TextBlock txt = new TextBlock();
                            txt.Text = commentID + commentAuthor + ": " + commentBody;
                            txt.FontSize = 14;

                            txt.Margin = new Thickness(95, 0, 0, 0);
                            txt.TextWrapping = TextWrapping.Wrap;
                           

                            var commentsGrid = new Grid();
                            
                            commentsGrid.Height = 100;
                            commentsGrid.Width = 465;
                            commentsGrid.VerticalAlignment = VerticalAlignment.Top;
                            commentsGrid.Margin = new Thickness(0, verticalMargin, 0, 0);
                            SolidColorBrush myBrush = new SolidColorBrush(Color.FromArgb(255, 35, 35, 35));
                            commentsGrid.Background = myBrush;

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
                            voteUpCollection[objectIndex] = downvote;
                            downvote.Text = "-";
                            downvote.FontSize = 20;
                            downvote.Margin = new Thickness(5, 75, 0, 0);
                            downvote.TextWrapping = TextWrapping.Wrap;

                            gridCollection[objectIndex] = commentsGrid;
                            commentsGrid.Children.Add(txt);
                            commentsGrid.Children.Add(totalVotes);
                            commentsGrid.Children.Add(upvote);
                            commentsGrid.Children.Add(downvote);

                            ContentPanel.Children.Add(commentsGrid);

                            verticalMargin = verticalMargin + 115;
                        });
                        objectIndex++;
                }
            });
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await Task.Factory.StartNew(() =>
            {

                //    //Post r = new Post();

                //    foreach (Post s in test)
                //    {
                Dispatcher.BeginInvoke(() =>
                {
                    MessageBox.Show(Statics.tappedPost.CommentCount.ToString());
                    //            r = s;
                });
                //    }

            });
        }
    }
}