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
        public event EventGetFigure GetFigure;
        public event EventCilckMarker ClickMarker;

        Polyline polyline;

        int currentPoint;
        bool clickMarker;

        Rectangle marker;
        Canvas canvas;

        public Segment(Canvas canvas)
        {
            this.canvas = canvas;
            SetMarker();
            NewPoliLine();
        }

        public object NewObject(Canvas canvas)
        {
            //del
            return "dfdf";
        }

        public void CreateFigure(Point mouse)
        {
            polyline.Points.Add(mouse);
            currentPoint++;
            polyline.Points.Add(mouse);
        }

        public void SetMarker()
        {
            marker = new();
            marker.Fill = Brushes.Red;

            marker.MouseLeftButtonDown += Marker_MouseLeftButtonDown;
            marker.MouseLeave += Marker_MouseLeave;
            marker.MouseLeftButtonUp += Marker_MouseLeftButtonUp;

            marker.Width = 10;
            marker.Height = 10;
        }

        private void NewPoliLine()
        {
            polyline = new();
            polyline.Stroke = SystemColors.WindowFrameBrush;

            polyline.MouseDown += Polyline_MouseDown;
            polyline.MouseMove += Polyline_MouseMove;
        }



        private void Polyline_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            GetFigure(this);
            for (int i = 0; i < polyline.Points.Count; i++)
            {
                Point point = e.GetPosition(canvas);

                if (Math.Abs(polyline.Points[i].X - point.X) < 5 && (Math.Abs(polyline.Points[i].Y - point.Y) < 5))
                {
                    currentPoint = i;

                    if (!canvas.Children.Contains(marker))
                        canvas.Children.Add(marker);
                }
            }
        }

        private void Polyline_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

        private void Marker_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ClickMarker(false);
        }

        private void Marker_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ClickMarker(true);
        }

        public void ChangePosition(Point point)
        {
            polyline.Points[currentPoint] = point;
            Canvas.SetLeft(marker, polyline.Points[currentPoint].X - 5);
            Canvas.SetTop(marker, polyline.Points[currentPoint].Y - 5);
        }

        private void Marker_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            canvas.Children.Remove(marker);
        }

        
        private void Line_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

        public void StartObject(Point point)
        {
        }

        public void EndObject(Point point)
        {
        }

        public object Figure()
        {
            return polyline;
        }

        public void DelMarker()
        {
            marker = null;
        }
    }
}
