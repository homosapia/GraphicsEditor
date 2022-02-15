using GraphicsEditor.Abstracts;
using GraphicsEditor.Data;
using GraphicsEditor.Objects;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using GraphicsEditor.Interfaces;

namespace GraphicsEditor
{
    public partial class Workspace : UserControl
    {
        private readonly List<IFigure> figures = new();
        private string key;
        private IFigure currentFigure;
        private IFactoryFigur factoryFigur = new FactoryFigur();

        private Point previousMouse = new();

        private bool figureSelected;
        private bool placingMode;

        private double figureThickness = 1;
        private Color figureColor = Color.FromArgb(255, 0, 0, 0);

        public Workspace()
        {
            InitializeComponent();
        }

        private void Сanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (placingMode)
            {
                SetCurrentFigure(key);
                currentFigure.StartDrawing(e.GetPosition(canvas));
                canvas.Children.Add(currentFigure.GetUIElement());
                placingMode = false;
            }

            currentFigure?.CanvasMouseLeftButtonDown();
        }

        private void Сanvas_MouseMove(object sender, MouseEventArgs e)
        {
            Point currentMouse = Mouse.GetPosition(canvas);
            double deltaX = currentMouse.X - previousMouse.X;
            double deltaY = currentMouse.Y - previousMouse.Y;
            
            if  (figureSelected)
            {
                currentFigure?.ChangeToDelta(deltaX, deltaY);
            }

            if (e.RightButton == MouseButtonState.Pressed && currentFigure == null)
            {
                foreach (IFigure figure in figures)
                {
                    figure.MoveDistance(deltaX, deltaY);
                }
            }

            previousMouse = currentMouse;
        }

        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            currentFigure?.CanvasMouseLeftButtonUp();
        }


        private void Canvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (currentFigure != null)
            {
                currentFigure.RemoveSelection();
                figureSelected = false;
                currentFigure = null;
            }

            previousMouse = Mouse.GetPosition(canvas);
        }

        public void SetDrawingMode(string key)
        {
            if (currentFigure != null)
                currentFigure.RemoveSelection();

            placingMode = true;
            figureSelected = true;

            this.key = key;
        }

        public void SetCurrentFigure(string key)
        {
            currentFigure = factoryFigur.Create(key);
            currentFigure.Selected += Figure_Selected;

            figures.Add(currentFigure);
            currentFigure.SetColor(figureColor);
            currentFigure.SetThickness(figureThickness);
        }

        public WorkspaceData Save()
        {
            WorkspaceData workspaceData = new();
            foreach (IFigure figure in figures)
            {
                FigureData figureData = figure.DataSave();

                workspaceData.Figures.Add(figureData);
            }
            return workspaceData;
        }

        public void SetData(WorkspaceData workspaceData)
        {
            canvas.Children.Clear();
            figures.Clear();
            foreach (FigureData figureData in workspaceData.Figures)
            {
                IFigure figure = factoryFigur.CreateFromData(figureData);
                figures.Add(figure);
                figure.Selected += Figure_Selected;
            }

            DisplayFigures(figures);
        }

        private void DisplayFigures(List<IFigure> figures)
        {
            foreach (IFigure figure in figures)
            {
                UIElement uIs = figure.GetUIElement();
                canvas.Children.Add(uIs);
            }
        }

        public void SetColor(Color colorPalette)
        {
            figureColor = colorPalette;
            if (currentFigure != null)
            {
                currentFigure.SetColor(colorPalette);
            }
        }

        public void SetThickness(double Slider)
        {
            figureThickness = Slider;
            if (currentFigure != null)
            {
                currentFigure.SetThickness(figureThickness);
            }
        }

        public void DeleteFigure()
        {
            if (currentFigure != null)
            {
                UIElement uIElements = currentFigure.GetUIElement();
                canvas.Children.Remove(uIElements);
            }
            figures.Remove(currentFigure);
            currentFigure = null;
            figureSelected = false;
        }

        private void Figure_Selected(IFigure figure)
        {
            if (currentFigure != figure && currentFigure != null)
                currentFigure.RemoveSelection();

            figureSelected = true;
            currentFigure = figure;

            for (int i = canvas.Children.Count - 1; i >= 0; i--)
            {
                Canvas.SetZIndex(canvas.Children[i], -i);
            }

            Canvas.SetZIndex(figure.GetUIElement(), 1);
        }
    }
}
