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
    public delegate void EventSetUIElement(List<UIElement> uIElements);
    public delegate void EventRemoveUiElemrnt(List<UIElement> uIElements);
    public delegate void EventFindPositionMouse();
    public interface IEvents
    {
        public event EventSelectFigure SelectObject;
        public event EventClickMarker ClickMarker;
        public event EventTransform Transform;
        public event EventSetUIElement UIElement;
        public event EventRemoveUiElemrnt RemoveUiElemrnt;
        public event EventFindPositionMouse FindPositionMouse;
    }
}
