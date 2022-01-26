using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphicsEditor.Abstracts;

namespace GraphicsEditor.Interfaces
{
    public interface IFactory
    {
        public IFigure CreateFromData(FigureDataToSave figureData);

        public IFigure CreateFigure(string key);
    }
}
