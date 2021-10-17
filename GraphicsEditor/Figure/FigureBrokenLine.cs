using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        List<Line> lines;
        List<int> idPointLine;

        Line currentLine;

        List<Rectangle> markers = new();
        Rectangle marker;

        Canvas canvas;

        public FigureBrokenLine(Canvas canvas)
        {
            this.canvas = canvas;
            lines = new();
        }

        private void Marker_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ClickMarker(false);
        }

        private void Marker_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
            marker = (Rectangle)sender;
            ClickMarker(true);
            if (e.ClickCount == 1)
            {
                FindThePointsOfTheLinesInTheRadius(0);
            }

            if (e.ClickCount == 2)
            {
                RemoveLineAndMarker(marker);
                FindThePointsOfTheLinesInTheRadius(5);
                Point point = new(Canvas.GetLeft(marker) + 5, Canvas.GetTop(marker) + 5);
                FindLinesWithSamePoints(point);
                FindIdenticalPointsOfTheLine();
            }
        }

        private void FindIdenticalPointsOfTheLine()
        {
            List<Point> points = new();
            foreach (Line line in lines)
            {
                points.Add(new Point(line.X1, line.Y1));
                points.Add(new Point(line.X2, line.Y2));
            }
            points = points.Distinct().ToList();

            RemoveMarker();

            markers = new();
            for (int i = 0; i < points.Count; i++)
            {
                markers.Add(CreateMarker(points[i]));

            }
            SetMarker(markers);
        }

        private void RemoveLineAndMarker(Rectangle marker)
        {
            ClickMarker(false);
            int i = 0;
            while (i < lines.Count)
            {
                if (Math.Abs(lines[i].X1 - lines[i].X2) <= 5 && Math.Abs(lines[i].Y1 - lines[i].Y2) <= 5)
                {
                    canvas.Children.Remove(lines[i]);
                    lines.Remove(lines[i]);
                    i = 0;
                }
                else
                    i++;
            }
        }

        private void Marker_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
        }

        public void ChangePosition(Point point)
        {
            FindLinesWithSamePoints(point);
            Canvas.SetLeft(marker, point.X - 5);
            Canvas.SetTop(marker, point.Y - 5);
        }

        private void FindLinesWithSamePoints(Point point)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                if (idPointLine[i] == 1)
                {
                    lines[i].X1 = point.X;
                    lines[i].Y1 = point.Y;
                }
                if (idPointLine[i] == 2)
                {
                    lines[i].X2 = point.X;
                    lines[i].Y2 = point.Y;
                }
            }
        }

        public void CreateFigure(Point mouse)
        {
            idPointLine = new();

            currentLine = new();
            currentLine.Stroke = Brushes.Black;
            currentLine.X1 = mouse.X;
            currentLine.Y1 = mouse.Y;
            currentLine.X2 = mouse.X;
            currentLine.Y2 = mouse.Y;

            currentLine.MouseLeftButtonDown += Line_MouseLeftDown;
            currentLine.MouseMove += Line_MouseMove;

            lines.Add(currentLine);
            idPointLine.Add(2);

            SelectObject(this);
        }

        public void DrawFigure(Point point)
        {
            currentLine.X2 = point.X;
            currentLine.Y2 = point.Y;
        }

        private void FindThePointsOfTheLinesInTheRadius(byte Radius)
        {
            idPointLine = new();
            foreach (Line line in lines)
            {
                Point point = new(Canvas.GetLeft(marker) + 5, Canvas.GetTop(marker) + 5);
                if (Math.Abs(point.X - line.X2) <= Radius && Math.Abs(point.Y - line.Y2) <= Radius)
                {
                    idPointLine.Add(2);
                    continue;
                }
                if (Math.Abs(point.X - line.X1) <= Radius && Math.Abs(point.Y - line.Y1) <= Radius) 
                {
                    idPointLine.Add(1);
                    continue;
                }
                idPointLine.Add(3);
            }
        }

        private void Line_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            /*
            SelectObject(this);

            currentLine = (Line)sender;

            FindLinesWithSamePoints();

            Point point = e.GetPosition(canvas);

            if (Math.Abs(currentLine.X1 - point.X) <= 5 && Math.Abs(currentLine.Y1 - point.Y) <= 5)
            {
                Canvas.SetLeft(marker, currentLine.X1 - 5);
                Canvas.SetTop(marker, currentLine.Y1 - 5);

                if (!canvas.Children.Contains(marker))
                    canvas.Children.Add(marker);
            }

            if (Math.Abs(currentLine.X2 - point.X) <= 5 && Math.Abs(currentLine.Y2 - point.Y) <= 5)
            {
                Canvas.SetLeft(marker, currentLine.X2 - 5);
                Canvas.SetTop(marker, currentLine.Y2 - 5);

                if (!canvas.Children.Contains(marker))
                    canvas.Children.Add(marker);
            }*/
        }

        private void Line_MouseLeftDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
            if (e.ClickCount == 1)
            {
                Transform(true);
                SelectObject(this);
            }

            if (e.ClickCount == 2)
            {
                Point point = e.GetPosition(canvas);

                Line line = (Line)sender;
                Point point1 = new(line.X2, line.Y2);

                line.X2 = point.X;
                line.Y2 = point.Y;

                currentLine = new();
                currentLine.Stroke = Brushes.Black;
                currentLine.X1 = point.X;
                currentLine.Y1 = point.Y;
                currentLine.X2 = point1.X;
                currentLine.Y2 = point1.Y;

                currentLine.MouseDown += Line_MouseLeftDown;
                currentLine.MouseMove += Line_MouseMove;

                lines.Add(currentLine);

                SelectObject(this);
            }
            FindIdenticalPointsOfTheLine();
        }
        /*
        private void DrawMarker()
        {
            markers = new();
            foreach (Line line in lines)
            {
                if(line == lines.Last())
                {
                    for (int i = 0; i < 2; i++) 
                    {
                        Rectangle marker = CreateMarker();
                        if(i == 0)
                        {
                            Canvas.SetLeft(marker, line.X1 - 5);
                            Canvas.SetTop(marker, line.Y1 - 5);
                        }
                        if(i == 1)
                        {
                            Canvas.SetLeft(marker, line.X2 - 5);
                            Canvas.SetTop(marker, line.Y2 - 5);
                        }
                        markers.Add(marker);
                    }
                }
                else
                {
                    Rectangle marker = CreateMarker();
                    Canvas.SetLeft(marker, line.X1 - 5);
                    Canvas.SetTop(marker, line.Y1 - 5);
                    markers.Add(marker);
                }
            }
            SetMarker(markers);
        }*/

        private Rectangle CreateMarker(Point point)
        {
            Rectangle marker = new();

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

        public void DelMarker()
        {
            throw new NotImplementedException();
        }

        public void EndObject(Point point)
        {
            throw new NotImplementedException();
        }

        public object Figure()
        {
            if (lines.Count != 0)
                return lines.Last();
            else
                return null;
        }

    }
}
