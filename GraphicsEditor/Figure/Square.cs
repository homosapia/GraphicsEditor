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
    class Square : IFigure
    {
        public event EventCilckMarker ClickMarker;
        public event EventGetFigure GetFigure;

        private Rectangle rectangle;
        Point [] points = new Point[2];

        Canvas canvas;
        int currentPoint;
        bool clickMarker;

        Rectangle marker;

        public object NewObject(Canvas canvas)
        {
            this.canvas = canvas;

            rectangle = new();
            rectangle.MouseMove += Rectangle_MouseMove;
            rectangle.MouseDown += Rectangle_MouseDown;
            rectangle.Stroke = new SolidColorBrush(Colors.Black);

            SetMarker();
            marker.MouseRightButtonDown += Marker_MouseRightButtonDown;
            marker.MouseRightButtonUp += Marker_MouseRightButtonUp;
            marker.MouseLeave += Marker_MouseLeave;

            return rectangle;
        }

        private void Rectangle_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

        private void Marker_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            canvas.Children.Remove(marker);
        }

        private void Marker_MouseRightButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            clickMarker = false;
        }

        private void Marker_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            clickMarker = true;
        }

        private void Rectangle_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Point point = e.GetPosition(canvas);

            if (Math.Abs(points[1].X - point.X) < 5 && (Math.Abs(points[1].Y - point.Y) < 5))
            {
                Canvas.SetLeft(marker, points[1].X - 5);
                Canvas.SetTop(marker, points[1].Y - 5);

                if (!canvas.Children.Contains(marker))
                {
                    canvas.Children.Add(marker);
                }
            }
        }

        public void SetMarker()
        {
            marker = new();
            marker.Fill = Brushes.Red;
            marker.Width = 10;
            marker.Height = 10;
        }

        public object Figure()
        {
            return rectangle;
        }

        public void StartObject(Point point)
        {
            this.points[0] = point;
            Canvas.SetLeft(rectangle, point.X);
            Canvas.SetTop(rectangle, point.Y);
        }

        public void EndObject(Point point)
        {
            this.points[1] = point;
            if (point.X > this.points[0].X && point.Y > this.points[0].Y)
            {
                Canvas.SetLeft(rectangle, points[0].X);
                Canvas.SetTop(rectangle, points[0].Y);

                rectangle.Width = Math.Abs(points[1].X - this.points[0].X);
                rectangle.Height = Math.Abs(points[1].Y - this.points[0].Y);
            }

            if (point.X < this.points[0].X && point.Y < this.points[0].Y)
            {
                Canvas.SetLeft(rectangle, points[1].X);
                Canvas.SetTop(rectangle, points[1].Y);

                rectangle.Width = Math.Abs(this.points[0].X - points[1].X);
                rectangle.Height = Math.Abs(this.points[0].Y - points[1].Y);
            }

            if (point.X > this.points[0].X && point.Y < this.points[0].Y)
            {
                Canvas.SetLeft(rectangle, this.points[0].X);
                Canvas.SetTop(rectangle, points[1].Y);

                rectangle.Width = Math.Abs(this.points[0].X - points[1].X);
                rectangle.Height = Math.Abs(this.points[0].Y - points[1].Y);
            }

            if (point.X < this.points[0].X && point.Y > this.points[0].Y)
            {
                Canvas.SetLeft(rectangle, points[1].X);
                Canvas.SetTop(rectangle, this.points[0].Y);

                rectangle.Width = Math.Abs(this.points[0].X - points[1].X);
                rectangle.Height = Math.Abs(this.points[0].Y - points[1].Y);
            }

            Canvas.SetLeft(marker, points[1].X - 5);
            Canvas.SetTop(marker, points[1].Y - 5);
        }

        public void DelMarker()
        {
            marker = null;
        }

        public void ChangePosition(Point point)
        {
            if (clickMarker)
            {
                EndObject(point);
            }
        }

        public void CreateFigure(Point mouse)
        {
            throw new NotImplementedException();
        }
    }
}
