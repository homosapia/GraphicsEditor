using GraphicsEditor.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GraphicsEditor.Data
{
    public class BrokenLineDataToSave
    {
        public Point[,] points;

        public byte colorA;
        public byte colorR;
        public byte colorG;
        public byte colorB;

        public double thick;

        public int LineCount;
    }
}
