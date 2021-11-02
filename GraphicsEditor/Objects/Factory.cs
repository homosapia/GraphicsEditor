using GraphicsEditor.Data;
using GraphicsEditor.Interfaces;
using Newtonsoft.Json;
using System;

namespace GraphicsEditor.Objects
{
    class Factory
    {
        public static FigureBrokenLine GetFigureBrokenLine()
        {
            FigureBrokenLine brokenLine = new();
            return brokenLine;
        }

        public static FigurePaddedRectangle GetFigureQuadrilateral()
        {
            FigurePaddedRectangle quadrilateral = new();
            return quadrilateral;
        }

        public static string Serialize(IFigure figure)
        {
            try
            {
                ListOfDataToSave data = figure.SerializeFigure();
                string json = JsonConvert.SerializeObject(data);
                return json;
            }
            catch { }

            throw new Exception("Unable to serialize");
        }

        public static IFigure Deserialize(string line)
        {
            try
            {
                ListOfDataToSave data = JsonConvert.DeserializeObject<ListOfDataToSave>(line);

                try
                {
                    FigureBrokenLine brokenLine = new();
                    brokenLine.DeserializeFigure(data);
                    return brokenLine;
                }
                catch { }

                try
                {
                    FigurePaddedRectangle figureRectangle = new();
                    figureRectangle.DeserializeFigure(data);
                    return figureRectangle;
                }
                catch { }
            }
            catch { }

            throw new Exception("Unable to Deserialize");
        }
    }
}
