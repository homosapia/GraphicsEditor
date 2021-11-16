using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using GraphicsEditor.Abstracts;
using GraphicsEditor.Data;
using GraphicsEditor.Objects;
using Newtonsoft.Json;

namespace GraphicsEditor
{
    public partial class Workspace : UserControl
    {

        private List<IFigure> figures = new();
        private IFigure currentFigure;

        private bool figureSelected;
        private bool drawingMode;

        private double figureThickness = 1;
        private Color color = Color.FromArgb(255, 0, 0, 0);

        public Workspace()
        {
            InitializeComponent();
        }

        private void Сanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (drawingMode)
            {
                currentFigure.StartDrawing(e.GetPosition(canvas));
                drawingMode = false;
            }

            foreach (IFigure figure in figures)
            {
                figure.StartMoving(e.GetPosition(canvas));
            }
        }

        private void Сanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && figureSelected)
            {
                currentFigure.Change(Mouse.GetPosition(canvas));
            }

            if (e.RightButton == MouseButtonState.Pressed && currentFigure == null)
            {
                foreach (IFigure figure in figures)
                {
                    figure.MoveFigure(Mouse.GetPosition(canvas));
                }
            }
        }
        

        private void canvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (currentFigure != null)
            {
                currentFigure.DeselectShape();
                figureSelected = false;
                currentFigure = null;
            }

            foreach (IFigure figure in figures)
            {
                figure.StartMoving(e.GetPosition(canvas));
            }
        }

        public void SetCurrentFigure(string key)
        {
            if(currentFigure != null) 
                currentFigure.DeselectShape();
            
            currentFigure = null;
            drawingMode = true;
            figureSelected = true;

            currentFigure = Factory.CreateFigure(key);

            currentFigure.ChangeColor(color);
            currentFigure.ChangeThickness(figureThickness);
            Sign(currentFigure);
            figures.Add(currentFigure);
        }

        public List<IFigure> Get()
        {
            return figures.ToList();
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

        public void UploadNewFigures(WorkspaceDataToSave workspaceData)
        {
            canvas.Children.Clear();
            figures.Clear();
            foreach (FigureDataToSave figureData in workspaceData.Figures)
            {
                IFigure figure = Factory.CreateFromData(figureData);
                figures.Add(figure);
                Sign(figure);
            }

            DisplayFigurs(figures);
        }

        private void DisplayFigurs(List<IFigure> figures)
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
            figure.FigureGive += Figure_SelectObject;
            figure.RemoveUIElement += FigureRemoveUiElement;
        }

        public void SetColor(Color colorPalette)
        {
            color = colorPalette;
            if (currentFigure != null)
            {
                currentFigure.ChangeColor(colorPalette);
            }
        }

        public void SetThickness(double Slider)
        {
            figureThickness = Slider;
            if (currentFigure != null)
            {
                currentFigure.ChangeThickness(figureThickness);
            }
        }

        public void ShapeHangeOff()
        {
            try
            {
                currentFigure.DeselectShape();
                figureSelected = false;
            }
            catch {}
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

        private void Figure_FindPositionMouse()
        {
            currentFigure.StartMoving(Mouse.GetPosition(canvas));
        }

        private void FigureRemoveUiElement(List<UIElement> uIElements)
        {
            foreach (UIElement uI in uIElements)
            {
                canvas.Children.Remove(uI);
            }
        }

        private void Figure_SelectObject(IFigure figure)
        {
            figureSelected = true;
            if (currentFigure != figure && currentFigure != null)
            {
                currentFigure.DeselectShape();
            }
            currentFigure = figure;

            foreach (var VARIABLE in figures)
            {
                VARIABLE.StartMoving(Mouse.GetPosition(canvas));
            }

            for (int i = canvas.Children.Count - 1; i >= 0; i--)
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
