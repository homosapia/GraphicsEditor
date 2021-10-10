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
        public object NewObject(IObject obj);

        public void StartObject(Point point);

        public void AppNewObject(Canvas canvas, IObject obj);

        public UIElement GetCurrentItem();

        public void NowObject(Point point);
    }
}
