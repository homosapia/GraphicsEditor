﻿using GraphicsEditor.Abstractions;
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

        public square CopyElements()
        {
            square objects = new();

            Rectangle rectangle = new();
            rectangle.Fill = Rectangle.Fill;
            rectangle.Width = Rectangle.Width;
            rectangle.Height = Rectangle.Height;

            objects.Rectangle = rectangle;

            Canvas substrate = new();
            substrate.Width = Substrate.Width;
            substrate.Height = Substrate.Height;
            substrate.RenderTransform = Substrate.RenderTransform;

            objects.Substrate = substrate;

            RotateTransform rotateTransform = new();
            rotateTransform.Angle = this.rotateTransform.Angle;
            rotateTransform.CenterX = this.rotateTransform.CenterX;
            rotateTransform.CenterY = this.rotateTransform.CenterY;

            objects.rotateTransform = rotateTransform;

            return objects;
        }

        public void InsertElements(List<object> objects)
        {
            Rectangle = (Rectangle)objects[0];
            
            Thickness thickness = new();
            thickness.Left = Rectangle.Width - 5;
            thickness.Top = Rectangle.Height - 5;
            Marker.Margin = thickness;

            Substrate = (Canvas)objects[1];
            rotateTransform = (RotateTransform)objects[2];
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
