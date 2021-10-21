using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphicsEditor.Objects
{
    class BrokenLine
    {
        private const byte idFirstPointLine = 1;
        private const byte idLastPointLine = 2;
        private const byte idUnselectedPoint = 3;

        private List<Line> lines = new();
        private List<byte> idPointLine = new();

        private Color color = Color.FromArgb(255,0,0,0);
        private double thick;


        public void FindThePointsOfTheLinesInTheRadius(Point point, byte Radius)
        {
            idPointLine = new();
            foreach (Line line in lines)
            {
                if (Math.Abs(point.X - line.X2) <= Radius && Math.Abs(point.Y - line.Y2) <= Radius)
                {
                    idPointLine.Add(idLastPointLine);
                    continue;
                }
                if (Math.Abs(point.X - line.X1) <= Radius && Math.Abs(point.Y - line.Y1) <= Radius)
                {
                    idPointLine.Add(idFirstPointLine);
                    continue;
                }
                idPointLine.Add(idUnselectedPoint);
            }
        }

        public List<Point> GetConnectionPointsOfLines()
        {
            List<Point> points = new();
            foreach (Line line in lines)
            {
                points.Add(new Point(line.X1, line.Y1));
                points.Add(new Point(line.X2, line.Y2));
            }
            return points.Distinct().ToList();
        }

        public void SetLine(Point start, Point end, byte idPoint = idLastPointLine)
        {
            Line line = new();
            line.Stroke = new SolidColorBrush(color);
            line.StrokeThickness = thick;

            line.X1 = start.X;
            line.Y1 = start.Y;

            line.X2 = end.X;
            line.Y2 = end.Y;

            idPointLine.Add(idPoint);
            lines.Add(line);
        }

        public List<Line> GetLines()
        {
            return lines.ToList();
        }

        public void ChangeLinePointPosition(Point point)
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

        public void SplitTheLine(Line line, Point point)
        {
            Point point1 = new(line.X2, line.Y2);

            line.X2 = point.X;
            line.Y2 = point.Y;

            SetLine(point, point1, idUnselectedPoint);
        }

        public List<Line> GetLinesLess(int length)
        {
            List<Line> lines = new();
            int i = 0;
            while (i < this.lines.Count)
            {
                if (Math.Abs(this.lines[i].X1 - this.lines[i].X2) <= length && Math.Abs(this.lines[i].Y1 - this.lines[i].Y2) <= length)
                {
                    lines.Add(this.lines[i]);
                    this.lines.Remove(this.lines[i]);
                    i = 0;
                }
                else
                    i++;
            }

            return lines;
        }

        public void ChangeColor(Color color)
        {
            this.color = color;
            foreach (Line line in lines)
            {
                line.Stroke = new SolidColorBrush(color);
            }
        }

        public void ChangeThickness(double thick)
        {
            this.thick = thick;
            foreach (Line line in lines)
            {
                line.StrokeThickness = this.thick;
            }
        }
    }
}
