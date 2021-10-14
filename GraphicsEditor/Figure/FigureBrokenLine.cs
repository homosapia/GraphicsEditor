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
        public event EventGetFigure GetFigure;
        public event EventCilckMarker ClickMarker;

        List<Line> lines;
        List<Line> connectedLines;

        Line currentLine;
        int numberLine;
        int BeginningOrEnd;

        bool transform;

        Rectangle marker;

        Canvas canvas;

        public FigureBrokenLine(Canvas canvas)
        {
            this.canvas = canvas;
            lines = new();
            marker = new();

            marker.Fill = Brushes.Red;

            marker.Width = 10;
            marker.Height = 10;

            marker.MouseLeave += Marker_MouseLeave;
            marker.MouseLeftButtonDown += Marker_MouseLeftButtonDown;
            marker.MouseLeftButtonUp += Marker_MouseLeftButtonUp;
        }

        private void Marker_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            transform = false;
            ClickMarker(transform);
        }

        private void Marker_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            transform = true;
            ClickMarker(transform);
        }

        private void Marker_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            canvas.Children.Remove(marker);
        }



        public void ChangePosition(Point point)
        {
            if (BeginningOrEnd == 1)
            {
                FindLinesWithSamePoints(point);

                currentLine.X1 = point.X;
                currentLine.Y1 = point.Y;

                Canvas.SetLeft(marker, currentLine.X1 - 5);
                Canvas.SetTop(marker, currentLine.Y1 - 5);
            }
            if (BeginningOrEnd == 2)
            {
                FindLinesWithSamePoints(point);

                currentLine.X2 = point.X;
                currentLine.Y2 = point.Y;

                Canvas.SetLeft(marker, currentLine.X2 - 5);
                Canvas.SetTop(marker, currentLine.Y2 - 5);
            }
        }

        public void CreateFigure(Point mouse)
        {
            connectedLines = new();
            currentLine = new();
            currentLine.Stroke = Brushes.Black;
            currentLine.X1 = mouse.X;
            currentLine.Y1 = mouse.Y;
            currentLine.X2 = mouse.X;
            currentLine.Y2 = mouse.Y;

            BeginningOrEnd = 2;

            currentLine.MouseDown += Line_MouseDown;
            currentLine.MouseMove += Line_MouseMove;

            lines.Add(currentLine);

            GetFigure(this);
        }

        private void FindLinesWithSamePoints(Point point)
        {
            foreach (Line line in connectedLines)
            {
                if (currentLine.X1 == line.X1 && currentLine.Y1 == line.Y1)
                {
                    line.X1 = point.X;
                    line.Y1 = point.Y;
                }
                if (currentLine.X2 == line.X2 && currentLine.Y2 == line.Y2)
                {
                    line.X2 = point.X;
                    line.Y2 = point.Y;
                }
                if (currentLine.X1 == line.X2 && currentLine.Y1 == line.Y2)
                {
                    line.X1 = point.X;
                    line.Y1 = point.Y;
                }
                if (currentLine.X2 == line.X1 && currentLine.Y2 == line.Y1)
                {
                    line.X2 = point.X;
                    line.Y2 = point.Y;
                }
            }
        }

        private void FindLinesWithSamePoints()
        {
            connectedLines = new();
            foreach (Line line in lines)
            {
                if (currentLine == line)
                    continue;

                if (currentLine.X1 == line.X1 && currentLine.Y1 == line.Y1)
                {
                    connectedLines.Add(line);
                    continue;
                }
                if (currentLine.X2 == line.X2 && currentLine.Y2 == line.Y2)
                {
                    connectedLines.Add(line);
                    continue;
                }
                if (currentLine.X1 == line.X2 && currentLine.Y1 == line.Y2)
                {
                    connectedLines.Add(line);
                    continue;
                }
                if (currentLine.X2 == line.X2 && currentLine.Y2 == line.Y1)
                {
                    connectedLines.Add(line);
                    continue;
                }

            }
        }

        private void Line_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            GetFigure(this);

            if (!transform)
            {
                currentLine = (Line)sender;
                for (int i = 0; i < lines.Count; i++)
                {
                    if (currentLine == lines[i])
                    {
                        currentLine = lines[i];
                        numberLine = i;
                    }
                }
            }
            

            Point point = e.GetPosition(canvas);

            if (Math.Abs(currentLine.X1 - point.X) <= 5 && Math.Abs(currentLine.Y1 - point.Y) <= 5)
            {
                BeginningOrEnd = 1;

                Canvas.SetLeft(marker, currentLine.X1 - 5);
                Canvas.SetTop(marker, currentLine.Y1 - 5);

                if (!canvas.Children.Contains(marker))
                    canvas.Children.Add(marker);
            }

            if (Math.Abs(currentLine.X2 - point.X) <= 5 && Math.Abs(currentLine.Y2 - point.Y) <= 5)
            {
                BeginningOrEnd = 2;

                Canvas.SetLeft(marker, currentLine.X2 - 5);
                Canvas.SetTop(marker, currentLine.Y2 - 5);

                if (!canvas.Children.Contains(marker))
                    canvas.Children.Add(marker);
            }
        }

        private void Line_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(e.ClickCount == 2)
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

                GetFigure(this);
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
            return currentLine;
        }

        public object NewObject(Canvas canvas)
        {
            throw new NotImplementedException();
        }

        public void SetMarker()
        {
            throw new NotImplementedException();
        }

        public void StartObject(Point point)
        {
            throw new NotImplementedException();
        }
    }
}
