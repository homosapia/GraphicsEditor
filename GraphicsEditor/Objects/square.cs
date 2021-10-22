using GraphicsEditor.Abstracts;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphicsEditor.Objects
{
    class square : SaveOrLoad
    {
        public Rectangle Rectangle = new();
        public Rectangle Marker = new();
        public Canvas Substrate = new();
        private RotateTransform rotateTransform = new();

        public square()
        {
            Marker.Width = 10;
            Marker.Height = 10;

            Substrate.Children.Add(Rectangle);
            Substrate.Children.Add(Marker);
            Substrate.RenderTransform = rotateTransform;
        }

        public override List<object> Save()
        {
            List<object> uIElements = new();
            Rectangle rectangle = new();
            rectangle.Fill = Rectangle.Fill;
            rectangle.Width = Rectangle.Width;
            rectangle.Height = Rectangle.Height;
            rectangle.StrokeThickness = Rectangle.StrokeThickness;

            Canvas substrate = new();
            substrate.Width = Substrate.Width;
            substrate.Height = Substrate.Height;

            RotateTransform rotateTransform = new();
            rotateTransform.Angle = this.rotateTransform.Angle;
            rotateTransform.CenterX = this.rotateTransform.CenterX;
            rotateTransform.CenterY = this.rotateTransform.CenterY;

            uIElements.Add(rectangle);
            uIElements.Add(substrate);
            uIElements.Add(rotateTransform);
            return uIElements;
        }

        public override void Losd(List<object> objects)
        {
            Rectangle = (Rectangle)objects[0];
            Substrate = (Canvas)objects[1];
            this.rotateTransform = (RotateTransform)objects[2];


            Rectangle Marker = new();
            Marker.Width = 10;
            Marker.Height = 10;
            Thickness thickness = new();
            thickness.Left = Rectangle.Width - 5;
            thickness.Top = Rectangle.Height - 5;
            Marker.Margin = thickness;
            this.Marker = Marker;

            Substrate.Children.Add(Rectangle);
            Substrate.Children.Add(this.Marker);
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
