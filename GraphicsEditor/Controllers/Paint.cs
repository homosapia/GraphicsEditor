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
    class Paint : IPaint
    {
        List<IFigure> figures = new();
        UIElement uiElement;

        Rectangle marker;
        public Paint()
        {
            marker = new Rectangle();
            marker.Stroke = Brushes.Red;
            marker.Width = 10;
            marker.Height = 10;
        }

        public void AppNewObject(Canvas canvas, IFigure figure)
        {
            figures.Add(figure);
            uiElement = (UIElement)figure.NewObject();
            canvas.Children.Add(uiElement);
        }

        public UIElement GetCurrentItem()
        {
            return uiElement;
        }

        public object NewObject(IFigure obj)
        {
            return obj.NewObject();
        }

        public void StartObject(Point point)
        {
            figures.Last().StartObject(point);
        }

        public void NowObject(Point point)
        {
            figures.Last().EndObject(point);
            SetMarker(figures.Last());
        }

        public void EndObject(Point point)
        {
            figures.Last().EndObject(point);
        }

        public Point FindFigure(Point MousePoint)
        {
            foreach(var f in figures)
            {
                Point[] points = f.GetPointObject();

                if (Math.Abs(points[0].X - MousePoint.X) < 5 && (Math.Abs(points[0].Y - MousePoint.Y) < 5))
                {
                    return points[0];
                }

                if (Math.Abs(points[1].X - MousePoint.X) < 5 && (Math.Abs(points[1].Y - MousePoint.Y) < 5))
                {
                    return points[1];
                }
            }
            throw new ArgumentException("there is no object with such coordinates");
        }

        public void ShowMarker(Canvas canvas, Point figure)
        {
            Canvas.SetLeft(marker, figure.X-5);
            Canvas.SetTop(marker, figure.Y-5);
            canvas.Children.Add(marker);
        }

        public void SetMarker(IFigure figure)
        {
            if(figure.GetMarker() == null)
            {
                Point[] points = figure.GetPointObject();

                Rectangle[] marker = new Rectangle[points.Length];

                for (int i = 0; i < points.Length; i++)
                {
                    Rectangle rectangle = new();
                    rectangle.Name = "Marker";
                    rectangle.Stroke = Brushes.Red;
                    rectangle.Width = 10;
                    rectangle.Height = 10;
                    Canvas.SetLeft(rectangle, points[i].X - 5);
                    Canvas.SetTop(rectangle, points[i].Y - 5);
                    marker[i] = rectangle;
                }

                figure.SetMarker(marker);
            }
        }

        public void DelMarker(Canvas canvas)
        {
            for (int i = 0; i < canvas.Children.Count; i++)
            {
                Rectangle rectangle;
                try
                {
                    rectangle = (Rectangle)canvas.Children[i];

                    if (rectangle == marker)
                    {
                        canvas.Children.Remove(rectangle);
                    }
                }
                catch { continue; }
            }
        }
    }
}
