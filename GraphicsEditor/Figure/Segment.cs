using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphicsEditor
{
    class Segment : IFigure
    {
        private Line line;
        Point [] point = new Point[2];

        List<Rectangle> marker = new();

        public object NewObject()
        {
            line = new();
            //line.MouseDown += Line_MouseDown;
            line.Stroke = SystemColors.WindowFrameBrush;
            return line;
        }

        /*
        private void Line_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            throw new NotImplementedException();
        }*/

        public void StartObject(Point point)
        {
            this.point[0] = point;
            line.X1 = point.X;
            line.Y1 = point.Y;
        }

        public void EndObject(Point point)
        {
            this.point[1] = point;
            line.X2 = point.X;
            line.Y2 = point.Y;
        }

        public object Figure()
        {
            return line;
        }

        public Point[] GetPointObject()
        {
            return point;
        }

        public void SetMarker(Rectangle [] marker)
        {
            foreach (var mar in marker)
            {
                this.marker.Add(mar);
            }
        }

        public List<Rectangle> GetMarker()
        {
            return marker;
        }

        public void DelMarker()
        {
            marker.Clear();
        }
    }
}
