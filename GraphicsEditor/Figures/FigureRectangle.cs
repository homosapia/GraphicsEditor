using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphicsEditor.Objects;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphicsEditor
{
    class FigureRectangle : IFigure
    {
        public event EventClickMarker ClickMarker;
        public event EventSetUIElement UIElement;
        public event EventTransform Transform;
        public event EventSelectFigure SelectObject;
        public event EventRemoveUiElemrnt RemoveUiElemrnt;
        public event EventFindPositionMouse FindPositionMouse;

        private square square = new();
        
        Point [] points = new Point[2];

        double distanceX;
        double distanceY;
        double pointY;

        bool transform;
        bool turn;
        bool move;

        public FigureRectangle()
        {
            transform = true;
            square.Rectangle.MouseLeftButtonDown += Rectangle_MouseLeftButtonDown;
            square.Rectangle.MouseLeave += Rectangle_MouseLeave;

            square.Marker.MouseLeftButtonDown += Marker_MouseLeftButtonDown;
            square.Marker.MouseLeftButtonUp += Marker_MouseLeftButtonUp;
        }

        private void Rectangle_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!transform)
            {
                turn = true;
                transform = false;
                move = false;
            }
        }

        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FindPositionMouse();

            e.Handled = true;
            move = true;
            turn = false;
            transform = false;

            square.ShowMarker();

            Transform(true);

            SelectObject(this);

        }
        private void Marker_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            transform = false;
        }

        private void Marker_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            transform = true;
            move = false;
            turn = false;
        }

        public object NewObject(Canvas canvas)
        {
            return canvas;
        }

        public object Figure()
        {
            return square.Substrate;
        }

        public void ChangePosition(Point point)
        {
            if (transform)
            {
                Point pointSubstrate = Mouse.GetPosition(square.Substrate);
                square.resize(pointSubstrate);
            }
            if(turn)
            {
                square.HideMarker();
                double rotat =  point.Y - pointY;
                square.ChangeTurn(rotat);
            }
            if(move)
            {
                points[0].X = point.X - distanceX;
                points[0].Y = point.Y - distanceY;
                square.SetPosition(points[0]);
            }
        }

        public void ChangeColor(Color color)
        {
            square.ChangeColor(color);
        }

        public void ChangeThickness(double thick)
        {
            square.ChangeThickness(thick);
        }

        public void DeselectShape()
        {
            square.HideMarker();
            transform = false;
            move = false;
            turn = false;
        }

        public void StartingPoint(Point point)
        {
            points[0] = point;
            square.SetPosition(point);
            SelectObject(this);
        }

        public void CurrentPositionMouseOnCanvas(Point point)
        {
            distanceX = point.X - points[0].X;
            distanceY = point.Y - points[0].Y;

            pointY = point.Y;
        }
    }
}
