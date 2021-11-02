using System;
using System.Collections.Generic;
using GraphicsEditor.Objects;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using GraphicsEditor.Data;

namespace GraphicsEditor
{
    public class FigurePaddedRectangle : IFigure
    {
        public event EventSelectFigure SelectObject;
        public event EventRemoveUiElemrnt RemoveUIElemrnt;
        public event EventFindPositionMouse FindPositionMouse;

        private PaddedRectangle paddedRectangle = new();

        private Point startObject = new Point();

        double distanceX;
        double distanceY;
        double pointY;

        bool transform;
        bool move;

        public FigurePaddedRectangle()
        {
            transform = true;
            paddedRectangle.RectangleMouseDown += Rectangle_MouseLeftButtonDown;
            paddedRectangle.RectangleMouseUp += Rectangle_MouseLeftButtonUp;

            paddedRectangle.MarkerMouseDown += Marker_MouseLeftButtonDown;
            paddedRectangle.MarkerMouseUp += Marker_MouseLeftButtonUp;
        }

        public ListOfDataToSave SerializeFigure()
        {
            ListOfDataToSave data = new();

            data.Objects = paddedRectangle.DataToSave();
            data.point = startObject;

            return data;
        }

        public void DeserializeFigure(ListOfDataToSave data)
        {
            paddedRectangle.LoadData(data.Objects);

            startObject = data.point;

            paddedRectangle.ConfigureAnObject();
            paddedRectangle.SetPosition(startObject);

            paddedRectangle.RectangleMouseDown += Rectangle_MouseLeftButtonDown;
            paddedRectangle.RectangleMouseUp += Rectangle_MouseLeftButtonUp;

            paddedRectangle.MarkerMouseDown += Marker_MouseLeftButtonDown;
            paddedRectangle.MarkerMouseUp += Marker_MouseLeftButtonUp;
        }

        private void Rectangle_MouseLeftButtonUp()
        {
            transform = false;
            move = false;
        }

        private void Rectangle_MouseLeftButtonDown()
        {
            move = true;
            transform = false;

            paddedRectangle.ShowMarker();

            SelectObject(this);
            FindPositionMouse();
        }

        private void Marker_MouseLeftButtonUp()
        {
            transform = false;
            move = false;
        }

        private void Marker_MouseLeftButtonDown()
        {
            transform = true;
            move = false;
        }

        public void Change(Point point)
        {
            if (transform)
            {
                Point positionMouseOnSubstrate = Mouse.GetPosition(paddedRectangle.GetRectangle());
                if (positionMouseOnSubstrate.X > 0 && positionMouseOnSubstrate.Y > 0)
                {
                    paddedRectangle.Resize(Math.Abs(positionMouseOnSubstrate.X), Math.Abs(positionMouseOnSubstrate.Y));
                }
            }
            if(move)
            {
                MoveFigure(point);
            }
            if (!transform && !move)
            {
                paddedRectangle.HideMarker();
                double rotat = point.Y - pointY;
                paddedRectangle.ChangeTurn(rotat);
            }
        }

        public void ChangeColor(Color color)
        {
            paddedRectangle.ChangeColor(color);
        }

        public void ChangeThickness(double thick)
        {
            paddedRectangle.ChangeThickness(thick);
        }

        public void DeselectShape()
        {
            paddedRectangle.HideMarker();
            transform = false;
            move = false;
        }

        public void StartingPoint(Point point)
        {
            startObject = point;
            paddedRectangle.ConfigureAnObject ();
            paddedRectangle.SetPosition(point);
            SelectObject(this);
        }

        public void MousePositionOnCanvas(Point point)
        {
            distanceX = point.X - startObject.X;
            distanceY = point.Y - startObject.Y;

            pointY = point.Y;
        }

        public List<UIElement> GetAllUIElements()
        {
            List<UIElement> uIElements = new();
            uIElements.Add(paddedRectangle.GetRectangle());
            return uIElements;
        }

        public void MoveFigure(Point point)
        {
            startObject.X = point.X - distanceX;
            startObject.Y = point.Y - distanceY;
            paddedRectangle.SetPosition(startObject);
        }
    }
}
