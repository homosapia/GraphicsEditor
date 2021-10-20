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

namespace GraphicsEditor
{
    class FigureBrokenLine : IFigure
    {
        public event EventSelectFigure SelectObject;
        public event EventTransform Transform;
        public event EventClickMarker ClickMarker;
        public event EventSetMarker SetMarker;
        public event EventRemoveMarker RemoveMarker;
        public event EventRemoveUiElemrnt RemoveUiElemrnt;

        BrokenLine brokenLine = new();


        List<Ellipse> markers = new();
        Ellipse marker = new();

        private void Marker_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ClickMarker(false);
        }

        private void Marker_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            marker = (Ellipse)sender;
            ClickMarker(true);
            Point point = new(Canvas.GetLeft(marker) + 5, Canvas.GetTop(marker) + 5);
            if (e.ClickCount == 1)
            {
                brokenLine.FindThePointsOfTheLinesInTheRadius(point, 0);
            }

            if (e.ClickCount == 2)
            {
                ClickMarker(false);
                RemoveLine();
                brokenLine.FindThePointsOfTheLinesInTheRadius(point, 5);
                brokenLine.ChangeLinePointPosition(point);
                SetMarkers();
            }
        }

        private void SetMarkers()
        {
            List<Point> points = brokenLine.GetConnectionPointsOfLines();
            
            RemoveMarker();
            markers = new();
            for (int i = 0; i < points.Count; i++)
            {
                markers.Add(CreateMarker(points[i]));

            }
            SetMarker(markers);
        }

        private void RemoveLine()
        {
            List<Line> lines = brokenLine.GetLinesLess(5);
            int i = 0;
            while (i < lines.Count)
            {
                Line del = lines[i];
                RemoveUiElemrnt(del);
                lines.Remove(lines[i]);
            }
        }

        private void Marker_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
        }

        public void ChangePosition(Point point)
        {
            brokenLine.ChangeLinePointPosition(point);
            Canvas.SetLeft(marker, point.X - 5);
            Canvas.SetTop(marker, point.Y - 5);
        }

        public void CreateFigure(Point mouse)
        {
            brokenLine.SetLine(mouse, mouse);
            SignLinesToEvents();
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

        public void DrawFigure(Point point)
        {
            brokenLine.ChangeLinePointPosition(point);
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

            marker.MouseLeave += Marker_MouseLeave;
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
    }
}
