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
        private ParentContainer parentContainer;
        private readonly List<IFigure> figures = new();
        private string key;
        private IFigure currentFigure;
        private IFactory factory = new Factory();

        private Point firstClickPosition = new();

        private bool figureSelected;
        private bool placingMode;

        private double figureThickness = 1;
        private Color color = Color.FromArgb(255, 0, 0, 0);

        public Workspace()
        {
            InitializeComponent();
            parentContainer = new ParentContainer(canvas);
        }

        private void Сanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (placingMode)
            {
                SetCurrentFigure(key);
                currentFigure.StartDrawing(e.GetPosition(canvas));
                placingMode = false;
            }

            currentFigure?.CanvasMouseLeftButtonDown();
        }

        private void Сanvas_MouseMove(object sender, MouseEventArgs e)
        {
            Point CurrentMouse = Mouse.GetPosition(canvas);
            double deltaX = CurrentMouse.X - firstClickPosition.X;
            double deltaY = CurrentMouse.Y - firstClickPosition.Y;
            
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

            firstClickPosition = CurrentMouse;
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

            firstClickPosition = Mouse.GetPosition(canvas);
        }

        public void SetKeyFigure(string key)
        {
            if (currentFigure != null)
                currentFigure.RemoveSelection();

            placingMode = true;
            figureSelected = true;

            this.key = key;
        }

        public void SetCurrentFigure(string key)
        {
            currentFigure = factory.CreateFigure(key);
            Sign(currentFigure);

            currentFigure.SetColor(color);
            currentFigure.SetThickness(figureThickness);
            figures.Add(currentFigure);
        }

        public WorkspaceDataToSave GetDataToSave()
        {
            WorkspaceDataToSave workspaceData = new();
            foreach (IFigure figure in figures)
            {
                FigureDataToSave figureData = figure.GetDataToSave();

                workspaceData.Figures.Add(figureData);
            }
            return workspaceData;
        }

        public void SetWorkspaceData(WorkspaceDataToSave workspaceData)
        {
            canvas.Children.Clear();
            figures.Clear();
            foreach (FigureDataToSave figureData in workspaceData.Figures)
            {
                IFigure figure = factory.CreateFromData(figureData);
                figures.Add(figure);
                Sign(figure);
            }

            DisplayFigures(figures);
        }

        private void DisplayFigures(List<IFigure> figures)
        {
            foreach (IFigure figure in figures)
            {
                List<UIElement> uIs = figure.GetAllUIElements();
                foreach (UIElement uI in uIs)
                {
                    canvas.Children.Add(uI);
                }
            }
        }

        private void Sign(IFigure figure)
        {
            figure.SelectFigure += SelectedFigure;
        }

        public void SetColor(Color colorPalette)
        {
            color = colorPalette;
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
                List<UIElement> uIElements = currentFigure.GetAllUIElements();
                foreach (UIElement uIFigure in uIElements)
                {
                    canvas.Children.Remove(uIFigure);
                }
            }
            figures.Remove(currentFigure);
            currentFigure = null;
            figureSelected = false;
        }

        private void SelectedFigure(IFigure figure)
        {
            figureSelected = true;
            if (currentFigure != figure)
            {
                currentFigure?.RemoveSelection();
            }
            currentFigure = figure;
            
            for(int i = canvas.Children.Count - 1; i >= 0; i--)
            {
                Canvas.SetZIndex(canvas.Children[i], -i);
            }

            List<UIElement> uIs = currentFigure.GetAllUIElements();
            foreach (UIElement uI in uIs)
            {
                Canvas.SetZIndex(uI, 1);
                if (!canvas.Children.Contains(uI))
                    canvas.Children.Add(uI);
            }
        }
    }
}
