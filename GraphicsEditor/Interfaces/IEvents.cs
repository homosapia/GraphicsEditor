using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace GraphicsEditor
{
    public delegate void EventSelectFigure(IFigure figure);
    public delegate void EventClickMarker(bool click);
    public delegate void EventTransform(bool click);
    public delegate void EventSetMarker(List<Ellipse> markers);
    public delegate void EventRemoveMarker();
    public delegate void EventRemoveUiElemrnt(UIElement uIElement);
    public interface IEvents
    {
        public event EventSelectFigure SelectObject;
        public event EventClickMarker ClickMarker;
        public event EventTransform Transform;
        public event EventSetMarker SetMarker;
        public event EventRemoveMarker RemoveMarker;
        public event EventRemoveUiElemrnt RemoveUiElemrnt;
    }
}
