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
        public event EventGetFigure ReceiveFigure;
        public event EventCilckMarker ClickMarker;
        public event EventRemoveFigure RemoveFigure;

        List<Line> lines;
        List<int> idPointLine;

        Line currentLine;

        bool transform;

        Rectangle marker;

        Canvas canvas;

        public FigureBrokenLine(Canvas canvas)
        {
            this.canvas = canvas;
            lines = new();
            marker = SetMarker();
        }

        private Rectangle SetMarker()
        {
            Rectangle marker = new();

            marker.Fill = Brushes.Red;

            marker.Width = 10;
            marker.Height = 10;

            marker.MouseLeave += Marker_MouseLeave;
            marker.MouseLeftButtonDown += Marker_MouseLeftButtonDown;
            marker.MouseLeftButtonUp += Marker_MouseLeftButtonUp;
            return marker;
        }

        private void Marker_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                Point point = new(Canvas.GetLeft(marker) + 5, Canvas.GetTop(marker) + 5);
                if (Math.Abs(lines[i].X1 - lines[i].X2) <= 5 && Math.Abs(lines[i].Y1 - lines[i].Y2) <= 5)
                {
                    if ((i - 1) >= 0 && (i + 1) < lines.Count)
                    {
                        lines[i - 1].X2 = point.X;
                        lines[i - 1].Y2 = point.Y;
                        lines[i + 1].X1 = point.X;
                        lines[i + 1].Y1 = point.Y;
                    }

                    canvas.Children.Remove(lines[i]);

                    lines.Remove(lines[i]);
                }
            }
            transform = false;
            ClickMarker(transform);
        }

        private void Marker_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            FindLinesWithSamePoints();

            transform = true;
            ClickMarker(transform);
        }

        private void Marker_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            canvas.Children.Remove(marker);
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


            currentLine.MouseDown += Line_MouseDown;
            currentLine.MouseMove += Line_MouseMove;

            lines.Add(currentLine);
            idPointLine.Add(2);

            ReceiveFigure(this);
        }

        private void FindLinesWithSamePoints()
        {
            idPointLine = new();
            foreach (Line line in lines)
            {
                Point point = new(Canvas.GetLeft(marker) + 5, Canvas.GetTop(marker) + 5);
                if (Math.Abs(point.X - line.X2) <= 5 && Math.Abs(point.Y - line.Y2) <= 5)
                {
                    idPointLine.Add(2);
                    continue;
                }
                if (Math.Abs(point.X - line.X1) <= 5 && Math.Abs(point.Y - line.Y1) <= 5) 
                {
                    idPointLine.Add(1);
                    continue;
                }
                idPointLine.Add(3);
            }
        }

        private void Line_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ReceiveFigure(this);

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
            }
        }

        private void Line_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                Point point = e.GetPosition(canvas);
                Point point1 = new(currentLine.X2, currentLine.Y2);

                currentLine.X2 = point.X;
                currentLine.Y2 = point.Y;

                currentLine = new();
                currentLine.Stroke = Brushes.Black;
                currentLine.X1 = point.X;
                currentLine.Y1 = point.Y;
                currentLine.X2 = point1.X;
                currentLine.Y2 = point1.Y;

                currentLine.MouseDown += Line_MouseDown;
                currentLine.MouseMove += Line_MouseMove;

                lines.Add(currentLine);

                FindLinesWithSamePoints();

                ReceiveFigure(this);
            }
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
