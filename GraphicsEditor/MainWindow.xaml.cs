
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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Segment_Click(object sender, RoutedEventArgs e)
        {
            workspace.paint.CreateFigure(Factory.GetFigureBrokenLine());
        }

        private void Rictangle_Click(object sender, RoutedEventArgs e)
        {
            workspace.paint.CreateFigure(Factory.GetFigureQuadrilateral());
        }

        private void Palette_SelectedBrushChanged(object sender, Syncfusion.Windows.Tools.Controls.SelectedBrushChangedEventArgs e)
        {
            workspace.paint.SetColor(palette.Color);
        }

        private void Del_Click(object sender, RoutedEventArgs e)
        {
            workspace.paint.DeleteFigure();
        }

        private void Download_Click(object sender, RoutedEventArgs e)
        {
            SaveLoad save = new();
            workspace.paint.UploadNewFigures(save.Load());
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveLoad save = new(workspace.paint.GetArrayFigures());
            save.Save();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if(workspace.paint != null)
                workspace.paint.SetThickness(slider.Value);
        }

        private void palette_RequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
        {
            workspace.paint.ShapeСhangeOff();
        }
    }
}