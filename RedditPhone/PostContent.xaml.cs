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
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace RedditPhone
{
    public partial class PostContent : PhoneApplicationPage
    {
        private int verticalMargin = 25;
        private int objectSize = 1000;
        private int objectIndex = 0;
        private Grid[] gridCollection;
        private TextBlock[] tBlockCollection;
        private TextBlock[] upvotesCollection;
        private TextBlock[] voteUpCollection;
        private TextBlock[] voteDownCollection;

        public PostContent()
        {
            upvotesCollection = new TextBlock[objectSize];
            voteDownCollection = new TextBlock[objectSize];
            voteUpCollection = new TextBlock[objectSize];
            gridCollection = new Grid[objectSize];
            InitializeComponent();
        }

        /// <summary>
        /// When navigated to this page does the method fillPageWithComments while giving the tapped post on subredditcontent
        /// </summary>
        /// <param name="e"></param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            await Task.Factory.StartNew(() =>
            {
                    Dispatcher.BeginInvoke(() =>
                   {
                       fillPageWithComments(Statics.tappedPost);
                   });
            });
        }


        /// <summary>
        /// Fills page with comments with given post. Works the same way as filling screen with posts.
        /// </summary>
        /// <param name="post"></param>
        private async void fillPageWithComments(Post post)
        {
            await Task.Factory.StartNew(() =>
            {
                Dispatcher.BeginInvoke(() => 
                {    
                postNameText.Text = post.Title;
                postNameText.FontSize = 30;
                postNameText.TextWrapping = TextWrapping.Wrap;
                postSubreddit.Text = "/r/" + post.Subreddit;
                postAuthor.Text = "Author: " + post.AuthorName;
                });
                foreach (Comment com in post.Comments)
                {
                    string commentBody = com.Body;
                    string commentAuthor = com.Author;
                        Dispatcher.BeginInvoke(() =>
                        {
                            TextBlock txt = new TextBlock();
                            txt.Text = commentAuthor + ": " + commentBody;
                            txt.FontSize = 14;
                            txt.Margin = new Thickness(30, 0, 0, 0);
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
                            totalVotes.Text = com.Upvotes.ToString();
                            totalVotes.FontSize = 14;
                            totalVotes.Margin = new Thickness(5, 43, 0, 0);
                            totalVotes.TextWrapping = TextWrapping.Wrap;

                            TextBlock upvote = new TextBlock();
                            voteUpCollection[objectIndex] = upvote;
                            //upvote.Tap += new EventHandler<GestureEventArgs>(upVotePost);
                            upvote.Tag = com;
                            upvote.Text = "+";
                            upvote.FontSize = 20;
                            upvote.Margin = new Thickness(5, 5, 0, 0);
                            upvote.TextWrapping = TextWrapping.Wrap;

                            TextBlock downvote = new TextBlock();
                            voteUpCollection[objectIndex] = downvote;
                            downvote.Text = "-";
                            downvote.Tag = com;
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

        /// <summary>
        /// button to navigate back to subredditcontent
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_Homepage(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/SubredditContent.xaml?", UriKind.Relative));

        }

        /// <summary>
        /// button to navigate to menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/SideMenu2.xaml?", UriKind.Relative));

        }

    }
}