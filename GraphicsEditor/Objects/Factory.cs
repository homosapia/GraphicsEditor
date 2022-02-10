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
        private static BrokenLineFigure CreateBrokenLineFigure()
        {
            BrokenLineFigure brokenLine = new();
            return brokenLine;
        }

        private static RectangleFigure CreateRectangleFigure()
        {
            RectangleFigure quadrilateral = new();
            return quadrilateral;
        }

        public IFigure CreateFromData(FigureDataToSave figureData)
        {
            if (figureData.FigureType == DataResources.BrokenLine)
            {
                IFigure brokenLine = CreateFigure(DataResources.BrokenLine);
                brokenLine.FillWithData(figureData);
                return brokenLine;
            }

            if (figureData.FigureType == DataResources.RectangleFigure)
            {
                IFigure rectangleFigure = CreateFigure(DataResources.RectangleFigure);
                rectangleFigure.FillWithData(figureData);
                return rectangleFigure;
            }

            throw new Exception("there is no such figure");
        }

        public IFigure CreateFigure(string key)
        {
            if (key == DataResources.BrokenLine)
            {
                return CreateBrokenLineFigure();
            }
            else if(key == DataResources.RectangleFigure)
            {
                return CreateRectangleFigure();
            }

            throw new Exception("there is no such figure");
        }
    }
}
