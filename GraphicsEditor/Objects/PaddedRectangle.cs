using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using GraphicsEditor.Abstracts;
using GraphicsEditor.Data;

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
        private Canvas padded = new();
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

        public RectangleDataToSave DataToSave()
        {
            RectangleDataToSave rectangleData = new RectangleDataToSave();

            rectangleData.colorA = color.A;
            rectangleData.colorR = color.R;
            rectangleData.colorG = color.G;
            rectangleData.colorB = color.B;

            rectangleData.Width = rectangle.Width;
            rectangleData.Height = rectangle.Height;

            rectangleData.Rotate = rotateTransform.Angle;

            return rectangleData;
        }

        public void FillWithData(RectangleDataToSave rectangleData)
        {
            color.A = rectangleData.colorA;
            color.R = rectangleData.colorR;
            color.G = rectangleData.colorG;
            color.B = rectangleData.colorB;

            rectangle.Width = rectangleData.Width;
            rectangle.Height = rectangleData.Height;

            padded.Width = rectangle.Width + 10;
            padded.Height = rectangle.Height + 10;

            rotateTransform.Angle = rectangleData.Rotate;
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

            padded.Children.Add(rectangle);
            padded.Children.Add(marker);
            padded.RenderTransform = rotateTransform;
        }

        public void Resize(double Widith, double Height)
        {
            rectangle.Width = Widith;
            rectangle.Height = Height;

            Thickness thickness = new();
            thickness.Left = Widith - 5;
            thickness.Top = Height - 5;
            marker.Margin = thickness;

            padded.Width = Widith;
            padded.Height = Height;
        }

        public void Rotate(double rotat)
        {
            rotateTransform.CenterX = rectangle.Width / 2;
            rotateTransform.CenterY = rectangle.Height / 2;
            rotateTransform.Angle = rotat;
        }

        public void SetPosition(Point point)
        {
            Canvas.SetLeft(padded, point.X);
            Canvas.SetTop(padded, point.Y);
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
            return padded;
        }
    }
}
