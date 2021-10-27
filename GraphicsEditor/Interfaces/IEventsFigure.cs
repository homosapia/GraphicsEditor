using System.Collections.Generic;
using System.Windows;

namespace GraphicsEditor
{
    public delegate void EventSelectFigure(IFigure figure);
    public delegate void EventSetUIElement(List<UIElement> uIElements);
    public delegate void EventRemoveUiElemrnt(List<UIElement> uIElements);
    public delegate void EventFindPositionMouse();
    public interface IEventsFigure
    {
        public event EventSelectFigure SelectObject;
        public event EventRemoveUiElemrnt RemoveUIElemrnt;
        public event EventFindPositionMouse FindPositionMouse;
    }
}
