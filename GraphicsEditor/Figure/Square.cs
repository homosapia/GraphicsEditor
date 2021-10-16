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
        public event EventClickMarker ClickMarker;
        public event EventSetMarker SetMarker;
        public event EventTransform Transform;
        public event EventSelectFigure SelectObject;
        public event EventRemoveMarker RemoveMarker;


        private Rectangle rectangle;
        private RotateTransform RotateFigure = new();
        
        Point [] points = new Point[2];

        Canvas canvas;
        int TransformOrTurn;
        bool clickMarker;

        Rectangle marker;

        public Square(Canvas canvas)
        {
            this.canvas = canvas;

            rectangle = new();

            rectangle.RenderTransform = RotateFigure;

            rectangle.Stroke = new SolidColorBrush(Colors.Black);
            rectangle.MouseMove += Rectangle_MouseMove;
            rectangle.MouseDown += Rectangle_MouseDown;

            marker = CreateMarker(Brushes.Red);
        }
        public Rectangle CreateMarker(Brush brush)
        {
            Rectangle marker = new();
            marker.Fill = brush;
            marker.Width = 10;
            marker.Height = 10;

            marker.MouseLeftButtonDown += Marker_MouseLeftButtonDown;
            marker.MouseLeftButtonUp += Marker_MouseLeftButtonUp;
            marker.MouseLeave += Marker_MouseLeave;

            return marker;
        }

        private void Marker_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            clickMarker = false;
            ClickMarker(clickMarker);
        }

        private void Marker_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            clickMarker = true;
            ClickMarker(clickMarker);
        }

        public object NewObject(Canvas canvas)
        {
            return canvas;
        }

        private void Rectangle_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

        private void Marker_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            canvas.Children.Remove(marker);
        }

        private void Rectangle_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            SelectObject(this);

            Point point = e.GetPosition(canvas);

            if (Math.Abs(points[1].X - point.X) < 5 && (Math.Abs(points[1].Y - point.Y) < 5))
            {
                TransformOrTurn = 1;
                Canvas.SetLeft(marker, points[1].X - 5);
                Canvas.SetTop(marker, points[1].Y - 5);

                if (!canvas.Children.Contains(marker))
                {
                    canvas.Children.Add(marker);
                }

                
            }

            if (Math.Abs(points[1].X - point.X) < 5 && (Math.Abs(points[0].Y - point.Y) < 5))
            {
                TransformOrTurn = 2;
                Canvas.SetLeft(marker, points[1].X - 5);
                Canvas.SetTop(marker, points[1].Y - 5);

                if (!canvas.Children.Contains(marker))
                {
                    marker.Fill = Brushes.Green;
                    canvas.Children.Add(marker);
                }


            }
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


        public void DelMarker()
        {
            marker = null;
        }

        public void ChangePosition(Point point)
        {
            this.points[1] = point;

            if (TransformOrTurn == 1)
            {
                _Transform();
            }
            else
            {
                Rotate(point);
            }

            Canvas.SetLeft(marker, points[1].X - 5);
            Canvas.SetTop(marker, points[1].Y - 5);
        }

        private void _Transform()
        {
            if (points[1].X > this.points[0].X && points[1].Y > this.points[0].Y)
            {
                Canvas.SetLeft(rectangle, points[0].X);
                Canvas.SetTop(rectangle, points[0].Y);

                rectangle.Width = Math.Abs(points[1].X - this.points[0].X);
                rectangle.Height = Math.Abs(points[1].Y - this.points[0].Y);
            }

            if (points[1].X < this.points[0].X && points[1].Y < this.points[0].Y)
            {
                Canvas.SetLeft(rectangle, points[1].X);
                Canvas.SetTop(rectangle, points[1].Y);

                rectangle.Width = Math.Abs(this.points[0].X - points[1].X);
                rectangle.Height = Math.Abs(this.points[0].Y - points[1].Y);
            }

            if (points[1].X > this.points[0].X && points[1].Y < this.points[0].Y)
            {
                Canvas.SetLeft(rectangle, this.points[0].X);
                Canvas.SetTop(rectangle, points[1].Y);

                rectangle.Width = Math.Abs(this.points[0].X - points[1].X);
                rectangle.Height = Math.Abs(this.points[0].Y - points[1].Y);
            }

            if (points[1].X < this.points[0].X && points[1].Y > this.points[0].Y)
            {
                Canvas.SetLeft(rectangle, points[1].X);
                Canvas.SetTop(rectangle, this.points[0].Y);

                rectangle.Width = Math.Abs(this.points[0].X - points[1].X);
                rectangle.Height = Math.Abs(this.points[0].Y - points[1].Y);
            }
        }

        private void Rotate(Point point)
        {
            RotateFigure.CenterX = Math.Abs(points[0].X - points[1].X) / 2;
            RotateFigure.CenterY = Math.Abs(points[0].Y - points[1].Y) / 2;

            RotateFigure.Angle = point.X - points[1].X;
        }

        public void CreateFigure(Point point)
        {
            points[0] = point;
            Canvas.SetLeft(rectangle, point.X);
            Canvas.SetTop(rectangle, point.Y);

            SelectObject(this);
        }

        public void DrawFigure(Point point)
        {
            throw new NotImplementedException();
        }
    }
}
