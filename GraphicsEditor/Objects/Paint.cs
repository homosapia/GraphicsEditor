using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GraphicsEditor
{
    class Paint
    {
        List<IFigure> figures = new();
        IFigure figure;
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
            if(figure != null)
            {
                figure.DeselectShape();
            }

            figure = null;
            startingObject = true;
            changesFigureAllowed = true;

            figure = figureFactory;

            figure.ChangeColor(color);
            figure.ChangeThickness(thick);
            SubscribeToEvents(figure);

            figures.Add(figure);
        }

        private void SubscribeToEvents(IFigure figure)
        {
            figure.SelectObject += Figure_SelectObject;
            figure.RemoveUIElemrnt += Figure_RemoveUiElemrnt;
            figure.FindPositionMouse += Figure_FindPositionMouse;
        }

        public List<IFigure> GetArrayFigures()
        {
            return figures.ToList();
        }

        public void UploadNewFigures(List<IFigure> figures)
        {
            int i = 0;
            while(i != canvas.Children.Count)
            {
                canvas.Children.RemoveAt(i);
            }
            this.figures = new();

            foreach (IFigure figure in figures)
            {
                SubscribeToEvents(figure);
                List<UIElement> uIs = figure.GetAllUIElements();
                foreach(UIElement uI in uIs)
                {
                    canvas.Children.Add(uI);
                }
                this.figures.Add(figure);
            }
        }

        public void DeleteFigure()
        {
            if (figure != null)
            {
                List<UIElement> uIElements = figure.GetAllUIElements();
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
            figures.Remove(figure);
            figure = null;
            changesFigureAllowed = false;
        }

        public void MoveEverything()
        {
            if(figure == null)
            {
                foreach (IFigure figure in figures)
                {
                    figure.MoveFigure(Mouse.GetPosition(canvas));
                }
            }
        }

        public void ChangeFigure()
        {
            if(changesFigureAllowed)
                figure.Change(Mouse.GetPosition(canvas));
        }

        public void DeselectAnObject()
        {
            if(figure != null)
            {
                figure.DeselectShape();
                changesFigureAllowed = false;
                figure = null;
            }
        }

        public void SetInitialValues(Point point)
        {
            if (startingObject)
            {
                figure.StartingPoint(point);
                startingObject = false;
            }

            foreach (IFigure figure in figures)
            {
                figure.CurrentPositionMouseOnCanvas(point);
            }
        }

        public void SetColor(Color color)
        {
            this.color = color;
            if (figure != null)
            {
                figure.ChangeColor(color);
            }
        }

        public void SetThickness(double thick)
        {
            this.thick = thick;
            if (figure != null)
            {
                figure.ChangeThickness(this.thick);
            }
        }


        private void Figure_FindPositionMouse()
        {
            figure.CurrentPositionMouseOnCanvas(Mouse.GetPosition(canvas));
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
            if (this.figure != figure && this.figure != null)
            {
                this.figure.DeselectShape();
            }
            this.figure = figure;

            List<UIElement> uIs = this.figure.GetAllUIElements();
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
