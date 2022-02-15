using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphicsEditor.Abstracts;

namespace GraphicsEditor.Interfaces
{
    public interface IFactoryFigur
    {
        public IFigure CreateFromData(FigureData figureData);
        public IFigure Create(string key);
    }
}