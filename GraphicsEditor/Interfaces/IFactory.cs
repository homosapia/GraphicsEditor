﻿
namespace GraphicsEditor.Interfaces
{
    public interface IFactory
    {
        public FigureBrokenLine GetFigureBrokenLine();

        public FigurePaddedRectangle GetFigureQuadrilateral();
    }
}
