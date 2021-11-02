using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphicsEditor.Objects
{
    public delegate void EventRectangleMouseDown();
    public delegate void EventRectangleMouseUp();
    public delegate void EventMarkerMouseDown();
    public delegate void EventMarkerMouseUp();

    class PaddedRectangle
    {
        public event EventRectangleMouseDown RectangleMouseDown;
        public event EventRectangleMouseUp RectangleMouseUp;
        public event EventMarkerMouseDown MarkerMouseDown;
        public event EventMarkerMouseUp MarkerMouseUp;

        private Rectangle rectangle = new();
        private Rectangle marker = new();
        private Canvas substrate = new();
        private RotateTransform rotateTransform = new();
        private Color color;

        public PaddedRectangle()
        {
            rectangle.MouseLeftButtonDown += Rectangle_MouseLeftButtonDown;
            rectangle.MouseLeftButtonUp += Rectangle_MouseLeftButtonUp;

            marker.MouseLeftButtonDown += Marker_MouseLeftButtonDown;
            marker.MouseLeftButtonUp += Marker_MouseLeftButtonUp;
        }

        private void Marker_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MarkerMouseUp();
        }

        private void Marker_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
            MarkerMouseDown();
        }

        private void Rectangle_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            RectangleMouseUp();
        }

        private void Rectangle_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
            RectangleMouseDown();
        }

        public List<object> DataToSave()
        {
            List<object> objects = new();

            objects.Add(color.A);
            objects.Add(color.R);
            objects.Add(color.G);
            objects.Add(color.B);

            objects.Add(rectangle.Width);
            objects.Add(rectangle.Height);

            objects.Add(rotateTransform.Angle);

            return objects;
        }

        public void LoadData(List<object> objects)
        {
            color.A = Byte.Parse(objects[0].ToString());
            color.R = Byte.Parse(objects[1].ToString());
            color.G = Byte.Parse(objects[2].ToString());
            color.B = Byte.Parse(objects[3].ToString());

            rectangle.Width = Double.Parse(objects[4].ToString());
            rectangle.Height = Double.Parse(objects[5].ToString());

            substrate.Width = rectangle.Width + 10;
            substrate.Height = rectangle.Height + 10;

            rotateTransform.Angle = Double.Parse(objects[6].ToString()); ;
            rotateTransform.CenterX = rectangle.Width / 2;
            rotateTransform.CenterY = rectangle.Height / 2;

            rectangle.Fill = new SolidColorBrush(color);

            Thickness thickness = new();
            thickness.Left = rectangle.Width - 5;
            thickness.Top = rectangle.Height - 5;
            marker.Margin = thickness;
        }

        public void ConfigureAnObject()
        {
            marker.Width = 10;
            marker.Height = 10;

            substrate.Children.Add(rectangle);
            substrate.Children.Add(marker);
            substrate.RenderTransform = rotateTransform;
        }

        public void Resize(double Widith, double Height)
        {
            rectangle.Width = Widith;
            rectangle.Height = Height;

            Thickness thickness = new();
            thickness.Left = Widith - 5;
            thickness.Top = Height - 5;
            marker.Margin = thickness;

            substrate.Width = Widith;
            substrate.Height = Height;
        }

        public void ChangeTurn(double rotat)
        {
            rotateTransform.CenterX = rectangle.Width / 2;
            rotateTransform.CenterY = rectangle.Height / 2;
            rotateTransform.Angle = rotat;
        }

        public void SetPosition(Point point)
        {
            Canvas.SetLeft(substrate, point.X);
            Canvas.SetTop(substrate, point.Y);
        }

        public void ChangeColor(Color color)
        {
            this.color = color;
            rectangle.Fill = new SolidColorBrush(color);
        }

        public void ChangeThickness(double thick)
        {
            rectangle.StrokeThickness = thick;
        }

        public void HideMarker()
        {
            marker.Fill = null;
        }

        public void ShowMarker()
        {
            marker.Fill = Brushes.Red;
        }

        public Canvas GetRectangle()
        {
            return substrate;
        }
    }
}
