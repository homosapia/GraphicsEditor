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

        Grid grid;
        Rectangle rectangle;
        Rectangle marker;

        bool transform;
        bool turn;
        bool move;

        public FigureRectangle()
        {
            grid = new();
            rectangle = new();
            marker = new();

            rectangle.Fill = new SolidColorBrush(Colors.Black);
            rectangle.Width = 1;
            rectangle.Height = 1;

            marker.Fill = Brushes.Red;
            marker.Width = 10;
            marker.Height = 10;
        }

        public void ChangePosition(Point point)
        {
            throw new NotImplementedException();
        }

        public void CreateFigure(Point mouse)
        {
            Canvas.SetLeft(rectangle, mouse.X);
            Canvas.SetTop(rectangle, mouse.Y);
            SelectObject(this);
        }

        public void DrawFigure(Point point)
        {
            ScaleTransform scale = new();
            scale.ScaleX = point.X / 10;
            scale.ScaleY = point.Y / 10;
            rectangle.RenderTransform = scale;
        }



        public object Figure()
        {
            return rectangle;
        }
        public void DelMarker()
        {
            throw new NotImplementedException();
        }
    }
}
