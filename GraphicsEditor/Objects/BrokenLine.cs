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
        private double thick;
        private Point InitialClick;

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
                Line line = new();
                line.X1 = points[i, 0, 0].X;
                line.Y1 = points[i, 0, 0].Y;
                line.X2 = points[i, 1, 0].Y;
                line.Y2 = points[i, 1, 0].Y;

                line.Stroke = new SolidColorBrush(color);
                line.StrokeThickness = thick;
                
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
            for (int li = 0; li < lines.Count; li++)
            {
                lines[li].X1 = move.X + -(InitialClick.X - lines[li].X1);
                lines[li].Y1 = move.Y + -(InitialClick.Y - lines[li].Y1);

                lines[li].X2 = move.X + -(InitialClick.X - lines[li].X2);
                lines[li].Y2 = move.Y + -(InitialClick.Y - lines[li].Y2);
            }

            InitialClick = move;
        }

        public void SetСlickPoint(Point click)
        {
            InitialClick = click;
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
            line.StrokeThickness = thick;


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
            this.thick = thick;
            foreach (Line line in lines)
            {
                line.StrokeThickness = this.thick;
            }
        }
    }
}
