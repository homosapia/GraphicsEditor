using GraphicsEditor.Data;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace GraphicsEditor
{
    public interface IFigure : IEventsFigure
    {
        public void Change(Point point);

        public void ChangeColor(Color color);

        public void ChangeThickness(double thick);

        public void DeselectShape();

        public void StartingPoint(Point point);

        public void MousePositionOnCanvas(Point point);

        public List<UIElement> GetAllUIElements();

        public ListOfDataToSave SerializeFigure();

        public void DeserializeFigure(ListOfDataToSave data);

        public void MoveFigure(Point point);
    }
}
