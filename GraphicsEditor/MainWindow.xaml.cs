﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GraphicsEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IFigure figure;

        bool transform;

        Point currentPoint = new();

        public MainWindow()
        {   
            InitializeComponent();
        }

        private void canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(!canvas.Children.Contains((UIElement)figure.Figure()))
            {
                transform = true;
                figure.CreateFigure(e.GetPosition(canvas));
                canvas.Children.Add((UIElement)figure.Figure());
            }
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed && transform)
            {
                figure.ChangePosition(e.GetPosition(canvas));
            }
        }

        private void canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            transform = false;
        }

        private void segment_Click(object sender, RoutedEventArgs e)
        {
            figure = new Segment(canvas);
            figure.GetFigure += СurrentFigure;
            figure.ClickMarker += ShowMarker;
        }

        private void rictangle_Click(object sender, RoutedEventArgs e)
        {
            figure = new Square();
            figure.GetFigure += СurrentFigure;
            figure.ClickMarker += ShowMarker;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        public void СurrentFigure(IFigure figure)
        {
            this.figure = figure;
        }

        public void ShowMarker(bool click)
        {
            transform = click;
        }
    }
}
