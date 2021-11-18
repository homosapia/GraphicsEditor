﻿
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
        private readonly IWorkspaceStorage Storage = new WorkspaceStorage();
        private const string RectangleFigureType = "RectangleFigure";
        private const string BrokenLineFigureType = "BrokenLineFigure";
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BrokenLine_Click(object sender, RoutedEventArgs e)
        {
            workspace.SetCurrentFigure(BrokenLineFigureType);
        }

        private void Rectangle_Click(object sender, RoutedEventArgs e)
        {
            workspace.SetCurrentFigure(RectangleFigureType);
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
            workspace.SetWorkspaceData(Storage.Load());
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Storage.Save(workspace.GetDataToSave());
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
             workspace.SetThickness(slider.Value);
        }
    }
}