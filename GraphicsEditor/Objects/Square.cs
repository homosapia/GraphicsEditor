using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphicsEditor
{
    class Square : IObject
    {
        private Rectangle rectangle;

        public object NewObject()
        {
            rectangle = new();
            rectangle.Fill = new SolidColorBrush(Colors.Black);
            return rectangle;
        }

        public void 
    }
}
