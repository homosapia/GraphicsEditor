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
    public delegate void EventGetFigure(IFigure figure);
    public delegate void EventCilckMarker(bool click);
    public delegate void EventRemoveFigure(UIElement figure);

    public interface IFigure
    {
        public event EventGetFigure ReceiveFigure;
        public event EventCilckMarker ClickMarker;
        public event EventRemoveFigure RemoveFigure;

        public object Figure();

        public void ChangePosition(Point point);

        public void DelMarker();

        public void CreateFigure(Point mouse);

    }
}
