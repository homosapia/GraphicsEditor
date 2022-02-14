using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using GraphicsEditor.Objects;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using GraphicsEditor.Abstracts;
using GraphicsEditor.Data;
using GraphicsEditor.Interfaces;
using Newtonsoft.Json;
using GraphicsEditor.Resources;

namespace GraphicsEditor
{
    public class RectangleFigure : IFigure
    {
        public event EventSelectFigure SelectFigure;

        private readonly PaddedRectangle paddedRectangle = new();
        private ParentContainer parentContainer;

        private Point previousMouse = new();

        private bool transform;
        private bool move;
        private bool rotate;

        public RectangleFigure()
        {
            transform = true;
            paddedRectangle.RectangleMouseDown += Rectangle_MouseLeftButtonDown;
            paddedRectangle.RectangleMouseUp += Rectangle_MouseLeftButtonUp;

            paddedRectangle.MarkerMouseDown += Marker_MouseLeftButtonDown;
            paddedRectangle.MarkerMouseUp += Marker_MouseLeftButtonUp;
        }

        public void SetParentContainer(ParentContainer parentContainer)
        {
            this.parentContainer = parentContainer;
        }

        public FigureDataToSave GetDataToSave()
        {
            FigureDataToSave figureData = new();

            RectangleDataToSave rectangleData = paddedRectangle.DataToSave();
            rectangleData.position = previousMouse;
            
            figureData.FigureJson = JsonConvert.SerializeObject(rectangleData);
            figureData.FigureType = DataResources.RectangleFigure;
            return figureData;
        }

        public void FillWithData(FigureDataToSave data)
        {
            RectangleDataToSave rectangleData = JsonConvert.DeserializeObject<RectangleDataToSave>(data.FigureJson);
            paddedRectangle.FillWithData(rectangleData);

            previousMouse = rectangleData.position;

            paddedRectangle.ConfigureAnRectangle();
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
            rotate = false;
            

            paddedRectangle.ShowMarker();

            SelectFigure(this);
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
            rotate = false;
        }

        public void ChangeToDelta(double deltaX, double deltaY)
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
                MoveDistance(deltaX, deltaY);
            }
            else if(rotate)
            {
                paddedRectangle.HideMarker();
                paddedRectangle.Rotate(deltaY);
            }
        }

        public void SetColor(Color color)
        {
            paddedRectangle.SetColor(color);
        }

        public void SetThickness(double thick)
        {
            paddedRectangle.SetThickness(thick);
        }

        public void RemoveSelection()
        {
            paddedRectangle.HideMarker();
            transform = false;
            move = false;
        }

        public void StartDrawing(Point point)
        {
            previousMouse = point;
            paddedRectangle.ConfigureAnRectangle();
            paddedRectangle.SetPosition(point);
            SelectFigure(this);
        }

        public void CanvasMouseLeftButtonDown()
        {
            rotate = true;
        }

        public List<UIElement> GetAllUIElements()
        {
            List<UIElement> uIElements = new();
            uIElements.Add(paddedRectangle.GetRectangle());
            return uIElements;
        }

        public void MoveDistance(double deltaX, double deltaY)
        {
            paddedRectangle.MoveDistance(deltaX, deltaY);
        }

        public void CanvasMouseLeftButtonUp()
        {
            transform = false;
            rotate = false;
            move = false;
        }
    }
}
