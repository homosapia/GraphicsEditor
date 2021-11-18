using GraphicsEditor.Abstracts;
using GraphicsEditor.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace GraphicsEditor.Objects
{
    class BrokenLine
    {
        private readonly List<Line> lines = new();

        private Line changeStart = new();
        private Line changeTheEnd = new();

        private Color color = Color.FromArgb(255,0,0,0);
        private double thickness;

        public BrokenLineDataToSave DataToSave()
        {
            Point[,] points = new Point[lines.Count, 2];
            for (int i = 0; i < lines.Count; i++)
            {
                Point point = new(lines[i].X1, lines[i].Y1);
                points[i, 0] = point;

                point = new(lines[i].X2, lines[i].Y2);
                points[i, 1] = point;
            }

            BrokenLineDataToSave brokenLineData = new()
            {
                points = points,

                colorA = color.A,
                colorR = color.R,
                colorG = color.G,
                colorB = color.B,

                thick = thickness,
                LineCount = lines.Count
            };

            return brokenLineData;
        }

        public void FillWithData(BrokenLineDataToSave brokenLineData)
        {
            color.A = brokenLineData.colorA;
            color.R = brokenLineData.colorR;
            color.G = brokenLineData.colorG;
            color.B = brokenLineData.colorB;

            thickness = brokenLineData.thick;

            Point[,] points = brokenLineData.points;

            for (int i = 0; i < brokenLineData.LineCount; i++)
            {
                SetLine(points[i, 0], points[i, 1]);
            }
        }

        public void PointInRadius(Point point, byte Radius)
        {
            changeTheEnd = null;
            changeStart = null;
            foreach (Line line in lines)
            {
                if (Math.Abs(point.X - line.X2) <= Radius && Math.Abs(point.Y - line.Y2) <= Radius)
                {
                    changeTheEnd = line;
                    continue;
                }
                if (Math.Abs(point.X - line.X1) <= Radius && Math.Abs(point.Y - line.Y1) <= Radius)
                {
                    changeStart = line;
                    continue;
                }
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

        public void SetLine(Point start, Point end)
        {
            Line line = new();

            line.Stroke = new SolidColorBrush(color);
            line.StrokeThickness = thickness;

            line.X1 = start.X;
            line.Y1 = start.Y;

            line.X2 = end.X;
            line.Y2 = end.Y;

            changeTheEnd = line;
            lines.Add(line);
        }

        public List<Line> GetLines()
        {
            return lines.ToList();
        }

        public void ChangeLinePointPosition(Point point)
        {
            if (changeStart != null)
            {
                changeStart.X1 = point.X;
                changeStart.Y1 = point.Y;
            }

            if (changeTheEnd != null)
            {
                changeTheEnd.X2 = point.X;
                changeTheEnd.Y2 = point.Y;
            }
        }

        public void MoveLines(double deltaX, double deltaY)
        {
            foreach (Line line in lines)
            {
                line.X1 += deltaX;
                line.Y1 += deltaY;

                line.X2 += deltaX;
                line.Y2 += deltaY;
            }
        }

        public void SplitTheLine(Line line, Point point)
        {
            Point point1 = new(line.X2, line.Y2);

            line.X2 = point.X;
            line.Y2 = point.Y;

            Line newLine = new();
            newLine.X1 = point.X;
            newLine.Y1 = point.Y;
            newLine.X2 = point1.X;
            newLine.Y2 = point1.Y;
            newLine.Stroke = new SolidColorBrush(color);
            newLine.StrokeThickness = thickness;
            
            changeTheEnd = line;
            changeStart = newLine;

            lines.Add(newLine);

            int index = lines.IndexOf(line);
            lines.Insert(index+1, lines.Last());
            lines.RemoveAt(lines.Count-1);
        }

        public List<UIElement> GetLinesLess(int length)
        {
            List<UIElement> lines = new();
            int i = 0;
            while (i < this.lines.Count)
            {
                if (Math.Abs(this.lines[i].X1 - this.lines[i].X2) <= length && Math.Abs(this.lines[i].Y1 - this.lines[i].Y2) <= length)
                {
                    lines.Add(this.lines[i]);

                    if((i-1) >= 0 && (i+1) <= this.lines.Count - 1)
                    {
                        this.lines[i - 1].X2 = this.lines[i + 1].X1;
                        this.lines[i - 1].Y2 = this.lines[i + 1].Y1;
                    }
                    this.lines.Remove(this.lines[i]);
                    i = 0;
                }
                else
                    i++;
            }

            return lines;
        }

        public void SetColor(Color color)
        {
            this.color = color;
            foreach (Line line in lines)
            {
                line.Stroke = new SolidColorBrush(color);
            }
        }

        public void SetThickness(double thick)
        {
            this.thickness = thick;
            foreach (Line line in lines)
            {
                line.StrokeThickness = this.thickness;
            }
        }
    }
}
