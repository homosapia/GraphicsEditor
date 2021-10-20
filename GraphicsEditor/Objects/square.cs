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

        public square()
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
