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
    class FigureRectangle : IFigure
    {
        public event EventSelectFigure SelectObject;
        public event EventClickMarker ClickMarker;
        public event EventTransform Transform;
        public event EventSetMarker SetMarker;
        public event EventRemoveMarker RemoveMarker;

        Canvas canvas;
        Rectangle rectangle;
        Rectangle marker;
        Thickness thickness;

        bool transform;
        bool turn;
        bool move;

        public FigureRectangle()
        {
            thickness = new();
            canvas = new();
            rectangle = new();
            marker = new();

            canvas.Background = Brushes.White;
            canvas.MouseLeftButtonDown += Canvas_MouseLeftButtonDown;
            canvas.Margin = thickness;

            rectangle.Margin = thickness;

            marker.Fill = Brushes.Red;
            marker.Width = 10;
            marker.Height = 10;

            canvas.Children.Add(rectangle);
            canvas.Children.Add(marker);
        }

        private void Canvas_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
        }

        public void ChangeColor(Color color)
        {
            throw new NotImplementedException();
        }

        public void ChangePosition(Point point)
        {
            throw new NotImplementedException();
        }

        public void ChangeThickness(double thick)
        {
        }

        public void CreateFigure(Point mouse)
        {
            canvas.Width = 1000;
            canvas.Height = 1000;

            RotateTransform rotate = new();
            rotate.Angle = 30;
            canvas.RenderTransform = rotate;

            thickness.Left = mouse.X;
            thickness.Top = mouse.Y;

            SelectObject(this);
        }

        public void DelMarker()
        {
            throw new NotImplementedException();
        }

        public void DrawFigure(Point point)
        {

        }

        public object Figure()
        {
            return canvas;
        }

        public void SetColor(Color color)
        {
            rectangle.Fill = new SolidColorBrush(color);
        }
    }
}
