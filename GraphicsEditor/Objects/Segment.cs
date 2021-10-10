using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace GraphicsEditor
{
    class Segment : IObject
    {
        private Line line;

        public object NewObject()
        {
            line = new();
            line.Stroke = SystemColors.WindowFrameBrush;
            return line;
        }
    }
}
