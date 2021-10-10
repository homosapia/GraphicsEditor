using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace GraphicsEditor
{
    class Paint : IPaint
    {
        UIElement element;
        public void AppNewObject(Canvas canvas, IObject obj)
        {
            element = (UIElement)obj.NewObject();
            canvas.Children.Add(element);
        }

        public UIElement GetCurrentItem()
        {
            return element;
        }

        public object NewObject(IObject obj)
        {
            return obj.NewObject();
        }

        public void StartObject(Point point)
        {
            Canvas.SetLeft(element, point.X);
            Canvas.SetTop(element, point.Y);
        }

        public void NowObject(Point point)
        {
            Canvas.SetRight(element, point.X);
            Canvas.SetBottom(element, point.Y);
        }
    }
}
