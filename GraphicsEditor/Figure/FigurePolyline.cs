using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphicsEditor
{
    class FigurePolyline : IFigure
    {
        public event EventSelectFigure SelectObject;
        public event EventTransform Transform;
        public event EventSetMarker SetMarker;
        public event EventClickMarker ClickMarker;
        public event EventRemoveMarker RemoveMarker;

        Polyline polyline;

        int currentPoint;
        bool clickMarker;

        Rectangle marker;

        Canvas canvas;

        public FigurePolyline(Canvas canvas)
        {
            this.canvas = canvas;
            NewPoliLine();
        }

        public void CreateFigure(Point mouse)
        {
            polyline.Points.Add(mouse);
            polyline.Points.Add(mouse);

            CreateMarker();
        }

        public void CreateMarker()
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

        private void Polyline_MouseMove(object sender, MouseEventArgs e)
        {
            SelectObject(this);
            for (int i = 0; i < polyline.Points.Count; i++)
            {
                Point point = e.GetPosition(canvas);
                if (Math.Abs(polyline.Points[i].X - point.X) <= 5 && Math.Abs(polyline.Points[i].Y - point.Y) <= 5)
                {
                    currentPoint = i;

                    Canvas.SetLeft(marker, polyline.Points[currentPoint].X - 5);
                    Canvas.SetTop(marker, polyline.Points[currentPoint].Y - 5);

                    if (!canvas.Children.Contains(marker))
                        canvas.Children.Add(marker);
                }
            }
        }

        private void Polyline_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ClickCount == 2)
            {
                Point point = e.GetPosition(canvas);

                Point point1 = polyline.Points.Last();

                polyline.Points[^1] = point;
                polyline.Points.Add(point1);
            }
        }

        private void Marker_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ClickMarker(false);
        }

        private void Marker_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ClickMarker(true);
        }

        public void ChangePosition(Point point)
        {
            polyline.Points[currentPoint] = point;

            Canvas.SetLeft(marker, polyline.Points[currentPoint].X - 5);
            Canvas.SetTop(marker, polyline.Points[currentPoint].Y - 5);
        }

        private void Marker_MouseLeave(object sender, MouseEventArgs e)
        {
            canvas.Children.Remove(marker);
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
        }

        public void DrawFigure(Point point)
        {
            throw new NotImplementedException();
        }
    }
}
