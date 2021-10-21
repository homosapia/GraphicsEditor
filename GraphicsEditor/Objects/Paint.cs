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
            figure.Transform += Figure_Transform;
            figure.SelectObject += Figure_SelectObject;
            figure.ClickMarker += Figure_ClickMarker;
            figure.UIElement += Figure_UIElement;
            figure.RemoveUiElemrnt += Figure_RemoveUiElemrnt;
            figure.FindPositionMouse += Figure_FindPositionMouse;
        }


        public void Change(Point point)
        {
            if(changesAllowed)
                figure.ChangePosition(point);
        }

        public void DeselectAnObject()
        {
            figure.DeselectShape();
            figure = null;
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
            figure.CurrentPositionMouseOnCanvas(point);
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
            if (this.figure != figure)
            {
                this.figure.DeselectShape();
                this.figure = figure;
            }
            if (!canvas.Children.Contains((UIElement)figure.Figure()))
                canvas.Children.Add((UIElement)figure.Figure());
        }

        private void Figure_Transform(bool click)
        {
        }
    }
}
