using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphicsEditor
{
    class Paint : IPaint
    {
        List<IFigure> figures = new();
        UIElement uiElement;

        public void AppNewObject(Canvas canvas, IFigure figure)
        {
            figures.Add(figure);
            uiElement = (UIElement)figure.NewObject(canvas);

            figure.ClickMarker += mes;

            canvas.Children.Add(uiElement);
        }

        public void mes(IFigure figure)
        {
            MessageBox.Show(figure.ToString());
        }

        public UIElement GetCurrentItem()
        {
            return uiElement;
        }

        public void StartObject(Point point)
        {
            figures.Last().StartObject(point);
        }

        public void NowObject(Point point)
        {
            figures.Last().EndObject(point);
        }

        public void EndObject(Point point)
        {
            figures.Last().EndObject(point);
        }

        public void ChangePosition(Point MousePoint)
        {

        }
    }
}
