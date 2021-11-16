using GraphicsEditor.Abstracts;
using GraphicsEditor.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Linq;

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
        private double thickness;
        private Point previousMouse;

        public BrokenLineDataToSave DataToSave()
        {
            Point[,] points = new Point[lines.Count, 2];
            for (int i = 0; i < lines.Count; i++)
            {
                for (int l = 0; l < 2; l++)
                {
                    if (l == 0)
                    {
                        Point point = new(lines[i].X1, lines[i].Y1);
                        points[i, l] = point;
                    }
                    if (l == 1)
                    {
                        Point point = new(lines[i].X2, lines[i].Y2);
                        points[i, l] = point;
                    }
                }
            }

            BrokenLineDataToSave brokenLineData = new BrokenLineDataToSave();

            brokenLineData.points = points;

            brokenLineData.colorA = color.A;
            brokenLineData.colorR = color.R;
            brokenLineData.colorG = color.G;
            brokenLineData.colorB = color.B;

            brokenLineData.thick = thickness;
            brokenLineData.LineCount = lines.Count;

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
                Line line = new();

                for (int j = 0; j < 2; j++)
                {
                    line.X1 = points[i, j].X;
                    line.Y1 = points[i, j].Y;
                    line.X2 = points[i, j].Y;
                    line.Y2 = points[i, j].Y;

                    line.Stroke = new SolidColorBrush(color);
                    line.StrokeThickness = thickness;
                }

                SetLine(line);
            }
        }

        public void PointInRadius(Point point, byte Radius)
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

        public void SetLine(Line line, byte idPoint = idLastPointLine)
        {
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

        public void MoveLines(Point move)
        {
            foreach (Line line in lines)
            {
                line.X1 += move.X - previousMouse.X;
                line.Y1 += move.Y - previousMouse.Y;

                line.X2 += move.X - previousMouse.X;
                line.Y2 += move.Y - previousMouse.Y;
            }

            previousMouse = move;
        }

        public void SetСlickPoint(Point click)
        {
            previousMouse = click;
        }

        public void SplitTheLine(Line line, Point point)
        {
            Point point1 = new(line.X2, line.Y2);

            line.X2 = point.X;
            line.Y2 = point.Y;

            Line newLine = new();
            newLine.X1 = point.X;
            newLine.Y1 = point.Y;
            newLine.X2 = point.X;
            newLine.Y2 = point.Y;
            line.Stroke = new SolidColorBrush(color);
            line.StrokeThickness = thickness;


            SetLine(newLine, idUnselectedPoint);

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
            this.thickness = thick;
            foreach (Line line in lines)
            {
                line.StrokeThickness = this.thickness;
            }
        }
    }
}
