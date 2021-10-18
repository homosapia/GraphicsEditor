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
            marker.Fill = Brushes.Red;
            marker.Width = 10;
            marker.Height = 10;

            grid.Children.Add(rectangle);
            grid.Children.Add(marker);
        }

        public void ChangePosition(Point point)
        {
            throw new NotImplementedException();
        }

        public void CreateFigure(Point mouse)
        {
            Canvas.SetLeft(grid, mouse.X);
            Canvas.SetTop(grid, mouse.Y);

            Canvas.SetRight(grid.Children[0], mouse.X);
            Canvas.SetBottom(grid.Children[0], mouse.Y);
            SelectObject(this);
        }

        public void DrawFigure(Point point)
        {

        }



        public object Figure()
        {
            return grid;
        }
        public void DelMarker()
        {
            throw new NotImplementedException();
        }
    }
}
