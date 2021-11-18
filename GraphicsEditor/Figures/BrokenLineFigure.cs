using System.Collections.Generic;
using System.Linq;
using GraphicsEditor.Objects;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using GraphicsEditor.Abstracts;
using GraphicsEditor.Data;
using Newtonsoft.Json;

namespace GraphicsEditor
{
    public class BrokenLineFigure : IFigure
    {
        private const string figureType = "BrokenLineFigure";

        public event EventSelectFigure SelectFigure;
        public event EventRemoveUiElement RemoveUiElement;

        private readonly BrokenLine brokenLine = new();

        private List<Ellipse> markers = new();
        private Ellipse marker = new();
        private bool markerSelected;

        public FigureDataToSave GetDataToSave()
        {
            FigureDataToSave figureData = new();
            figureData.FigureJson = JsonConvert.SerializeObject(brokenLine.DataToSave());
            figureData.FigureType = figureType;
            return figureData;
        }

        public void FillWithData(FigureDataToSave data)
        {
            brokenLine.FillWithData(JsonConvert.DeserializeObject<BrokenLineDataToSave>(data.FigureJson));
            
            SignLinesToEvents();
        }

        private void Marker_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            markerSelected = false;
            List<UIElement> lines = brokenLine.GetLinesLess(5);
            if (lines.Count > 0)
            {
                marker = (Ellipse)sender;
                Point point = new(Canvas.GetLeft(marker) + 5, Canvas.GetTop(marker) + 5);

                RemoveUiElement(lines);

                SetMarkers();
                SelectFigure(this);
            }
        }

        private void Marker_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
            marker = (Ellipse)sender;
            markerSelected = true;
            Point point = new(Canvas.GetLeft(marker) + 5, Canvas.GetTop(marker) + 5);
            if (e.ClickCount == 1)
            {
                brokenLine.PointInRadius(point, 0);
            }
            SelectFigure(this);
        }

        private void Line_MouseLeftDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
            SetMarkers();
            markerSelected = false;
            if (e.ClickCount == 2)
            {
                Line line = (Line)sender;
                Point point = e.GetPosition(line);

                brokenLine.SplitTheLine(line, point);
                SignLinesToEvents();
                brokenLine.PointInRadius(point, 0);

                marker = CreateMarker(point);
                markers.Add(marker);
                markerSelected = true;
            }
            SelectFigure(this);
        }

        private void SetMarkers()
        {
            List<Point> points = brokenLine.GetConnectionPointsOfLines();

            RemoveSelection();

            markers = new();
            for (int i = 0; i < points.Count; i++)
            {
                markers.Add(CreateMarker(points[i]));
            }
        }

        public void ChangeToDelta(double deltaX, double deltaY)
        {
            if (markerSelected)
            {
                Point positionMarker = new(Canvas.GetLeft(marker) + deltaX, Canvas.GetTop(marker) + deltaY);
                Point positionLine = new(positionMarker.X + 5, positionMarker.Y + 5);
                
                brokenLine.ChangeLinePointPosition(positionLine);
                
                Canvas.SetLeft(marker, positionMarker.X);
                Canvas.SetTop(marker, positionMarker.Y);
            }
        }

        private void SignLinesToEvents()
        {
            List<Line> lines = brokenLine.GetLines();
            foreach (Line line in lines)
            {
                line.MouseLeftButtonDown += Line_MouseLeftDown;
            }
        }



        private Ellipse CreateMarker(Point point)
        {
            Ellipse marker = new();

            marker.Fill = Brushes.Red;

            marker.Width = 10;
            marker.Height = 10;

            marker.MouseLeftButtonDown += Marker_MouseLeftButtonDown;
            marker.MouseLeftButtonUp += Marker_MouseLeftButtonUp;

            Canvas.SetLeft(marker, point.X - 5);
            Canvas.SetTop(marker, point.Y - 5);

            return marker;
        }

        public void SetColor(Color color)
        {
            brokenLine.SetColor(color);
        }

        public void SetThickness(double thick)
        {
            brokenLine.SetThickness(thick);
        }

        public void RemoveSelection()
        {
            RemoveUiElement?.Invoke(new List<UIElement>(markers));
        }

        public void StartDrawing(Point point)
        {
            markerSelected = true;
            
            
            brokenLine.SetLine(point, point);
            
            SignLinesToEvents();
            
            markers.Add(CreateMarker(point));
            markers.Add(marker = CreateMarker(point));

            SelectFigure(this);
        }

        public void CanvasMouseLeftButtonDown()
        {
        }

        public List<UIElement> GetAllUIElements()
        {
            List<UIElement> uIElements = new();

            uIElements.AddRange(brokenLine.GetLines());
            uIElements.AddRange(markers);

            return uIElements;
        }

        public void MoveDistance(double deltaX, double deltaY)
        {
            brokenLine.MoveLines(deltaX, deltaY);
        }

        public void CanvasMouseLeftButtonUp()
        {
        }
    }
}
