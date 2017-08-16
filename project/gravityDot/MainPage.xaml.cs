using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Graphics;
using Windows.Graphics.Display;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;
using Windows.UI;
using Windows.UI.Xaml.Media.Animation;
using Windows.Graphics.Display;

//using Windows.UI.Xaml.Resources;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace gravityDot
{
    
    public sealed partial class MainPage : Page
    {
        private List<Circle> circles = new List<Circle>();
       //BitmapImage circleimage = new BitmapImage(new Uri(@"Assets/ring.png", UriKind.Relative));
        DispatcherTimer gametimer = new DispatcherTimer();
        Ellipse el = new Ellipse();
     
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            //Image img = new Image();
            gametimer.Interval = TimeSpan.FromMilliseconds(10);
            gametimer.Tick += gametimer_Tick;
            gametimer.Start();
            startGame();
            Rectangle redRectangle = new Rectangle();
            redRectangle.Width = 200;
            redRectangle.Height = 200;
            redRectangle.Stroke = new SolidColorBrush(Colors.Black);
            redRectangle.StrokeThickness = 10;
            redRectangle.Fill = new SolidColorBrush(Colors.Red);

            Canvas.SetLeft(redRectangle, 10);
            Canvas.SetTop(redRectangle, 10);
            // Add Rectangle to Canvas
            can.Children.Add(redRectangle);

            
            el.Width = 100;
            el.Height = 100;
            //el.Fill = new SolidColorBrush(Colors.Red);
            el.Stroke = new SolidColorBrush(Colors.Blue);
            Canvas.SetLeft(el,210);
            Canvas.SetTop(el, 210);
            can.Children.Add(el);

            //add image
            Image test = new Image();
            BitmapImage b = new BitmapImage();
            b.UriSource = new Uri("/Asstes/Deck/34.png", UriKind.Relative);
            test.Source = b;
            test.Height = 100;
            test.Width = 70;
            can.Children.Add(test);
        }

        void gametimer_Tick(object sender, object e)
        {
            tb1.Text = "dfghjkl";
            Canvas.SetTop(el, (Canvas.GetTop(el) + 10)%(can.ActualHeight));
            
        }

        private void startGame()
        {
            circles.Clear();

            Circle maincircle = new Circle();
            maincircle.X = can.ActualWidth / 2;
            maincircle.Y = can.ActualHeight / 2;
            maincircle.R = 100;

            circles.Add(maincircle);
        }

        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
           
        }

       
    }
}
