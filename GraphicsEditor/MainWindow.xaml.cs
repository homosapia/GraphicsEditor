
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
            workspace.SetCurrentFigure(Factory.GetFigureBrokenLine());
        }

        private void Rictangle_Click(object sender, RoutedEventArgs e)
        {
            workspace.SetCurrentFigure(Factory.GetFigureQuadrilateral());
        }

        private void Palette_SelectedBrushChanged(object sender, Syncfusion.Windows.Tools.Controls.SelectedBrushChangedEventArgs e)
        {
            workspace.SetColor(palette.Color);
        }

        private void Del_Click(object sender, RoutedEventArgs e)
        {
            workspace.DeleteFigure();
        }

        private void Download_Click(object sender, RoutedEventArgs e)
        {
            SaveLoad save = new();
            workspace.UploadNewFigures(save.Load());
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveLoad save = new(workspace.GetArrayFigures());
            save.Save();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
             workspace.SetThickness(slider.Value);
        }

        private void palette_RequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
        {
            workspace.ShapeHangeOff();
        }
    }
}