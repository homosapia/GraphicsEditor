using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using GraphicsEditor.Objects;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
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

        private Point previousMouse = new();

        private readonly Rectangle rectangle = new();
        private readonly Rectangle marker = new();
        private readonly Canvas padded = new();
        private readonly RotateTransform rotateTransform = new();
        private Color color;

        private bool transform;
        private bool move;
        private bool rotate;

        public RectangleFigure()
        {
            transform = true;
            rectangle.MouseLeftButtonDown += Rectangle_MouseLeftButtonDown;
            rectangle.MouseLeftButtonUp += Rectangle_MouseLeftButtonUp;

            marker.MouseLeftButtonDown += Marker_MouseLeftButtonDown;
            marker.MouseLeftButtonUp += Marker_MouseLeftButtonUp;
        }

        public FigureDataToSave GetDataToSave()
        {
            FigureDataToSave figureData = new();


            RectangleDataToSave rectangleData = new()
            {
                colorA = color.A,
                colorR = color.R,
                colorG = color.G,
                colorB = color.B,

                Width = rectangle.Width,
                Height = rectangle.Height,

                Rotate = rotateTransform.Angle
            };

            rectangleData.position = previousMouse;
            
            figureData.FigureJson = JsonConvert.SerializeObject(rectangleData);
            figureData.FigureType = DataResources.RectangleFigure;
            return figureData;
        }

        public void FillWithData(FigureDataToSave data)
        {
            RectangleDataToSave rectangleData = JsonConvert.DeserializeObject<RectangleDataToSave>(data.FigureJson);
            
            color.A = rectangleData.colorA;
            color.R = rectangleData.colorR;
            color.G = rectangleData.colorG;
            color.B = rectangleData.colorB;

            rectangle.Width = rectangleData.Width;
            rectangle.Height = rectangleData.Height;

            padded.Width = rectangle.Width + 10;
            padded.Height = rectangle.Height + 10;

            rotateTransform.Angle = rectangleData.Rotate;
            rotateTransform.CenterX = rectangle.Width / 2;
            rotateTransform.CenterY = rectangle.Height / 2;

            rectangle.Fill = new SolidColorBrush(color);

            Thickness thickness = new();
            thickness.Left = rectangle.Width - 5;
            thickness.Top = rectangle.Height - 5;
            marker.Margin = thickness;

            previousMouse = rectangleData.position;

            ConfigureAnRectangle();
            SetPosition(previousMouse);

            rectangle.MouseLeftButtonDown += Rectangle_MouseLeftButtonDown;
            rectangle.MouseLeftButtonUp += Rectangle_MouseLeftButtonUp;

            rectangle.MouseLeftButtonDown += Marker_MouseLeftButtonDown;
            rectangle.MouseLeftButtonUp += Marker_MouseLeftButtonUp;
        }

        private void ConfigureAnRectangle()
        {
            marker.Width = 10;
            marker.Height = 10;

            padded.Children.Add(rectangle);
            padded.Children.Add(marker);
            padded.RenderTransform = rotateTransform;
        }

        private void Rectangle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            transform = false;
            move = false;
        }

        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;

            move = true;
            transform = false;
            rotate = false;
            
            marker.Fill = Brushes.Red;

            SelectFigure(this);
        }

        private void Marker_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            transform = false;
            move = false;
        }

        private void Marker_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;

            transform = true;
            move = false;
            rotate = false;
        }

        public void ChangeToDelta(double deltaX, double deltaY)
        {
            if (transform)
            {
                Point positionMouseOnSubstrate = Mouse.GetPosition(padded);
                if (positionMouseOnSubstrate.X > 0 && positionMouseOnSubstrate.Y > 0)
                {
                    Resize(Math.Abs(positionMouseOnSubstrate.X), Math.Abs(positionMouseOnSubstrate.Y));
                }
            }
            else if(move)
            {
                MoveDistance(deltaX, deltaY);
            }
            else if(rotate)
            {
                HideMarker();
                Rotate(deltaY);
            }
        }

        private void Rotate(double rotate)
        {
            rotateTransform.CenterX = rectangle.Width / 2;
            rotateTransform.CenterY = rectangle.Height / 2;
            rotateTransform.Angle += rotate;
        }

        public void SetColor(Color color)
        {
            this.color = color;
            rectangle.Fill = new SolidColorBrush(color);
        }

        public void SetThickness(double thick)
        {
            rectangle.StrokeThickness = thick;
        }

        public void RemoveSelection()
        {
            HideMarker();
            transform = false;
            move = false;
        }

        public void StartDrawing(Point point)
        {
            previousMouse = point;
            ConfigureAnRectangle();
            SetPosition(point);
            SelectFigure(this);
        }

        public void CanvasMouseLeftButtonDown()
        {
            rotate = true;
        }

        public List<UIElement> GetAllUIElements()
        {
            List<UIElement> uIElements = new();
            uIElements.Add(padded);
            return uIElements;
        }

        public void MoveDistance(double deltaX, double deltaY)
        {
            Point positionPadded = new(Canvas.GetLeft(padded), Canvas.GetTop(padded));

            positionPadded.X += deltaX;
            positionPadded.Y += deltaY;

            Canvas.SetLeft(padded, positionPadded.X);
            Canvas.SetTop(padded, positionPadded.Y);
        }

        public void CanvasMouseLeftButtonUp()
        {
            transform = false;
            rotate = false;
            move = false;
        }

        private void SetPosition(Point point)
        {
            Canvas.SetLeft(padded, point.X);
            Canvas.SetTop(padded, point.Y);
        }

        private void HideMarker()
        {
            marker.Fill = null;
        }

        public void Resize(double Widith, double Height)
        {
            rectangle.Width = Widith;
            rectangle.Height = Height;

            Thickness thickness = new();
            thickness.Left = Widith - 5;
            thickness.Top = Height - 5;
            marker.Margin = thickness;

            padded.Width = Widith;
            padded.Height = Height;
        }
    }
}
