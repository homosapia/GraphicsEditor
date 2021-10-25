using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphicsEditor.Objects;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Text.Json;
using Newtonsoft.Json;

namespace GraphicsEditor
{
    class FigureBrokenLine : IFigure
    {
        public event EventSelectFigure SelectObject;
        public event EventTransform Transform;
        public event EventClickMarker ClickMarker;
        public event EventSetUIElement UIElement;
        public event EventRemoveUiElemrnt RemoveUiElemrnt;
        public event EventFindPositionMouse FindPositionMouse;

        public BrokenLine brokenLine = new();

        List<UIElement> markers = new();
        UIElement marker = new();
        bool СellMarker = true;

        public string SerializeFigure()
        {
            List<string> objects = new();

            objects.Add(JsonConvert.SerializeObject(brokenLine.CopyElements()));

            return JsonConvert.SerializeObject(objects);
        }

        public void DeserializeFigure(List<string> objects)
        {
            brokenLine.InsertElements(JsonConvert.DeserializeObject<List<object>>(objects[0]));
        }

        public void TuneElements()
        {
            SignLinesToEvents();
        }

        public IFigure GetCopyIFigure()
        {
            FigureBrokenLine brokenLine = new();
            return brokenLine;
        }

        private void Marker_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            СellMarker = false;
        }

        private void Marker_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
            marker = (Ellipse)sender;
            СellMarker = true;
            Point point = new(Canvas.GetLeft(marker) + 5, Canvas.GetTop(marker) + 5);
            if (e.ClickCount == 1)
            {
                brokenLine.FindThePointsOfTheLinesInTheRadius(point, 0);
            }

            if (e.ClickCount == 2)
            {
                RemoveLine();
                brokenLine.FindThePointsOfTheLinesInTheRadius(point, 5);
                brokenLine.ChangeLinePointPosition(point);
                SetMarkers();
            }
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
            UIElement(markers);
        }

        private void RemoveLine()
        {
            List<Line> lines = brokenLine.GetLinesLess(5);
            List<UIElement> del = new();
            foreach (UIElement ui in lines)
            {
                del.Add(ui);
            }
            RemoveUiElemrnt(del);
        }

        public void ChangePosition(Point point)
        {
            if (СellMarker)
            {
                brokenLine.ChangeLinePointPosition(point);
                SignLinesToEvents();
                Canvas.SetLeft(marker, point.X - 5);
                Canvas.SetTop(marker, point.Y - 5);
            }
        }

        public void CreateFigure()
        {
            SelectObject(this);
        }

        private void SignLinesToEvents()
        {
            List<Line> lines = brokenLine.GetLines();
            foreach (Line line in lines)
            {
                line.MouseLeftButtonDown += Line_MouseLeftDown;
            }
        }

        private void Line_MouseLeftDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
            if (e.ClickCount == 1)
            {
                Transform(true);
            }

            if (e.ClickCount == 2)
            {
                Line line = (Line)sender;
                Point point = e.GetPosition(line);

                brokenLine.SplitTheLine(line, point);
                SignLinesToEvents();
            }
            SetMarkers();
            SelectObject(this);
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

        public object Figure()
        {
            return brokenLine.GetLines().Last();
        }

        public void ChangeColor(Color color)
        {
            brokenLine.ChangeColor(color);
        }

        public void ChangeThickness(double thick)
        {
            brokenLine.ChangeThickness(thick);
        }

        public void DeselectShape()
        {
            RemoveUiElemrnt(markers);
        }

        public void StartingPoint(Point point)
        {
            brokenLine.SetLine(point, point);
            SelectObject(this);
        }

        public void CurrentPositionMouseOnCanvas(Point point)
        {
            brokenLine.CalculateDistanceBetweenLinePointAndClick(point);
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
            brokenLine.MoveLine(point);
        }
    }
}
