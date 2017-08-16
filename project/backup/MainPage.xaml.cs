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
using Windows.Devices.Sensors;
using Windows.UI.Core;
using System.Windows;
using System.Windows.Input;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;
using Windows.UI;
using Windows.UI.Xaml.Media.Animation;
using Windows.Phone.Media;



// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace dot2
{
    
    public sealed partial class MainPage : Page
    {

        Accelerometer accelerometer;
        //private uint _desiredReportInterval;
        DispatcherTimer tnfr = new DispatcherTimer();
        DispatcherTimer play = new DispatcherTimer();
        //int count = 0;
        double centerX,centerY;
        double Radius;
        bool collision;
        int i;
        int move,dmove;
        List<double> avgX=new List<double>();
        List<double> avgY = new List<double>();
        List<Dcrls> dir = new List<Dcrls>();
       private List<Circle> circles = new List<Circle>();
       private List<Ellipse> ellipses = new List<Ellipse>();
       private List<Ellipse> d_circles = new List<Ellipse>();
       double sizeofdot;
       Setting setting = new Setting();
              
           
       
        
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            startgame();
           
        }

        private void startgame()
        {
            //set dot and ring
            Canvas.SetLeft(ring, 100);
            Canvas.SetTop(ring, 220);
            Canvas.SetLeft(dot, 188);
            Canvas.SetTop(dot, 327);
            //timer for play
            play.Interval = TimeSpan.FromMilliseconds(1);
            play.Tick += play_Tick;
            play.Start();

            Radius = 100;
            i = 1;
            sizeofdot = 12;
            collision = false;
            move = 5;
            dmove = 5;
            setting.score = 0;
            setting.speed = 1;


            accelerometer = Accelerometer.GetDefault();
            uint minReportInterval = accelerometer.MinimumReportInterval;
            tnfr.Interval = TimeSpan.FromMilliseconds(10);
            tnfr.Tick += tnfr_Tick;
            // _desiredReportInterval = minReportInterval > 16 ? minReportInterval : 30;
            // accelerometer.ReportInterval = _desiredReportInterval;

            accelerometer.ReportInterval = 10;

            Window.Current.VisibilityChanged += Current_VisibilityChanged;
            //accelerometer.ReadingChanged += accelerometer_ReadingChanged;
            centerX = can.ActualWidth / 2;
            centerY = can.ActualHeight / 2;
            createCircle();
            mediaEl.Play();

            //dynamic image load test
            Image test = new Image();
            can2.Children.Add(test);
            Canvas.SetLeft(test, 100);
            Canvas.SetTop(test, 100);                  
            BitmapImage b = new BitmapImage();
            b.UriSource = new Uri(test.BaseUri,"Asstes/layout.png");
            test.Source = b;
            test.Height = 100;
            test.Width = 70;
                
           
        }

       
        void play_Tick(object sender, object e)
        {
            Canvas.SetTop(ring, Canvas.GetTop(ring) - move);
            //centerX = centerX + 2;
            centerY = centerY -move;
            Canvas.SetTop(dot, Canvas.GetTop(dot)-move);
                      
            moveellipse();
            if (Canvas.GetTop(dot) < 0) stopgame();

            removeellipse();
            if (circles.ElementAt(circles.Count - 1).centerY < can.ActualHeight - can.ActualWidth * 2 / 3)
            {
                createCircle();
                createRCircle();
            }
           
        }

        private void createRCircle()
        {
            Ellipse el = new Ellipse();
            el.Width = 60;
            el.Height =60;
            el.Fill = new SolidColorBrush(Colors.Red);
            el.Stroke = new SolidColorBrush(Colors.Blue);
            d_circles.Add(el);
            Dcrls dcrs = new Dcrls();
            dcrs.dir = 1;
            dir.Add(dcrs);
            can2.Children.Add(d_circles.ElementAt(d_circles.Count - 1));
            Random rnd=new Random();
            Canvas.SetLeft(d_circles.ElementAt(d_circles.Count-1), rnd.Next(0,Convert.ToInt32(can2.ActualWidth)));
            Canvas.SetTop(d_circles.ElementAt(d_circles.Count - 1), can2.ActualHeight-can.ActualWidth/6-30);
        }

        private void moveellipse()
        {
            int i;
             for (i = 0; i < ellipses.Count; i++)
            {
                Canvas.SetTop(ellipses.ElementAt(i), Canvas.GetTop(ellipses.ElementAt(i)) - move);
                circles.ElementAt(i).centerY = circles.ElementAt(i).centerY - move;
            }

            for (i = 0; i < d_circles.Count; i++)
            {
                Canvas.SetTop(d_circles.ElementAt(i), Canvas.GetTop(d_circles.ElementAt(i)) - move);
                if (dir.ElementAt(i).dir== 1)
                {
                    Canvas.SetLeft(d_circles.ElementAt(i), Canvas.GetLeft(d_circles.ElementAt(i)) + dmove);
                    if (Canvas.GetLeft(d_circles.ElementAt(i)) > can2.ActualWidth - 60) dir.ElementAt(i).dir = 0; 
                }
                else
                {
                    Canvas.SetLeft(d_circles.ElementAt(i), Canvas.GetLeft(d_circles.ElementAt(i)) - dmove);
                    if (Canvas.GetLeft(d_circles.ElementAt(i)) <0) dir.ElementAt(i).dir = 1; 
                }
                
                
            }
        }

        private void stopgame()
        {
            tnfr.Stop();            
            accelerometer.ReadingChanged -= new TypedEventHandler<Accelerometer, AccelerometerReadingChangedEventArgs>(ReadingChanged);
            can.Tapped -= can_Tapped;
            play.Stop();            
            this.Frame.Navigate(typeof(gameOver), setting.score+"");
            
        }

        private void removeellipse()
        {
            int i;
            for ( i= 0; i < ellipses.Count;i++ )
            {
                if (Canvas.GetTop(ellipses.ElementAt(i)) < -1*(ellipses.ElementAt(i).Height))
                {
                    
                    can2.Children.Remove(ellipses.ElementAt(i));
                    ellipses.RemoveAt(i);
                    circles.RemoveAt(i);

                }
            }

            for (i = 0; i < d_circles.Count; i++)
            {
                if (Canvas.GetTop(d_circles.ElementAt(i)) < -60)
                {

                    can2.Children.Remove(d_circles.ElementAt(i));
                    d_circles.RemoveAt(i);
                    dir.RemoveAt(i);

                }
            }
        }

        void Current_VisibilityChanged(object sender, Windows.UI.Core.VisibilityChangedEventArgs e)
        {
            if (e.Visible)
            {
                // Re-enable sensor input (no need to restore the desired reportInterval... it is restored for us upon app resume)
                accelerometer.ReadingChanged += new TypedEventHandler<Accelerometer, AccelerometerReadingChangedEventArgs>(ReadingChanged);
            }
            else
            {
                // Disable sensor input (no need to restore the default reportInterval... resources will be released upon app suspension)
                accelerometer.ReadingChanged -= new TypedEventHandler<Accelerometer, AccelerometerReadingChangedEventArgs>(ReadingChanged);
            }
        }

        //async private void ReadingChanged(Accelerometer sender, AccelerometerReadingChangedEventArgs args)
        async private void ReadingChanged(object sender, AccelerometerReadingChangedEventArgs e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {

                double h = centerY;
                double w = centerX;
                double R = Radius;
                AccelerometerReading reading = e.Reading;
               
               
                // for movement in a circle
               double X = w  + (can.ActualWidth/2) *(reading.AccelerationX);
               double Y = h  - (can.ActualHeight/2) *(reading.AccelerationY);
               double X1, Y1;

               if (avgX.Count < 10) avgX.Add(X);
               else
               {
                   avgX.RemoveAt(0);
                   avgX.Add(X);
               }
               if (avgX.Count < 10) avgY.Add(Y);
               else
               {
                   avgY.RemoveAt(0);
                   avgY.Add(Y);
               }
               Y = avgY.Average();
               X = avgX.Average(); 
               
               double tan = (Y - h) / (X-w);
               double sin = Math.Sqrt(tan * tan / (1 + tan * tan));
               double cos = Math.Sqrt(1 / (1 + tan * tan));

               if (Y < h ) Y1 = h  - sin * R-12; //+ to make the dot to remain in lower half
               else Y1 = h  + sin * R-12;
               if (X < w ) X1 = w - cos * R-12;
               else X1 = w  + cos * R-12;

               

                //set coordinates of the dot
               Canvas.SetLeft(dot, X1);
               Canvas.SetTop(dot,Y1);


            
            });


           }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.NavigationCacheMode = NavigationCacheMode.Disabled;
            
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Back)
            {
                
                //circles.Clear(); d_circles.Clear(); ellipses.Clear(); avgX.Clear(); avgY.Clear(); dir.Clear(); can2.Children.Clear();
                accelerometer.ReadingChanged += new TypedEventHandler<Accelerometer, AccelerometerReadingChangedEventArgs>(ReadingChanged);
                //can.Tapped+=can_Tapped;
                //startgame();
            }
            if (e.NavigationMode == NavigationMode.New)
            {
                accelerometer.ReadingChanged += new TypedEventHandler<Accelerometer, AccelerometerReadingChangedEventArgs>(ReadingChanged);              
            }
        }

        private void can_Tapped(object sender, TappedRoutedEventArgs e)
        {
            
            i = 1;
            effects.Stop();
            effects.Play();

            accelerometer.ReadingChanged -= new TypedEventHandler<Accelerometer, AccelerometerReadingChangedEventArgs>(ReadingChanged);
            //DispatcherTimer tnfr = new DispatcherTimer();
            //can.Tapped -= can_Tapped;
            
            tnfr.Start();
           
            
        }

       
        
        void tnfr_Tick(object sender, object e)
        {
           
            //Canvas.SetTop(dot, tY + 5*i);
            if (!collision)
            {
                translatedot(i);// remove        
                i++;
                ckeckdot();
                double tY = Canvas.GetTop(dot);
                if (tY > can.ActualHeight)
                {
                    stopgame();

                }
            }
            else
            {
                circulartransition();                
            }
              
            
        }

        private void translatedot(int i)
        {
            double tY = Canvas.GetTop(dot) +sizeofdot;
            double tX = Canvas.GetLeft(dot) + sizeofdot;
            double ty1 = tY + 2 * i;
            double tx1=centerX-((centerX-tX)*(ty1-centerY)/(tY-centerY));
            Canvas.SetLeft(dot, tx1 - sizeofdot);
            Canvas.SetTop(dot, ty1 - sizeofdot);
            return;
        }

        //check if dot is reached circle or not

        private void ckeckdot()
        {
            int p;
            double x, y, r, X, Y, R;
                for(p=0;p<circles.Count;p++)
                    {
                        X=circles.ElementAt(p).centerX;
                        Y = circles.ElementAt(p).centerY;
                        R = circles.ElementAt(p).radius;
                        x = Canvas.GetLeft(dot)-X+12; 
                        y = Canvas.GetTop(dot)-Y+12;
                        r = x * x + y * y;
                        if (r <= R * R)
                            {
                                collision = true;
                                //tnfr.Tick -= tnfr_Tick;
                                can.Tapped -= can_Tapped;
                                centerX = X;
                                centerY = Y;
                                Radius = R;
                                setting.score = setting.score + 10;
                                if (circles.ElementAt(p).type == 0) setting.score = setting.score + 20;
                                        //tnfr.Stop();
                                   

                            }
            
                    }

                for (p = 0; p < d_circles.Count; p++)
                {
                    X = Canvas.GetLeft(d_circles.ElementAt(p))+30;
                    Y = Canvas.GetTop(d_circles.ElementAt(p))+30;
                    R = 30+sizeofdot;
                    x = Canvas.GetLeft(dot) - X + 12;
                    y = Canvas.GetTop(dot) - Y + 12;
                    r = x * x + y * y;
                    if (r < R * R)
                    {
                        stopgame();
                    }
                }
            return;
        }

        private void circulartransition()
        {
            double dotX = Canvas.GetLeft(dot)+sizeofdot;
            double dotY = Canvas.GetTop(dot)+sizeofdot;
            if (centerY+Radius> dotY)
            {
                dotY = dotY + 10;
                Canvas.SetTop(dot, dotY - sizeofdot);
                if (dotX > centerX) 
                {

                    dotX = centerX + Math.Sqrt(Radius * Radius - (dotY - centerY) * (dotY - centerY));
                    Canvas.SetLeft(dot, dotX - sizeofdot);
                }
                else
                {
                   dotX = centerX- Math.Sqrt(Radius * Radius - (dotY - centerY) * (dotY - centerY));
                   Canvas.SetLeft(dot, dotX - sizeofdot); 
                }
            }
            else
            {
                avgX.Clear();
                avgY.Clear();
                accelerometer.ReadingChanged += new TypedEventHandler<Accelerometer, AccelerometerReadingChangedEventArgs>(ReadingChanged);
                can.Tapped += can_Tapped;                
                collision = false;
                tnfr.Stop();
            }
        }

        //create circle for canvas
        private void createCircle()
        {
            int size =Convert.ToInt32(can.ActualWidth);
            double height = can.ActualHeight;
            Random rnd = new Random();
            if(rnd.Next(0,10)>6)
                {
                    int c1 = rnd.Next(size /4 , size / 3);
                    int c2 = rnd.Next(size /4, size / 3);
                    int cx1 = rnd.Next(c1,size-c1);
                    int cx2 = rnd.Next(c2,size-c2);
                    double gap = rnd.Next(c1 + c2 + 50, c1 + c2 + 150);
                    gencircle(cx1,height+c1,c1,1);
                    //gencircle(cx2,height+gap,c2);
                }
            else
            {
                int c1 = rnd.Next(size / 6, size / 4);
                int c2 = rnd.Next(size / 6, size / 4);
                int a, b;
                if (rnd.Next(0, 10) > 5)
                {
                    a = 1; b = 0;
                }
                else
                {
                    a = 0; b = 1;
                }
                
                gencircle(size/4, height + c1, c1,a);
                gencircle(3*size/4,height+c2,c2,b);
            }
            return;
           
        }

        private void gencircle(double X,double Y, double R,int i)
        {
            Ellipse el = new Ellipse();
            el.Width = 2*R;
            el.Height = 2*R;
            // el.Fill = new SolidColorBrush(Colors.Red);
            switch (i)
            {
                case 1: el.Stroke = new SolidColorBrush(Colors.Blue);
                    break;
                case 0: el.Stroke = new SolidColorBrush(Colors.Green);
                    break;
                default: el.Stroke = new SolidColorBrush(Colors.Blue);
                    break;
            }
            el.StrokeThickness = 5;
            ellipses.Add(el);
            Circle circle = new Circle();
            //circle.ellipse = el;
            circle.centerX = X;
            circle.centerY = Y;
            circle.radius = R;
            circle.type = i;
            circles.Add(circle);

            //adding circle to canvas
            int n = ellipses.Count - 1;
            can2.Children.Add(ellipses.ElementAt(n));
            Canvas.SetLeft(ellipses.ElementAt(n), circle.centerX-R);
            Canvas.SetTop(ellipses.ElementAt(n), circle.centerY - R);
            
        }

       


    }
}
