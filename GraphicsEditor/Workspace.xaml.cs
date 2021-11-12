using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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
                currentFigure.StartingPoint(e.GetPosition(canvas));
                drawingMode = false;
            }

            foreach (IFigure figure in figures)
            {
                figure.MousePositionOnCanvas(e.GetPosition(canvas));
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
                figure.MousePositionOnCanvas(e.GetPosition(canvas));
            }
        }

        public void SetCurrentFigure(IFigure figure)
        {
            if(currentFigure != null) 
                currentFigure.DeselectShape();
            
            currentFigure = null;
            drawingMode = true;
            figureSelected = true;

            currentFigure = figure;

            currentFigure.ChangeColor(color);
            currentFigure.ChangeThickness(figureThickness);
            SubscribeToEvents(currentFigure);
            figures.Add(currentFigure);
        }

        public List<IFigure> GetArrayFigures()
        {
            return figures.ToList();
        }

        private void SubscribeToEvents(IFigure figure)
        {
            figure.SelectObject += Figure_SelectObject;
            figure.RemoveUIElement += FigureRemoveUiElement;
        }

        public void UploadNewFigures(List<IFigure> figures)
        {
            canvas.Children.Clear();

            this.figures = new();

            foreach (IFigure figure in figures)
            {
                SubscribeToEvents(figure);
                List<UIElement> uIs = figure.GetAllUIElements();
                foreach (UIElement uI in uIs)
                {
                    canvas.Children.Add(uI);
                }
                this.figures.Add(figure);
            }
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
            currentFigure.MousePositionOnCanvas(Mouse.GetPosition(canvas));
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
                VARIABLE.MousePositionOnCanvas(Mouse.GetPosition(canvas));
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
