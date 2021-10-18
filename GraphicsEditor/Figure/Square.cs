﻿using System;
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
    class Square : IFigure
    {
        public event EventClickMarker ClickMarker;
        public event EventSetMarker SetMarker;
        public event EventTransform Transform;
        public event EventSelectFigure SelectObject;
        public event EventRemoveMarker RemoveMarker;


        private Rectangle rectangle;
        
        Point [] points = new Point[2];

        double distanceX;
        double distanceY;

        bool transform;
        bool turn;
        bool move;

        Rectangle marker;
        Canvas canvas;
        ScaleTransform scaleTransform;
        public Square(Canvas canvas)
        {
            this.canvas = canvas;

            rectangle = new();

            rectangle.Fill = new SolidColorBrush(Colors.Black);
            rectangle.MouseMove += Rectangle_MouseMove;
            rectangle.MouseLeftButtonDown += Rectangle_MouseLeftButtonDown;
            rectangle.MouseLeave += Rectangle_MouseLeave;
 
            scaleTransform = new();
        }

        private void Rectangle_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (!transform)
            {
                turn = true;
                transform = false;
                move = false;
            }
        }

        private void Rectangle_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            distanceX = e.GetPosition(canvas).X - points[0].X;
            distanceY = e.GetPosition(canvas).Y - points[0].Y;

            e.Handled = true;
            move = true;
            turn = false;
            transform = false;

            RemoveMarker();
            List<Rectangle> markers = new();
            marker = CreateMarker(points[1]);
            markers.Add(marker);
            SetMarker(markers);

            Transform(true);
            ClickMarker(true);

            SelectObject(this);
        }

        public Rectangle CreateMarker(Point point)
        {
            Rectangle marker = new();
            marker.Fill = Brushes.Red;
            marker.Width = 10;
            marker.Height = 10;

            marker.MouseLeftButtonDown += Marker_MouseLeftButtonDown;
            marker.MouseLeftButtonUp += Marker_MouseLeftButtonUp;
            marker.MouseLeave += Marker_MouseLeave;

            Canvas.SetLeft(marker, point.X - 5);
            Canvas.SetTop(marker, point.Y - 5);

            return marker;
        }

        private void Marker_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            transform = false;
            ClickMarker(transform);
        }

        private void Marker_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            transform = true;
            move = false;
            turn = false;
            ClickMarker(transform);
        }

        public object NewObject(Canvas canvas)
        {
            return canvas;
        }

        private void Rectangle_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

        private void Marker_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
        }

        private void Rectangle_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            
        }

        

        public object Figure()
        {
            return rectangle;
        }

        public void StartObject(Point point)
        {
            this.points[0] = point;
            Canvas.SetLeft(rectangle, point.X);
            Canvas.SetTop(rectangle, point.Y);
        }


        public void DelMarker()
        {
            marker = null;
        }

        public void ChangePosition(Point point)
        {
            if (transform)
            {
                this.points[1] = point;
                _Transform(point);

                Canvas.SetLeft(marker, points[1].X - 5);
                Canvas.SetTop(marker, points[1].Y - 5);
            }
            if(turn)
            {
                Rotate(point);
            }
            if(move)
            {
                MoveFigure(point);
            }
        }

        private void _Transform(Point point)
        {
            points[1] = point;
            if (points[1].X > this.points[0].X && points[1].Y > this.points[0].Y)
            {
                Canvas.SetLeft(rectangle, points[0].X);
                Canvas.SetTop(rectangle, points[0].Y);

                rectangle.Width = Math.Abs(points[1].X - this.points[0].X);
                rectangle.Height = Math.Abs(points[1].Y - this.points[0].Y);
            }

            if (points[1].X < this.points[0].X && points[1].Y < this.points[0].Y)
            {
                Canvas.SetLeft(rectangle, points[1].X);
                Canvas.SetTop(rectangle, points[1].Y);

                rectangle.Width = Math.Abs(this.points[0].X - points[1].X);
                rectangle.Height = Math.Abs(this.points[0].Y - points[1].Y);
            }

            if (points[1].X > this.points[0].X && points[1].Y < this.points[0].Y)
            {
                Canvas.SetLeft(rectangle, this.points[0].X);
                Canvas.SetTop(rectangle, points[1].Y);

                rectangle.Width = Math.Abs(this.points[0].X - points[1].X);
                rectangle.Height = Math.Abs(this.points[0].Y - points[1].Y);
            }

            if (points[1].X < this.points[0].X && points[1].Y > this.points[0].Y)
            {
                Canvas.SetLeft(rectangle, points[1].X);
                Canvas.SetTop(rectangle, this.points[0].Y);

                rectangle.Width = Math.Abs(this.points[0].X - points[1].X);
                rectangle.Height = Math.Abs(this.points[0].Y - points[1].Y);
            }
        }

        private void Rotate(Point point)
        {
            RotateTransform RotateFigure = new();

            RotateFigure.CenterX = rectangle.Width / 2;
            RotateFigure.CenterY = rectangle.Height / 2;

            RotateFigure.Angle = point.Y - points[1].Y;
            rectangle.LayoutTransform = RotateFigure;
        }

        private void MoveFigure(Point point)
        {
            points[0].X = point.X - distanceX;
            points[0].Y = point.Y - distanceY;

            points[1].X = points[0].X + rectangle.Width;
            points[1].Y = points[0].Y + rectangle.Height;

            Canvas.SetLeft(rectangle, points[0].X);
            Canvas.SetTop(rectangle, points[0].Y);
        }

        public void CreateFigure(Point point)
        {
            points[0] = point;

            Canvas.SetLeft(rectangle, point.X);
            Canvas.SetTop(rectangle, point.Y);

            SelectObject(this);
        }

        public void DrawFigure(Point point)
        {
            _Transform(point);
        }
    }
}
