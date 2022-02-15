using GraphicsEditor.Abstracts;
using GraphicsEditor.Data;
using GraphicsEditor.Interfaces;
using Newtonsoft.Json;
using System;
using GraphicsEditor.Resources;

namespace GraphicsEditor.Objects
{
    class FactoryFigur : IFactoryFigur
    {
        public IFigure CreateFromData(FigureData figureData)
        {
            IFigure figure = Create(figureData.FigureType);
            figure.FillWithData(figureData);
            return figure;
        }

        public IFigure Create(string key)
        {
            if (key == DataResources.BrokenLine)
            {
                return new BrokenLineFigure();
            }
            else if(key == DataResources.RectangleFigure)
            {
                return new RectangleFigure();
            }

            throw new Exception("there is no such figure");
        }
    }
}
