﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;
using Windows.UI;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media;

namespace dot2
{
    class Circle
    {
        public double centerX{get;set;}
        public double centerY{get;set;}
        public double radius{get;set;}
       // public Ellipse ellipse{get;set;}

        public Circle()
        {
            centerX=0;
            centerY=0;
            radius=50;
            // ellipse.Width = 100;
            //ellipse.Height = 100;
           // el.Fill = new SolidColorBrush(Colors.Red);
           // ellipse.Stroke = new SolidColorBrush(Colors.Blue);
        }
    }
}
