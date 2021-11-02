using System.Collections.Generic;
using GraphicsEditor.Objects;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using GraphicsEditor.Data;

namespace GraphicsEditor
{
    public class FigureBrokenLine : IFigure
    {
        public event EventSelectFigure SelectObject;
        public event EventRemoveUiElemrnt RemoveUIElemrnt;
        public event EventFindPositionMouse FindPositionMouse;

        private BrokenLine brokenLine = new();

        private List<UIElement> markers = new();
        private UIElement marker = new();
        private bool СellMarker;

        public ListOfDataToSave SerializeFigure()
        {
            ListOfDataToSave data = new();

            data.Objects = brokenLine.DataToSave();

            return data;
        }

        public void DeserializeFigure(ListOfDataToSave data)
        {
            brokenLine.LoadData(data.Objects);
            
            SignLinesToEvents();
        }

        private void Marker_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            СellMarker = false;
            marker = (Ellipse)sender;
            Point point = new(Canvas.GetLeft(marker) + 5, Canvas.GetTop(marker) + 5);
            
            RemoveUIElemrnt(brokenLine.GetLinesLess(5));
            SetMarkers();
            SelectObject(this);
        }

        private void Marker_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
            marker = (Ellipse)sender;
            СellMarker = true;
            Point point = new(Canvas.GetLeft(marker) + 5, Canvas.GetTop(marker) + 5);
            if (e.ClickCount == 1)
            {
                brokenLine.PointInRadius(point, 0);
            }
            SelectObject(this);
        }

        private void Line_MouseLeftDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;

            if (e.ClickCount == 2)
            {
                marker = null;
                СellMarker = false;
                Line line = (Line)sender;
                Point point = e.GetPosition(line);

                brokenLine.SplitTheLine(line, point);
                SignLinesToEvents();
            }
            SetMarkers();
            SelectObject(this);
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
            if (СellMarker)
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
            brokenLine.ChangeColor(color);
        }

        public void ChangeThickness(double thick)
        {
            brokenLine.ChangeThickness(thick);
        }

        public void DeselectShape()
        {
            RemoveUIElemrnt(markers);
        }

        public void StartingPoint(Point point)
        {
            СellMarker = true;
            brokenLine.SetLine(point, point);
            SignLinesToEvents();
            SelectObject(this);
        }

        public void MousePositionOnCanvas(Point point)
        {
            brokenLine.DistanceFromLinePointToClick(point);
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
