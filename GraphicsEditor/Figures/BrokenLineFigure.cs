using System.Collections.Generic;
using GraphicsEditor.Objects;
using System.Windows;
using System.Windows.Controls;
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

        public event EventFigureGive FigureGive;
        public event EventRemoveUiElement RemoveUIElement;

        private readonly BrokenLine brokenLine = new();

        private List<UIElement> markers = new();
        private UIElement marker = new();
        private bool markerSelected;
        private Color color = Color.FromArgb(255, 0, 0, 0);
        private double thick;

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

                RemoveUIElement(lines);
                SetMarkers();
                FigureGive(this);
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
            FigureGive(this);
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
            FigureGive(this);
        }

        private void SetMarkers()
        {
            List<Point> points = brokenLine.GetConnectionPointsOfLines();

            DeselectShape();

            markers = new();
            for (int i = 0; i < points.Count; i++)
            {
                markers.Add(CreateMarker(points[i]));
            }
        }

        public void Change(Point point)
        {
            if (markerSelected)
            {
                brokenLine.ChangeLinePointPosition(point);
                Canvas.SetLeft(marker, point.X - 5);
                Canvas.SetTop(marker, point.Y - 5);
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

        public void ChangeColor(Color color)
        {
            this.color = color;
            brokenLine.ChangeColor(color);
        }

        public void ChangeThickness(double thick)
        {
            this.thick = thick;
            brokenLine.ChangeThickness(thick);
        }

        public void DeselectShape()
        {
            RemoveUIElement(markers);
        }

        public void StartDrawing(Point point)
        {
            markerSelected = true;

            Line line = new();
            line.Stroke = new SolidColorBrush(color);
            line.StrokeThickness = thick;

            line.X1 = point.X;
            line.Y1 = point.Y;

            line.X2 = point.X;
            line.Y2 = point.Y;
            
            brokenLine.SetLine(line);
            
            SignLinesToEvents();
            
            markers.Add(CreateMarker(point));
            markers.Add(marker = CreateMarker(point));

            FigureGive(this);
        }

        public void StartMoving(Point point)
        {
            brokenLine.SetСlickPoint(point);
        }

        public List<UIElement> GetAllUIElements()
        {
            List<UIElement> uIElements = new();

            uIElements.AddRange(brokenLine.GetLines());
            uIElements.AddRange(markers);

            return uIElements;
        }

        public void MoveFigure(Point point)
        {
            try
            {
                brokenLine.MoveLines(point);
            }
            catch { }
        }
    }
}
