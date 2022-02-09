using GraphicsEditor.Abstracts;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace GraphicsEditor
{
    public delegate void EventSelectFigure(IFigure figure);
    public delegate void EventRemoveUiElements(List<UIElement> uIElements);
    public delegate void EventAddUiElements(List<UIElement> uIElements);

    public interface IFigure
    {
        public event EventSelectFigure SelectFigure;
        public event EventRemoveUiElements RemoveUiElements;
        public event EventAddUiElements AddUiElements;

        public void ChangeToDelta(double deltaX, double deltaY);

        public void SetColor(Color color);

        public void SetThickness(double thick);

        public void RemoveSelection();

        public void StartDrawing(Point point);

        public void CanvasMouseLeftButtonDown();

        public void CanvasMouseLeftButtonUp();

        public List<UIElement> GetAllUIElements();

        public FigureDataToSave GetDataToSave();

        public void FillWithData(FigureDataToSave json);

        public void MoveDistance(double deltaX, double deltaY);
    }
}
