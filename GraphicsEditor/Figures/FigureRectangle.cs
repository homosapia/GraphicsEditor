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
        public event EventSetMarker SetMarker;
        public event EventTransform Transform;
        public event EventSelectFigure SelectObject;
        public event EventRemoveMarker RemoveMarker;
        public event EventRemoveUiElemrnt RemoveUiElemrnt;

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
            distanceX = e.GetPosition(square.Rectangle).X - points[0].X;
            distanceY = e.GetPosition(square.Rectangle).Y - points[0].Y;

            pointY = e.GetPosition(square.Rectangle).Y;

            e.Handled = true;
            move = true;
            turn = false;
            transform = false;

            square.ShowMarker();

            Transform(true);
            ClickMarker(true);

            SelectObject(this);

        }
        private void Marker_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            transform = false;
            ClickMarker(transform);
        }

        private void Marker_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            transform = true;
            move = false;
            turn = false;
            ClickMarker(transform);
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
                square.HideMarker();
                points[0].X = point.X - distanceX;
                points[0].Y = point.Y - distanceY;
                square.SetPosition(points[0]);
            }
        }

        public void CreateFigure(Point point)
        {
            points[0] = point;

            square.SetPosition(point);

            SelectObject(this);
        }

        public void DrawFigure(Point point)
        {
            Point pointSubstrate = Mouse.GetPosition(square.Substrate);
            square.resize(pointSubstrate);
        }

        public void ChangeColor(Color color)
        {
            square.ChangeColor(color);
        }

        public void ChangeThickness(double thick)
        {
            square.ChangeThickness(thick);
        }
    }
}
