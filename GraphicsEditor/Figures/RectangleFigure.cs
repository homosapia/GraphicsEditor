using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using GraphicsEditor.Objects;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using GraphicsEditor.Abstracts;
using GraphicsEditor.Data;
using Newtonsoft.Json;

namespace GraphicsEditor
{
    public class RectangleFigure : IFigure
    {
        private const string figureType = "RectangleFigure";

        public event EventFigureGive FigureGive;
        public event EventRemoveUiElement RemoveUIElement;

        private PaddedRectangle paddedRectangle = new();

        private Point previousMouse = new Point();

        double distanceX;
        double distanceY;
        double pointY;

        bool transform;
        bool move;

        public RectangleFigure()
        {
            transform = true;
            paddedRectangle.RectangleMouseDown += Rectangle_MouseLeftButtonDown;
            paddedRectangle.RectangleMouseUp += Rectangle_MouseLeftButtonUp;

            paddedRectangle.MarkerMouseDown += Marker_MouseLeftButtonDown;
            paddedRectangle.MarkerMouseUp += Marker_MouseLeftButtonUp;
        }

        public FigureDataToSave GetDataToSave()
        {
            FigureDataToSave figureData = new();

            RectangleDataToSave rectangleData = paddedRectangle.DataToSave();
            rectangleData.position = previousMouse;
            
            figureData.FigureJson = JsonConvert.SerializeObject(rectangleData);
            figureData.FigureType = figureType;
            return figureData;
        }

        public void FillWithData(FigureDataToSave data)
        {
            RectangleDataToSave rectangleData = JsonConvert.DeserializeObject<RectangleDataToSave>(data.FigureJson);
            paddedRectangle.FillWithData(rectangleData);

            previousMouse = rectangleData.position;

            paddedRectangle.ConfigureAnObject();
            paddedRectangle.SetPosition(previousMouse);

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

            FigureGive(this);
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
            else if(move)
            {
                MoveFigure(point);
            }
            else
            {
                paddedRectangle.HideMarker();
                double rotat = point.Y - pointY;
                paddedRectangle.Rotate(rotat);
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

        public void StartDrawing(Point point)
        {
            previousMouse = point;
            paddedRectangle.ConfigureAnObject ();
            paddedRectangle.SetPosition(point);
            FigureGive(this);
        }

        public void StartMoving(Point point)
        {
            distanceX = point.X - previousMouse.X;
            distanceY = point.Y - previousMouse.Y;

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
            previousMouse.X = point.X - distanceX;
            previousMouse.Y = point.Y - distanceY;
            paddedRectangle.SetPosition(previousMouse);
        }
    }
}
