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
    public delegate void EventSelectFigure(IFigure figure);
    public delegate void EventClickMarker(bool click);
    public delegate void EventTransform(bool click);
    public delegate void EventSetMarker(List<Rectangle> markers);
    public delegate void EventRemoveMarker(List<Rectangle> markers);

    public interface IFigure
    {
        public event EventSelectFigure SelectObject;
        public event EventClickMarker ClickMarker;
        public event EventTransform Transform;
        public event EventSetMarker SetMarker;
        public event EventRemoveMarker RemoveMarker;

        public object Figure();

        public void ChangePosition(Point point);

        public void DrawFigure(Point point);

        public void DelMarker();

        public void CreateFigure(Point mouse);

    }
}
