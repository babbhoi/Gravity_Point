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
        int i;
        List<double> avgX=new List<double>();
        List<double> avgY = new List<double>();
       private List<Circle> circles = new List<Circle>();
       private List<Ellipse> ellipses = new List<Ellipse>();
         
       
           
       
        
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            //timer for play
            play.Interval = TimeSpan.FromMilliseconds(10);
            play.Tick += play_Tick;
            play.Start();
           
           
            Radius = 100;
            i = 1;



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
           
        }

       
        void play_Tick(object sender, object e)
        {
            Canvas.SetTop(ring, Canvas.GetTop(ring) - 3);
            //centerX = centerX + 2;
            centerY = centerY -3;
            //Canvas.SetTop(dot, Canvas.GetTop(dot));
                      
            moveellipse();
            if (Canvas.GetTop(dot) < 0) stopgame();

            removeellipse();
            if (circles.ElementAt(circles.Count - 1).centerY < can.ActualHeight*3 /4) createCircle();
        }

        private void moveellipse()
        {
            for (int i = 0; i < ellipses.Count; i++)
            {
                Canvas.SetTop(ellipses.ElementAt(i), Canvas.GetTop(ellipses.ElementAt(i)) - 3);
                circles.ElementAt(i).centerY = circles.ElementAt(i).centerY - 3;
            }
        }

        private void stopgame()
        {
            tnfr.Stop();
            play.Stop();
            accelerometer.ReadingChanged -= new TypedEventHandler<Accelerometer, AccelerometerReadingChangedEventArgs>(ReadingChanged);
        }

        private void removeellipse()
        {
            for (int i = 0; i < ellipses.Count;i++ )
            {
                if (Canvas.GetTop(ellipses.ElementAt(i)) < -1*(ellipses.ElementAt(i).Height))
                {
                    
                    can2.Children.Remove(ellipses.ElementAt(i));
                    ellipses.RemoveAt(i);
                    circles.RemoveAt(i);

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
               

                // for movement in a circle -----chrck again
               double X = w  - 12 + (w ) * (reading.AccelerationX);
               double Y = h  - 12 - (h ) * (reading.AccelerationY);
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

               if (Y < h ) Y1 = h + sin * R-12; //+ to make the dot to remain in lower half
               else Y1 = h  + sin * R-12;
               if (X < w ) X1 = w  - cos * R-12;
               else X1 = w  + cos * R-12;

               

                //set coordinates of the dot
               Canvas.SetLeft(dot, X1);
               Canvas.SetTop(dot,Y1);


            
            });


           }

        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
        }

        private void can_Tapped(object sender, TappedRoutedEventArgs e)
        {
            
            i = 1;
            accelerometer.ReadingChanged -= new TypedEventHandler<Accelerometer, AccelerometerReadingChangedEventArgs>(ReadingChanged);
            //DispatcherTimer tnfr = new DispatcherTimer();
            //can.Tapped -= can_Tapped;
            tnfr.Start();

            
        }

       
        
        void tnfr_Tick(object sender, object e)
        {
           
            //Canvas.SetTop(dot, tY + 5*i);
            translatedot(i);
           

            //-------------------
            double tY = Canvas.GetTop(dot);
            i++;
            ckeckdot();
            if (tY > can.ActualHeight)
            {
                tnfr.Stop();
                can.Tapped -= can_Tapped;
                accelerometer.ReadingChanged += new TypedEventHandler<Accelerometer, AccelerometerReadingChangedEventArgs>(ReadingChanged);
                can.Tapped += can_Tapped;

            }
            
        }

        private void translatedot(int i)
        {
            double tY = Canvas.GetTop(dot);
            double tX = Canvas.GetLeft(dot);
            double ty1 = tY + 2 * i;
            double tx1=centerX-((centerX-tX)*(ty1-centerY)/(tY-centerY));
           Canvas.SetLeft(dot, tx1);
            Canvas.SetTop(dot, ty1);
            return;
        }

        //check if dot is reached circle or not

        private void ckeckdot()
        {
            double x, y, r, X, Y, R;
                for(int p=0;p<circles.Count;p++)
                    {
                        X=circles.ElementAt(p).centerX;
                        Y = circles.ElementAt(p).centerY;
                        R = circles.ElementAt(p).radius;
                        x = Canvas.GetLeft(dot)-X+12; 
                        y = Canvas.GetTop(dot)-Y+12;
                        r = x * x + y * y;
                        if (r < R * R)
                            {

                                dotanim.Begin();
                                //tnfr.Tick -= tnfr_Tick;
                                can.Tapped -= can_Tapped;
                                centerX = X;
                                centerY = Y;
                                Radius = R;
                                accelerometer.ReadingChanged += new TypedEventHandler<Accelerometer, AccelerometerReadingChangedEventArgs>(ReadingChanged);
                                can.Tapped += can_Tapped;
                                avgX.Clear();
                                avgY.Clear();                                
                                tnfr.Stop();
                                break; 

                            }
            
                    }
            return;
        }

        //create circle for canvas
        private void createCircle()
        {
            int size =Convert.ToInt32(can.ActualWidth);
            Random rnd = new Random();
            double c1 = rnd.Next(size /8 , size / 5);
            double c2 = rnd.Next(size /8, size / 5);
            gencircle(size*1.0 / 4, c1);
            gencircle(size*3.0 / 4, c2);
            return;
           
        }

        private void gencircle(double X, double R)
        {
            Ellipse el = new Ellipse();
            el.Width = 2*R;
            el.Height = 2*R;
            // el.Fill = new SolidColorBrush(Colors.Red);
            el.Stroke = new SolidColorBrush(Colors.Blue);
            ellipses.Add(el);
            Circle circle = new Circle();
            //circle.ellipse = el;
            circle.centerX = X;
            circle.centerY = can.ActualHeight+can.ActualWidth/2;
            circle.radius = R;
            circles.Add(circle);

            //adding circle to canvas
            int n = ellipses.Count - 1;
            can2.Children.Add(ellipses.ElementAt(n));
            Canvas.SetLeft(ellipses.ElementAt(n), circle.centerX-R);
            Canvas.SetTop(ellipses.ElementAt(n), circle.centerY-R);
        }

       


    }
}
