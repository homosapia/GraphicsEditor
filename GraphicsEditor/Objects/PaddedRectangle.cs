using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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

        private readonly Rectangle rectangle = new();
        private readonly Rectangle marker = new();
        private readonly Canvas padded = new();
        private readonly RotateTransform rotateTransform = new();
        private Color color;

        public PaddedRectangle()
        {
            rectangle.MouseLeftButtonDown += Rectangle_MouseLeftButtonDown;
            rectangle.MouseLeftButtonUp += Rectangle_MouseLeftButtonUp;

            marker.MouseLeftButtonDown += Marker_MouseLeftButtonDown;
            marker.MouseLeftButtonUp += Marker_MouseLeftButtonUp;
        }

        private void Marker_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MarkerMouseUp();
        }

        private void Marker_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            MarkerMouseDown();
        }

        private void Rectangle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            RectangleMouseUp();
        }

        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            RectangleMouseDown();
        }

        public RectangleDataToSave DataToSave()
        {
            RectangleDataToSave rectangleData = new()
            {
                colorA = color.A,
                colorR = color.R,
                colorG = color.G,
                colorB = color.B,

                Width = rectangle.Width,
                Height = rectangle.Height,

                Rotate = rotateTransform.Angle
            };

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

        public void ConfigureAnRectangle()
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

        public void Rotate(double rotate)
        {
            rotateTransform.CenterX = rectangle.Width / 2;
            rotateTransform.CenterY = rectangle.Height / 2;
            rotateTransform.Angle += rotate;
        }

        public void SetPosition(Point point)
        {
            Canvas.SetLeft(padded, point.X);
            Canvas.SetTop(padded, point.Y);
        }

        public void MoveDistance(double deltaX, double deltaY)
        {
            Point positionPadded = new(Canvas.GetLeft(padded), Canvas.GetTop(padded));

            positionPadded.X += deltaX;
            positionPadded.Y += deltaY;

            Canvas.SetLeft(padded, positionPadded.X);
            Canvas.SetTop(padded, positionPadded.Y);
        }

        public void SetColor(Color color)
        {
            this.color = color;
            rectangle.Fill = new SolidColorBrush(color);
        }

        public void SetThickness(double thickness)
        {
            rectangle.StrokeThickness = thickness;
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
