
using GraphicsEditor.Interfaces;
using GraphicsEditor.Objects;
using Newtonsoft.Json;
using System;
using System.Windows;
using System.Windows.Input;

namespace GraphicsEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool СolorСhange;
        Paint paint;
        IFactory factory;
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            paint = new(canvas);
            factory = new Factory();
        }

        private void canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
                paint.DeselectAnObject();

            paint.SetInitialValues(e.GetPosition(canvas));
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed && !СolorСhange)
            {
                paint.ChangeFigure();
                paint.MoveEverything();
            }
            СolorСhange = false;
        }

        private void segment_Click(object sender, RoutedEventArgs e)
        {
            paint.CreateFigure(factory.GetFigureBrokenLine());
        }

        private void rictangle_Click(object sender, RoutedEventArgs e)
        {
            paint.CreateFigure(factory.GetFigureQuadrilateral());
        }

        private void palette_SelectedBrushChanged(object sender, Syncfusion.Windows.Tools.Controls.SelectedBrushChangedEventArgs e)
        {
            СolorСhange = true;
            paint.SetColor(palette.Color);
        }

        private void del_Click(object sender, RoutedEventArgs e)
        {
            paint.DeleteFigure();
        }

        private void download_Click(object sender, RoutedEventArgs e)
        {
            SaveLoad save = new();
            paint.UploadNewFigures(save.Load());
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveLoad save = new(paint.GetArrayFigures());
            save.Save();
        }

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if(paint != null)
                paint.SetThickness(slider.Value);
        }
    }
}
