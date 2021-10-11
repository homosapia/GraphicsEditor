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
        private Rectangle rectangle;
        Point [] point = new Point[2];

        List<Rectangle> marker;

        public object NewObject()
        {
            rectangle = new();
            rectangle.Fill = new SolidColorBrush(Colors.Black);
            return rectangle;
        }

        public object Figure()
        {
            return rectangle;
        }

        public void StartObject(Point point)
        {
            this.point[0] = point;
            Canvas.SetLeft(rectangle, point.X);
            Canvas.SetTop(rectangle, point.Y);
        }

        public void EndObject(Point point)
        {
            this.point[1] = point;
            if (point.X > this.point[0].X && point.Y > this.point[0].Y)
            {
                rectangle.Width = Math.Abs(point.X - this.point[0].X);
                rectangle.Height = Math.Abs(point.Y - this.point[0].Y);
            }

            if (point.X < this.point[0].X && point.Y < this.point[0].Y)
            {
                Canvas.SetLeft(rectangle, point.X);
                Canvas.SetTop(rectangle, point.Y);

                rectangle.Width = Math.Abs(this.point[0].X - point.X);
                rectangle.Height = Math.Abs(this.point[0].Y - point.Y);
            }

            if (point.X > this.point[0].X && point.Y < this.point[0].Y)
            {
                Canvas.SetLeft(rectangle, this.point[0].X);
                Canvas.SetTop(rectangle, point.Y);

                rectangle.Width = Math.Abs(this.point[0].X - point.X);
                rectangle.Height = Math.Abs(this.point[0].Y - point.Y);
            }

            if (point.X < this.point[0].X && point.Y > this.point[0].Y)
            {
                Canvas.SetLeft(rectangle, point.X);
                Canvas.SetTop(rectangle, this.point[0].Y);

                rectangle.Width = Math.Abs(this.point[0].X - point.X);
                rectangle.Height = Math.Abs(this.point[0].Y - point.Y);
            }
        }

        public Point[] GetPointObject()
        {
            return point;
        }

        public void SetMarker(Rectangle[] marker)
        {
            this.marker = new();
            foreach (var m in marker)
            {
                this.marker.Add(m);
            }
        }

        public List<Rectangle> GetMarker()
        {
            return marker;
        }

        public void DelMarker()
        {
            marker.Clear();
        }
    }
}
