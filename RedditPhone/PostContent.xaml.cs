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
        public int objectSize = 55;
        public int objectIndex = 0;
        public Grid[] gridCollection;
        public TextBlock[] tBlockCollection;
        public string opgeteld = "0";
 

        public PostContent()
        {
            gridCollection = new Grid[objectSize];
            tBlockCollection = new TextBlock[objectSize];
            InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {

            Reddit reddit = new Reddit();

            var sReddit = await Task.Factory.StartNew(() => { return reddit.FrontPage; });
            test = await Task.Factory.StartNew(() => { return sReddit.Posts.Take(1); });


            await Task.Factory.StartNew(() =>
            {
                foreach (Post s in test)
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        r = s;
                    });
                }

            });

            comments = await Task.Factory.StartNew(() => { return r.Comments.Take(50); });


            

        }

        //public void fillPagewithPost(Post post)
        //{
        //    Post newPost = new Post();
        //    post.Id = postID;
        //    //textBoxPost.Text = newPost.Id;
        //    textBoxPost.Text = post.Title;
        //    Reddit s = new Reddit();
        //    Post g = new Post();
        //    //MessageBox.Show("hallo" + postID);
        //}

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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            doShit(test);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            fillPageWithComments(comments);
        }

        public async void doShitWithComments(IEnumerable<Comment> comments)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (Comment com in comments)
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        opgeteld = opgeteld + "+" + com.Author;
                    });
                }


            });

            Dispatcher.BeginInvoke(() => { textBoxPost.Text = opgeteld; });

        }

        public async void fillPageWithComments(IEnumerable<Comment> comments)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (Comment com in comments)
                {


                    string postTitle = com.Body.ToString();

                        Dispatcher.BeginInvoke(() =>
                        {
                          //  thumb = post.Thumbnail.ToString();
                           // var img = new Image();
                           // img.Height = 80;
                           // img.Width = 80;
                           // img.Margin = new Thickness(10, 0, 0, 0);
                           // thumbnailCollection[objectIndex] = img;

                            //if (thumb != ifNotSet)
                            //{
                            //    try
                            //    {
                            //        Uri url3 = new Uri(thumb);
                            //        img.Source = new BitmapImage(url3);
                            //        img.HorizontalAlignment = HorizontalAlignment.Left;
                            //        //img.Margin = new Thickness(0, 0, 0, 0);
                            //    }

                            //    catch (Exception)
                            //    {
                            //        Uri url3 = new Uri(thumbDefault);
                            //        img.Source = new BitmapImage(url3);
                            //        img.HorizontalAlignment = HorizontalAlignment.Left;
                            //        img.VerticalAlignment = VerticalAlignment.Bottom;
                            //        img.MaxHeight = 70;
                            //        img.MaxWidth = 100;
                            //        // img.Margin = new Thickness(0, 0, 0, 0);
                            //    }
                            //}

                            //else
                            //{
                            //    Uri url3 = new Uri(thumbDefault);
                            //    img.Source = new BitmapImage(url3);
                            //    img.HorizontalAlignment = HorizontalAlignment.Left;
                            //    // img.Margin = new Thickness(0, 0, 0, 0);
                            //}
                            TextBlock txt = new TextBlock();
                            tBlockCollection[objectIndex] = txt;
                            txt.Text = postTitle;
                            txt.FontSize = 14;

                            txt.Margin = new Thickness(95, 0, 0, 0);
                            txt.TextWrapping = TextWrapping.Wrap;
                            //  txt.Margin = new Thickness(60,0,0,0);

                            var panel1 = new Grid();
                            //Dispatcher.BeginInvoke(() => { postListComments = post.Comments; });
                           // panel1.Tap += new EventHandler<GestureEventArgs>(print);
                            panel1.MaxHeight = 70;
                            panel1.Height = 80;
                            panel1.Width = 465;
                            panel1.MaxWidth = 465;
                            panel1.VerticalAlignment = VerticalAlignment.Top;
                            panel1.Margin = new Thickness(0, verticalMargin, 0, 0);
                            SolidColorBrush myBrush = new SolidColorBrush(Color.FromArgb(255, 35, 35, 35));
                            panel1.Background = myBrush;

                            gridCollection[objectIndex] = panel1;
                            panel1.Children.Add(txt);
                           // panel1.Children.Add(img);

                            ContentPanel.Children.Add(panel1);

                            verticalMargin = verticalMargin + 90;

                        });

                        objectIndex++;

                    
                }

            });
        }

    }
}