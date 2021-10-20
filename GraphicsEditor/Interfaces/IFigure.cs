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

        public void DrawFigure(Point point);

        public void CreateFigure(Point mouse);

        public void ChangeColor(Color color);

        public void ChangeThickness(double thick);
    }
}
