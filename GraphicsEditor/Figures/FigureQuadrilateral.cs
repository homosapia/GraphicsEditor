using System;
using System.Collections.Generic;
using GraphicsEditor.Objects;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using GraphicsEditor.Data;

namespace GraphicsEditor
{
    public class FigureQuadrilateral : IFigure
    {
        public event EventSelectFigure SelectObject;
        public event EventRemoveUiElemrnt RemoveUIElemrnt;
        public event EventFindPositionMouse FindPositionMouse;

        private Quadrilateral rectangle = new();

        private Point startObject = new Point();

        double distanceX;
        double distanceY;
        double pointY;

        bool transform;
        bool turn;
        bool move;

        public FigureQuadrilateral()
        {
            transform = true;
            rectangle.RectangleMouseDown += Rectangle_MouseLeftButtonDown;
            rectangle.RectangleMouseUp += Rectangle_MouseLeftButtonUp;

            rectangle.MarkerMouseDown += Marker_MouseLeftButtonDown;
            rectangle.MarkerMouseUp += Marker_MouseLeftButtonUp;
        }

        public ListOfDataToSave SerializeFigure()
        {
            ListOfDataToSave data = new();

            data.Objects = rectangle.DataToSave();
            data.point = startObject;

            return data;
        }

        public void DeserializeFigure(ListOfDataToSave data)
        {
            rectangle.LoadData(data.Objects);

            startObject = data.point;

            rectangle.ConfigureAnObject();
            rectangle.SetPosition(startObject);

            rectangle.RectangleMouseDown += Rectangle_MouseLeftButtonDown;
            rectangle.RectangleMouseUp += Rectangle_MouseLeftButtonUp;

            rectangle.MarkerMouseDown += Marker_MouseLeftButtonDown;
            rectangle.MarkerMouseUp += Marker_MouseLeftButtonUp;
        }

        private void Rectangle_MouseLeftButtonUp()
        {
            turn = false;
            transform = false;
            move = false;
        }

        private void Rectangle_MouseLeftButtonDown()
        {
            move = true;
            turn = false;
            transform = false;

            rectangle.ShowMarker();

            SelectObject(this);
            FindPositionMouse();
        }

        private void Marker_MouseLeftButtonUp()
        {
            transform = false;
            move = false;
            turn = false;
        }

        private void Marker_MouseLeftButtonDown()
        {
            transform = true;
            move = false;
            turn = false;
        }

        public void Change(Point point)
        {
            if (transform)
            {
                Point positionMouseOnSubstrate = Mouse.GetPosition(rectangle.GetRectangle());
                if (positionMouseOnSubstrate.X > 0 && positionMouseOnSubstrate.Y > 0)
                {
                    rectangle.Resize(Math.Abs(positionMouseOnSubstrate.X), Math.Abs(positionMouseOnSubstrate.Y));
                }
            }
            if(move)
            {
                MoveFigure(point);
            }
            if (!transform && !move)
            {
                rectangle.HideMarker();
                double rotat = point.Y - pointY;
                rectangle.ChangeTurn(rotat);
            }
        }

        public void ChangeColor(Color color)
        {
            rectangle.ChangeColor(color);
        }

        public void ChangeThickness(double thick)
        {
            rectangle.ChangeThickness(thick);
        }

        public void DeselectShape()
        {
            rectangle.HideMarker();
            transform = false;
            move = false;
            turn = false;
        }

        public void StartingPoint(Point point)
        {
            startObject = point;
            rectangle.ConfigureAnObject ();
            rectangle.SetPosition(point);
            SelectObject(this);
        }

        public void CurrentPositionMouseOnCanvas(Point point)
        {
            distanceX = point.X - startObject.X;
            distanceY = point.Y - startObject.Y;

            pointY = point.Y;
        }

        public List<UIElement> GetAllUIElements()
        {
            List<UIElement> uIElements = new();
            uIElements.Add(rectangle.GetRectangle());
            return uIElements;
        }

        public void MoveFigure(Point point)
        {
            startObject.X = point.X - distanceX;
            startObject.Y = point.Y - distanceY;
            rectangle.SetPosition(startObject);
        }
    }
}
