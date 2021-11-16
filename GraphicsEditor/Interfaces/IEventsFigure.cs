using System.Collections.Generic;
using System.Windows;

namespace GraphicsEditor
{
    public delegate void EventFigureGive(IFigure figure);
    public delegate void EventRemoveUiElement(List<UIElement> uIElements);
    public interface IEventsFigure
    {
        public event EventFigureGive FigureGive;
        public event EventRemoveUiElement RemoveUIElement;
    }
}
