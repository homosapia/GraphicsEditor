using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GraphicsEditor.Abstracts;

namespace GraphicsEditor.Data
{
    class RectangleDataToSave
    {
        public byte colorA;
        public byte colorR;
        public byte colorG;
        public byte colorB;

        public double Rotate;

        public double Width;
        public double Height;

        public Point position;
    }
}
