
using GraphicsEditor.Objects;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GraphicsEditor
{
    class ControllerSave
    {
        private const string Brkenline = "FigureBrokenLine$";
        private const string square = "FigureRectangle$";


        List<IFigure> figures;

        public ControllerSave(){}

        public ControllerSave(List<IFigure> figures)
        {
            this.figures = figures;
        }

        public void Save()
        {
            SaveFileDialog saveFileDialog = new();
            saveFileDialog.ShowDialog();
            using (StreamWriter sw = new StreamWriter(saveFileDialog.FileName, false, Encoding.Default))
            {
                foreach (IFigure figure in figures)
                {
                    if(figure.ToString() == "GraphicsEditor.FigureRectangle")
                    {
                        string json = square;
                        json += figure.SerializeFigure();
                        sw.WriteLine(json);
                    }

                    if (figure.ToString() == "GraphicsEditor.FigureBrokenLine")
                    {
                        string json = Brkenline;
                        json += figure.SerializeFigure();
                        sw.WriteLine(json);
                    }
                }
            }
        }

        public List<IFigure> Loud()
        {
            List<IFigure> figures = new();

            OpenFileDialog openFileDialog = new();
            openFileDialog.ShowDialog();

            using (StreamReader sr = new StreamReader(openFileDialog.FileName, Encoding.Default))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Contains(Brkenline))
                    {
                        string FBL = line.Remove(0,Brkenline.IndexOf("$") + 1);
                        List<string> list = JsonConvert.DeserializeObject<List<string>>(FBL);
                        FigureBrokenLine brokenLine = new();
                        brokenLine.DeserializeFigure(list);
                        figures.Add(brokenLine);
                    }
                    if (line.Contains(square))
                    {
                        string FR = line.Remove(0, square.IndexOf("$") + 1);
                        List<string> list = JsonConvert.DeserializeObject<List<string>>(FR);
                        FigureRectangle figureRectangle = new();
                        figureRectangle.DeserializeFigure(list);
                        figures.Add(figureRectangle);
                    }
                }
            }

            return figures;
        }
    }
}
