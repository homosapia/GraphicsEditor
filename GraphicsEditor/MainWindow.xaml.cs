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
        Paint paint;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
                paint.DeselectAnObject();

            paint.ClickMouseDown(e.GetPosition(canvas));
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                paint.Change(e.GetPosition(canvas));
            }
        }

        private void segment_Click(object sender, RoutedEventArgs e)
        {
            paint.CreateFigure("Линия");
        }

        private void rictangle_Click(object sender, RoutedEventArgs e)
        {
            paint.CreateFigure("Прямоугольник");
        }

        private void palette_SelectedBrushChanged(object sender, Syncfusion.Windows.Tools.Controls.SelectedBrushChangedEventArgs e)
        {
            paint.SetColor(palette.Color);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            paint = new(canvas);
        }

        private void slider_MouseDown(object sender, MouseButtonEventArgs e)
        {
            paint.SetThickness(slider.Value);
        }
    }
}
