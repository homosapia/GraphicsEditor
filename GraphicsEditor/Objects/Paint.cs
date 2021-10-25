using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        bool changesAllowed;
        bool startingObject;

        public Paint(Canvas canvas)
        {
            this.canvas = canvas;
        }

        //создать фигуру
        public void CreateFigure(string nameFigure)
        {
            if(figure != null)
            {
                figure.DeselectShape();
            }

            figure = null;
            startingObject = true;
            changesAllowed = true;

            switch (nameFigure)
            {
                case "Линия":
                    figure = new FigureBrokenLine();
                    break;
                case "Прямоугольник":
                    figure = new FigureRectangle();
                    break;
            }

            figure.ChangeColor(color);
            figure.ChangeThickness(thick);
            SubscribeToEvents(figure);
        }

        public void SubscribeToEvents(IFigure figure)
        {
            figure.Transform += Figure_Transform;
            figure.SelectObject += Figure_SelectObject;
            figure.ClickMarker += Figure_ClickMarker;
            figure.UIElement += Figure_UIElement;
            figure.RemoveUiElemrnt += Figure_RemoveUiElemrnt;
            figure.FindPositionMouse += Figure_FindPositionMouse;
        }

        //получить копию
        public List<IFigure> GetArrayFigures()
        {
            return figures;
        }

        //установить вигуры
        public void UploadNewFigures(List<IFigure> figures)
        {
            foreach (IFigure figure in figures)
            {
                figure.TuneElements();
                SubscribeToEvents(figure);
                List<UIElement> uIs = figure.GetAllUIElements();
                foreach(UIElement uI in uIs)
                {
                    canvas.Children.Add(uI);
                }
            }
        }


        //удалить фигуру
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
            figure = null;
        }

        public void MoveEverything(Point point)
        {
            if(figure == null)
            {
                foreach (IFigure figure in figures)
                {
                    figure.MoveFigure(point);
                }
            }
        }

        public void Change(Point point)
        {
            if(changesAllowed)
                figure.ChangePosition(point);
        }

        public void DeselectAnObject()
        {
            if(figure != null)
            {
                figure.DeselectShape();
                changesAllowed = false;
                figure = null;
            }
        }

        public void ClickMouseDown(Point point)
        {
            Starting(point);
            SendMousePosition(point);
        }

        private void Starting(Point point)
        {
            if (startingObject)
            {
                figure.StartingPoint(point);
                startingObject = false;
            }
        }

        public void SendMousePosition(Point point)
        {
            foreach (IFigure figure in figures)
            {
                figure.CurrentPositionMouseOnCanvas(point);
            }
        }

        public void SetColor(Color color)
        {
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

        //события
        private void Figure_FindPositionMouse()
        {
            figure.CurrentPositionMouseOnCanvas(Mouse.GetPosition(canvas));
        }

        private void Figure_RemoveUiElemrnt(List<System.Windows.UIElement> uIElements)
        {
            foreach (UIElement uI in uIElements)
            {
                canvas.Children.Remove(uI);
            }
        }

        private void Figure_UIElement(List<UIElement> uIElements)
        {
            foreach (UIElement ui in uIElements)
            {
                canvas.Children.Add(ui);
            }
        }

        private void Figure_ClickMarker(bool click)
        {
            changesAllowed = click;
        }

        private void Figure_SelectObject(IFigure figure)
        {
            bool AddToArray = true;
            foreach(IFigure figure1 in figures)
            {
                if (figure1 == figure)
                    AddToArray = false;
            }
            if (AddToArray)
                figures.Add(figure);

            changesAllowed = true;
            if (this.figure != figure && this.figure != null)
            {
                this.figure.DeselectShape();
            }
            this.figure = figure;
            if (!canvas.Children.Contains((UIElement)figure.Figure()))
                canvas.Children.Add((UIElement)figure.Figure());
        }

        private void Figure_Transform(bool click)
        {
        }
    }
}
