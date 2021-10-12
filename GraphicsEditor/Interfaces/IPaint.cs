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
    public interface IPaint
    {
        public void StartObject(Point point);

        public void AppNewObject(Canvas canvas, IFigure obj);

        public UIElement GetCurrentItem();

        public void NowObject(Point point);

        public void EndObject(Point point);

        public void ChangePosition(Point MousePoint);

    }
}
