using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace GraphicsEditor
{
    public interface IFigure
    {
        public object NewObject();
        public object Figure();
        public void StartObject(Point point);
        public void EndObject(Point point);

        public Point[] GetPointObject();

        public void SetMarker(Rectangle[] marker);

        public List<Rectangle> GetMarker();

        public void DelMarker();
    }
}
