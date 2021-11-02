using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GraphicsEditor
{
    public class Paint
    {
        List<IFigure> figuresList = new();
        IFigure currentFigure;
        Canvas canvas;

        double thick = 1;
        Color color = Color.FromArgb(255,0,0,0);

        bool changesFigureAllowed;
        bool startingObject;

        public Paint(Canvas canvas)
        {
            this.canvas = canvas;
        }

        public void CreateFigure(IFigure figureFactory)
        {
            if(currentFigure != null)
            {
                currentFigure.DeselectShape();
            }

            currentFigure = null;
            startingObject = true;
            changesFigureAllowed = true;

            currentFigure = figureFactory;

            currentFigure.ChangeColor(color);
            currentFigure.ChangeThickness(thick);
            SubscribeToEvents(currentFigure);

            figuresList.Add(currentFigure);
        }

        private void SubscribeToEvents(IFigure figure)
        {
            figure.SelectObject += Figure_SelectObject;
            figure.RemoveUIElemrnt += Figure_RemoveUiElemrnt;
            figure.FindPositionMouse += Figure_FindPositionMouse;
        }

        public List<IFigure> GetArrayFigures()
        {
            return figuresList.ToList();
        }

        public void UploadNewFigures(List<IFigure> figures)
        {
            int i = 0;
            while(i != canvas.Children.Count)
            {
                canvas.Children.RemoveAt(i);
            }
            figuresList = new();

            foreach (IFigure figure in figures)
            {
                SubscribeToEvents(figure);
                List<UIElement> uIs = figure.GetAllUIElements();
                foreach(UIElement uI in uIs)
                {
                    canvas.Children.Add(uI);
                }
                figuresList.Add(figure);
            }
        }

        public void DeleteFigure()
        {
            if (currentFigure != null)
            {
                List<UIElement> uIElements = currentFigure.GetAllUIElements();
                int i = 0;
                while (i < canvas.Children.Count)
                {
                    bool uIElementsFound = false;
                    foreach (UIElement uIFigure in uIElements)
                    {
                        if (canvas.Children[i] == uIFigure)
                        {
                            canvas.Children.Remove(canvas.Children[i]);
                            uIElementsFound = true;
                            break;
                        }
                    }
                    
                    if (uIElementsFound)
                    {
                        i = 0;
                    }
                    else
                        i++;
                }
            }
            figuresList.Remove(currentFigure);
            currentFigure = null;
            changesFigureAllowed = false;
        }

        public void MoveEverything()
        {
            if(currentFigure == null)
            {
                foreach (IFigure figure in figuresList)
                {
                    figure.MoveFigure(Mouse.GetPosition(canvas));
                }
            }
        }

        public void ChangeFigure()
        {
            if(changesFigureAllowed)
                currentFigure.Change(Mouse.GetPosition(canvas));
        }

        public void DeselectAnObject()
        {
            if(currentFigure != null)
            {
                currentFigure.DeselectShape();
                changesFigureAllowed = false;
                currentFigure = null;
            }
        }

        public void ShapeСhangeOff()
        {
            currentFigure.DeselectShape();
            changesFigureAllowed = false;
        }

        public void SetInitialValues(Point point)
        {
            if (startingObject)
            {
                currentFigure.StartingPoint(point);
                startingObject = false;
            }

            foreach (IFigure figure in figuresList)
            {
                figure.MousePositionOnCanvas(point);
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
            thick = Slider;
            if (currentFigure != null)
            {
                currentFigure.ChangeThickness(thick);
            }
        }


        private void Figure_FindPositionMouse()
        {
            currentFigure.MousePositionOnCanvas(Mouse.GetPosition(canvas));
        }

        private void Figure_RemoveUiElemrnt(List<UIElement> uIElements)
        {
            foreach (UIElement uI in uIElements)
            {
                canvas.Children.Remove(uI);
            }
        }

        private void Figure_SelectObject(IFigure figure)
        {
            changesFigureAllowed = true;
            if (currentFigure != figure && currentFigure != null)
            {
                currentFigure.DeselectShape();
            }
            currentFigure = figure;

            List<UIElement> uIs = currentFigure.GetAllUIElements();
            for (int i = canvas.Children.Count - 1; i >= 0; i--)
            {
                Canvas.SetZIndex(canvas.Children[i], -i);
            }

            foreach (UIElement uI in uIs)
            {
                Canvas.SetZIndex(uI, 1);
                if(!canvas.Children.Contains(uI))
                    canvas.Children.Add(uI);
            }
        }
    }
}
