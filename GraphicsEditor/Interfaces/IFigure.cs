using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphicsEditor
{
    public interface IFigure : IEvents
    {
        public object Figure();

        public void ChangePosition(Point point);

        public void ChangeColor(Color color);

        public void ChangeThickness(double thick);

        public void DeselectShape();

        public void StartingPoint(Point point);

        public void CurrentPositionMouseOnCanvas(Point point);

        public List<UIElement> GetAllUIElements();

        public string SerializeFigure();

        public void DeserializeFigure(List<string> objects);

        public void TuneElements();

        public IFigure GetCopyIFigure();

        public void MoveFigure(Point point);
    }
}
