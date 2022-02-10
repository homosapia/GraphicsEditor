using System;
using System.Collections.Generic;
using System.Linq;
using GraphicsEditor.Objects;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using GraphicsEditor.Abstracts;
using GraphicsEditor.Data;
using Newtonsoft.Json;
using GraphicsEditor.Resources;

namespace GraphicsEditor
{
    public class BrokenLineFigure : IFigure
    {
        public event EventSelectFigure SelectFigure;
        public event EventRemoveUiElements RemoveUiElements;
        public event EventAddUiElements AddUiElements;

        private readonly List<Line> lines = new();
        private Line changeStart = new();
        private Line changeTheEnd = new();

        private List<Ellipse> markers = new();
        private Ellipse marker = new();
        private bool markerSelected;

        private Color color = Color.FromArgb(255, 0, 0, 0);
        private double thickness;

        public FigureDataToSave GetDataToSave()
        {
            FigureDataToSave figureData = new();

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

            figureData.FigureJson = JsonConvert.SerializeObject(brokenLineData);
            figureData.FigureType = DataResources.BrokenLine;
            return figureData;
        }

        public void FillWithData(FigureDataToSave data)
        {
            BrokenLineDataToSave brokenLineData = JsonConvert.DeserializeObject<BrokenLineDataToSave>(data.FigureJson);

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

            SignLinesToEvents();
        }

        private void Marker_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            markerSelected = false;
            List<UIElement> lines = GetLinesLess(5);
            if (lines.Count > 0)
            {
                marker = (Ellipse)sender;
                Point point = new(Canvas.GetLeft(marker) + 5, Canvas.GetTop(marker) + 5);

                RemoveUiElements(lines);
                
                SetMarkers();
                SelectFigure(this);
            }
        }

        private List<UIElement> GetLinesLess(int length)
        {
            List<UIElement> lines = new();
            int i = 0;
            while (i < this.lines.Count)
            {
                if (Math.Abs(this.lines[i].X1 - this.lines[i].X2) <= length && Math.Abs(this.lines[i].Y1 - this.lines[i].Y2) <= length)
                {
                    lines.Add(this.lines[i]);

                    if ((i - 1) >= 0 && (i + 1) <= this.lines.Count - 1)
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

        private void Marker_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
            marker = (Ellipse)sender;
            markerSelected = true;
            Point point = new(Canvas.GetLeft(marker) + 5, Canvas.GetTop(marker) + 5);
            if (e.ClickCount == 1)
            {
                PointInRadius(point, 0);
            }
            SelectFigure(this);
        }

        private void Line_MouseLeftDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            RemoveUiElements?.Invoke(new List<UIElement>(markers));

            e.Handled = true;
            SetMarkers();
            markerSelected = false;
            if (e.ClickCount == 2)
            {
                Line line = (Line)sender;
                Point point = e.GetPosition(line);

                SplitTheLine(line, point);
                SignLinesToEvents();
                PointInRadius(point, 0);

                marker = CreateMarker(point);
                markers.Add(marker);
                markerSelected = true;
            }
            SelectFigure(this);
        }

        private void PointInRadius(Point point, byte Radius)
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

        private void SplitTheLine(Line line, Point point)
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
            lines.Insert(index + 1, lines.Last());
            lines.RemoveAt(lines.Count - 1);
        }

        private void MoveLines(double deltaX, double deltaY)
        {
            foreach (Line line in lines)
            {
                line.X1 += deltaX;
                line.Y1 += deltaY;

                line.X2 += deltaX;
                line.Y2 += deltaY;
            }

            List<Point> points = GetConnectionPointsOfLines();
            for (int i = 0; i < markers.Count; i++)
            {
                Canvas.SetLeft(markers[i], points[i + 1].X - (thickness / 2));
                Canvas.SetTop(markers[i], points[i + 1].Y - (thickness / 2));
            }
        }

        private void SetLine(Point start, Point end)
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

        private void SetMarkers()
        {
            List<Point> points = GetConnectionPointsOfLines();

            RemoveUiElements?.Invoke(new List<UIElement>(markers));

            markers = new();
            for (int i = 0; i < points.Count; i++)
            {
                markers.Add(CreateMarker(points[i]));
            }
        }

        public void ChangeToDelta(double deltaX, double deltaY)
        {
            if (markerSelected)
            {
                Point positionMarker = new(Canvas.GetLeft(marker) + deltaX, Canvas.GetTop(marker) + deltaY);
                Point positionLine = new(positionMarker.X + 5, positionMarker.Y + 5);
                
                ChangeLinePointPosition(positionLine);
                
                Canvas.SetLeft(marker, positionMarker.X);
                Canvas.SetTop(marker, positionMarker.Y);
            }
        }

        private void ChangeLinePointPosition(Point point)
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

        private void SignLinesToEvents()
        {
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

        public void RemoveSelection()
        {
            RemoveUiElements?.Invoke(new List<UIElement>(markers));
            AddUiElements?.Invoke(new List<UIElement>(CreatePlaque()));

        }
        private List<Ellipse> CreatePlaque()
        {
            markers.Clear();

            List<Point> points = GetConnectionPointsOfLines();

            for (int i = 1; i < points.Count - 1; i++)
            {
                Ellipse plaque = new();

                plaque.Fill = new SolidColorBrush(color);

                plaque.Width = thickness;
                plaque.Height = thickness;

                Canvas.SetLeft(plaque, points[i].X - (thickness / 2));
                Canvas.SetTop(plaque, points[i].Y - (thickness / 2));

                this.markers.Add(plaque);
            }

            return markers;
        }

        private List<Point> GetConnectionPointsOfLines()
        {
            List<Point> points = new();
            foreach (Line line in lines)
            {
                points.Add(new Point(line.X1, line.Y1));
                points.Add(new Point(line.X2, line.Y2));
            }
            return points.Distinct().ToList();
        }

        public void StartDrawing(Point point)
        {
            markerSelected = true;
            
            
            SetLine(point, point);
            
            SignLinesToEvents();
            
            markers.Add(CreateMarker(point));
            markers.Add(marker = CreateMarker(point));

            SelectFigure(this);
        }

        public void CanvasMouseLeftButtonDown()
        {
        }

        public List<UIElement> GetAllUIElements()
        {
            List<UIElement> uIElements = new();

            uIElements.AddRange(lines);

            uIElements.AddRange(markers);

            return uIElements;
        }

        public void MoveDistance(double deltaX, double deltaY)
        {
            MoveLines(deltaX, deltaY);
        }

        public void CanvasMouseLeftButtonUp()
        {
        }
    }
}
