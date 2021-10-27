using GraphicsEditor.Interfaces;

namespace GraphicsEditor.Objects
{
    class Factory : IFactory
    {
        public FigureBrokenLine GetFigureBrokenLine()
        {
            FigureBrokenLine brokenLine = new();
            return brokenLine;
        }

        public FigureQuadrilateral GetFigureQuadrilateral()
        {
            FigureQuadrilateral quadrilateral = new();
            return quadrilateral;
        }
    }
}
