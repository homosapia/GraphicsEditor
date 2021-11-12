using System.Collections.Generic;
using System.Windows;

namespace GraphicsEditor
{
    public delegate void EventSelectFigure(IFigure figure);
    public delegate void EventRemoveUiElement(List<UIElement> uIElements);
    public interface IEventsFigure
    {
        public event EventSelectFigure SelectObject;
        public event EventRemoveUiElement RemoveUIElement;
    }
}
