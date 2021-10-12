using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphicsEditor
{
    class Segment : IFigure
    {
        public event EventClickMarker ClickMarker;

        private Line line;
        Point [] points = new Point[2];

        int currentPoint;
        bool clicMarker;

        Rectangle marker;
        Canvas canvas;


        public object NewObject(Canvas canvas)
        {
            line = new();
            line.Stroke = SystemColors.WindowFrameBrush;

            line.MouseMove += Line_MouseMove;
            line.MouseDown += Line_MouseDown;

            this.canvas = canvas;
            SetMarker();
            return line;
        }

        public void SetMarker()
        {
            marker = new();
            marker.Fill = Brushes.Red;

            marker.MouseRightButtonDown += Marker_MouseRightButtonDown; ;
            marker.MouseLeave += Marker_MouseLeave;
            marker.MouseRightButtonUp += Marker_MouseRightButtonUp; ;

            marker.Width = 10;
            marker.Height = 10;
        }

        private void Marker_MouseRightButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            clicMarker = false;
        }

        private void Marker_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            clicMarker = true;
            ClickMarker.Invoke(this);
        }

        public void ChangePosition(Point point)
        {
            if (clicMarker)
            {
                points[currentPoint].X = point.X;
                points[currentPoint].Y = point.Y;

                if (currentPoint == 0)
                {
                    line.X1 = point.X;
                    line.Y1 = point.Y;
                }
                else
                {
                    line.X2 = point.X;
                    line.Y2 = point.Y;
                }

                Canvas.SetLeft(marker, point.X - 5);
                Canvas.SetTop(marker, point.Y - 5);
            }
        }


        private void Marker_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            canvas.Children.Remove(marker);
        }

        private void Line_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            for (int i = 0; i < points.Length; i++)
            {
                if (Math.Abs(points[i].X - e.GetPosition(canvas).X) < 5 && (Math.Abs(points[i].Y - e.GetPosition(canvas).Y) < 5))
                {

                    Canvas.SetLeft(marker, points[i].X - 5);
                    Canvas.SetTop(marker, points[i].Y - 5);

                    currentPoint = i;

                    if (!canvas.Children.Contains(marker))
                    {
                        canvas.Children.Add(marker);
                        break;
                    }
                    else
                        break;
                }
            }
        }

        
        private void Line_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

        public void StartObject(Point point)
        {
            this.points[0] = point;
            line.X1 = point.X;
            line.Y1 = point.Y;
        }

        public void EndObject(Point point)
        {
            this.points[1] = point;
            line.X2 = point.X;
            line.Y2 = point.Y;
        }

        public object Figure()
        {
            return line;
        }

        public Point[] GetPointObject()
        {
            return points;
        }

        public void DelMarker()
        {
            marker = null;
        }
    }
}
