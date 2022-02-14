using GraphicsEditor.Abstracts;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using GraphicsEditor.Interfaces;
using GraphicsEditor.Objects;

namespace GraphicsEditor
{
    public delegate void EventSelectFigure(IFigure figure);

    public interface IFigure
    {
        public event EventSelectFigure SelectFigure;

        public void SetParentContainer(ParentContainer parentContainer);

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
