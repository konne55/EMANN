using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace EMANeuralNetwerk
{
    class GraphXY
    {
        String title;
        String titleXAxis;
        String titleYAxis;
        Canvas canvas;
        Point point;
        Point ursprungRel;
        List<Point> points;
        

        public GraphXY()
        {
        }

        public GraphXY(Canvas canvas, String Title, String TitleXAxis, String TitleYAxis)
        {
            this.canvas = canvas;
            title = Title;
            titleXAxis = TitleXAxis;
            titleYAxis = TitleYAxis;
            initGraph();
        }

        public List<Point> Points { get { return points; } set { points = value; } }

        public void AddPoint(Point point)
        {
            points.Add(point);
        }

        public void initGraph()
        {
            ursprungRel.X = 20;
            ursprungRel.Y = 20;


            Line xAchse = new Line();
            Line yAchse = new Line();
            xAchse.StrokeThickness = 2;
            yAchse.StrokeThickness = 2;

            xAchse.Fill = Brushes.Black;
            xAchse.Stroke = Brushes.Black;
            yAchse.Fill = Brushes.Black;
            yAchse.Stroke = Brushes.Black;


            xAchse.X1 = ursprungRel.X;
            xAchse.Y1 = ursprungRel.Y;
            yAchse.X1 = ursprungRel.X;
            yAchse.Y1 = ursprungRel.Y;

            xAchse.X2 = canvas.ActualWidth - ursprungRel.X;
            xAchse.Y2 = xAchse.Y1;
            yAchse.Y2 = canvas.ActualHeight - ursprungRel.Y;
            yAchse.X2 = yAchse.X1;


            canvas.Children.Add(xAchse);
            canvas.Children.Add(yAchse);


        }



    }
}
