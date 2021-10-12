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
        IFigure Element;

        public void AppNewObject(Canvas canvas, IFigure figure)
        {
            Element = figure;

            figure.ClickMarker += Mes;

            canvas.Children.Add((UIElement)figure.NewObject(canvas));
        }

        public void Mes(IFigure figure)
        {
            Element = figure;
        }

        public void StartObject(Point point)
        {
            Element.StartObject(point);
        }

        public void NowObject(Point point)
        {
            Element.EndObject(point);
        }

        public void EndObject(Point point)
        {
            Element.EndObject(point);
        }

        public void ChangePosition(Point MousePoint)
        {
            Element.ChangePosition(MousePoint);
        }
    }
}
