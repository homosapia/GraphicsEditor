using GraphicsEditor.Abstractions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphicsEditor.Objects
{
    class square
    {
        public Rectangle Rectangle = new();
        public Rectangle Marker = new();
        public Canvas Substrate = new();
        private RotateTransform rotateTransform = new();
        private Color color;

        public List<object> CopyElements()
        {
            List<object> objects = new();

            objects.Add(color.A);
            objects.Add(color.R);
            objects.Add(color.G);
            objects.Add(color.B);

            objects.Add(Rectangle.Width);
            objects.Add(Rectangle.Height);

            objects.Add(rotateTransform.Angle);

            return objects;
        }

        public void InsertElements(List<object> objects)
        {
            color.A = Byte.Parse(objects[0].ToString());
            color.R = Byte.Parse(objects[1].ToString());
            color.G = Byte.Parse(objects[2].ToString());
            color.B = Byte.Parse(objects[3].ToString());

            Rectangle.Width = Double.Parse(objects[4].ToString());
            Rectangle.Height = Double.Parse(objects[5].ToString());

            Substrate.Width = Rectangle.Width + 10;
            Substrate.Height = Rectangle.Height + 10;

            rotateTransform.Angle = Double.Parse(objects[6].ToString()); ;
            rotateTransform.CenterX = Rectangle.Width / 2;
            rotateTransform.CenterY = Rectangle.Height / 2;

            Rectangle.Fill = new SolidColorBrush(color);

            Thickness thickness = new();
            thickness.Left = Rectangle.Width - 5;
            thickness.Top = Rectangle.Height - 5;
            Marker.Margin = thickness;
        }

        public void CreateObject()
        {
            Marker.Width = 10;
            Marker.Height = 10;

            Substrate.Children.Add(Rectangle);
            Substrate.Children.Add(Marker);
            Substrate.RenderTransform = rotateTransform;
        }

        public Canvas GetRectangle()
        {
            return Substrate;
        }

        public void resize(Point point)
        {
            Rectangle.Width = point.X;
            Rectangle.Height = point.Y;

            Thickness thickness = new();
            thickness.Left = Rectangle.Width - 5;
            thickness.Top = Rectangle.Height - 5;
            Marker.Margin = thickness;

            Substrate.Width = Rectangle.Width + 10;
            Substrate.Height = Rectangle.Height + 10;
        }

        public void ChangeTurn(double rotat)
        {
            rotateTransform.CenterX = Rectangle.Width / 2;
            rotateTransform.CenterY = Rectangle.Height / 2;
            rotateTransform.Angle = rotat;
        }

        public void SetPosition(Point point)
        {
            Canvas.SetLeft(Substrate, point.X);
            Canvas.SetTop(Substrate, point.Y);
        }

        public void ChangeColor(Color color)
        {
            this.color = color;
            Rectangle.Fill = new SolidColorBrush(color);
        }

        public void ChangeThickness(double thick)
        {
            Rectangle.StrokeThickness = thick;
        }

        public void HideMarker()
        {
            Marker.Fill = new SolidColorBrush(Color.FromArgb(0,0,0,0));
        }

        public void ShowMarker()
        {
            Marker.Fill = Brushes.Red;
        }
    }
}
