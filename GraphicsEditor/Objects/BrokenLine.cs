﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
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

        private double[,,,] distance;

        public List<object> DataToSave()
        {
            List<object> objects = new();

            Point[,,] points = new Point[lines.Count,2,1];
            for (int i = 0; i < lines.Count; i++)
            {
                for(int l = 0; l < 2; l++)
                {
                    if (l == 0)
                    {
                        Point point = new(lines[i].X1, lines[i].Y1);
                        points[i, l, 0] = point;
                    }
                    if(l == 1)
                    {
                        Point point = new(lines[i].X2, lines[i].Y2);
                        points[i, l, 0] = point;
                    }
                }
            }

            objects.Add(JsonConvert.SerializeObject(points));

            objects.Add(color.A);
            objects.Add(color.R);
            objects.Add(color.G);
            objects.Add(color.B);

            objects.Add(thick);

            objects.Add(lines.Count);

            return objects;
        }

        public void LoadData(List<object> objects)
        {
            color.A = Byte.Parse(objects[1].ToString());
            color.R = Byte.Parse(objects[2].ToString());
            color.G = Byte.Parse(objects[3].ToString());
            color.B = Byte.Parse(objects[4].ToString());

            thick = Double.Parse(objects[5].ToString());

            Point[,,] points = JsonConvert.DeserializeObject<Point[,,]>(objects[0].ToString());
            for (int i = 0; i < Int32.Parse(objects[6].ToString()); i++)
            {
                SetLine(points[i,0,0], points[i,1,0]);
            }
        }

        public void PointInRadius(Point point, byte Radius)
        {
            bool local = true;
            idPointLine = new();
            foreach (Line line in lines)
            {
                if (local && Math.Abs(point.X - line.X2) <= Radius && Math.Abs(point.Y - line.Y2) <= Radius)
                {
                    idPointLine.Add(idLastPointLine);
                    idPointLine.Add(idFirstPointLine);
                    local = false;
                    continue;
                }
                if (local && Math.Abs(point.X - line.X1) <= Radius && Math.Abs(point.Y - line.Y1) <= Radius)
                {
                    idPointLine.Add(idFirstPointLine);
                    idPointLine.Add(idLastPointLine);
                    local = false;
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

        public void MoveLines(Point move)
        {
            for (int li = 0; li < lines.Count; li++)
            {
                lines[li].X1 = move.X + -distance[li, 0, 0, 0];
                lines[li].Y1 = move.Y + -distance[li, 0, 1, 0];

                lines[li].X2 = move.X + -distance[li, 1, 0, 0];
                lines[li].Y2 = move.Y + -distance[li, 1, 1, 0];
            }
        }

        public void DistanceFromLinePointToClick(Point click)
        {
            distance = new double[lines.Count, 2, 2, 1];

            for (int li = 0; li < lines.Count; li++)
            {
                for (int po = 0; po < 2; po++)
                {
                    for (int XY = 0; XY < 2; XY++)
                    {
                        if(po == 0)
                        {
                            if (XY == 0)
                            {
                                distance[li, po, XY, 0] = click.X - lines[li].X1;
                            }
                            if(XY == 1)
                            {
                                distance[li, po, XY, 0] = click.Y - lines[li].Y1;
                            }
                        }
                        if(po == 1)
                        {
                            if (XY == 0)
                            {
                                distance[li, po, XY, 0] = click.X - lines[li].X2;
                            }
                            if (XY == 1)
                            {
                                distance[li, po, XY, 0] = click.Y - lines[li].Y2;
                            }
                        }
                    }
                }
            }


        }

        public void SplitTheLine(Line line, Point point)
        {
            Point point1 = new(line.X2, line.Y2);

            line.X2 = point.X;
            line.Y2 = point.Y;

            SetLine(point, point1, idUnselectedPoint);

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
            this.thick = thick;
            foreach (Line line in lines)
            {
                line.StrokeThickness = this.thick;
            }
        }
    }
}
