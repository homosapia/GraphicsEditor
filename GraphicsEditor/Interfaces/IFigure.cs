using GraphicsEditor.Data;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using GraphicsEditor.Abstracts;

namespace GraphicsEditor
{
    public interface IFigure : IEventsFigure
    {
        public void Change(Point point);

        public void ChangeColor(Color color);

        public void ChangeThickness(double thick);

        public void DeselectShape();

        public void StartDrawing(Point point);

        public void StartMoving(Point point);

        public List<UIElement> GetAllUIElements();

        public FigureDataToSave GetDataToSave();

        public void FillWithData(FigureDataToSave json);

        public void MoveFigure(Point point);
    }
}
