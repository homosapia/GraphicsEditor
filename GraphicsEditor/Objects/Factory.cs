using GraphicsEditor.Abstracts;
using GraphicsEditor.Data;
using GraphicsEditor.Interfaces;
using Newtonsoft.Json;
using System;
using GraphicsEditor.Resources;

namespace GraphicsEditor.Objects
{
    class Factory : IFactory
    {
        public IFigure CreateFromData(FigureDataToSave figureData)
        {
            IFigure figure = CreateFigure(figureData.FigureType);
            figure.FillWithData(figureData);
            return figure;
        }

        public IFigure CreateFigure(string key)
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
