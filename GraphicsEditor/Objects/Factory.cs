using GraphicsEditor.Abstracts;
using GraphicsEditor.Data;
using GraphicsEditor.Interfaces;
using Newtonsoft.Json;
using System;

namespace GraphicsEditor.Objects
{
    class Factory
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

        public static IFigure CreateFromData(FigureDataToSave figureData)
        {
            if (figureData.FigureType == "BrokenLineFigure")
            {
                BrokenLineFigure brokenLine = CreateBrokenLineFigure();
                brokenLine.FillWithData(figureData);
                return brokenLine;
            }

            if (figureData.FigureType == "RectangleFigure")
            {
                RectangleFigure rectangleFigure = CreateRectangleFigure();
                rectangleFigure.FillWithData(figureData);
                return rectangleFigure;
            }

            throw new Exception("there is no such figure");
        }

        public static IFigure CreateFigure(string key)
        {
            switch (key)
            {
                case "BrokenLineFigure":
                    return CreateBrokenLineFigure();

                case "RectangleFigure":
                    return CreateRectangleFigure();
            }

            throw new Exception("there is no such figure");
        }
    }
}
