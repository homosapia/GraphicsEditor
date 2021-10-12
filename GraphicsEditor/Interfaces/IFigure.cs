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
    public delegate void EventClickMarker(IFigure figure);

    public interface IFigure
    {
        public event EventClickMarker ClickMarker;
        public object NewObject(Canvas canvas);
        public object Figure();
        public void StartObject(Point point);
        public void EndObject(Point point);

        public Point[] GetPointObject();

        public void SetMarker();

        public void ChangePosition(Point point);

        public void DelMarker();

    }
}
